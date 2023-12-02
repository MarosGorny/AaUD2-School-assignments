using DynamicHashingDS.Data;
using DynamicHashingDS.DH;
using System.Collections;
using System.Security;

namespace DynamicHashingDS.Nodes;

/// <summary>
/// Represents an external node in a dynamic hashing trie structure.
/// </summary>
/// <typeparam name="T">The type of the record.</typeparam>
public class DHExternalNode<T> : DHNode<T> where T : IDHRecord<T>, new()
{
    public int _recordsCount;  //FIXME: JUST FOR NOW PUBLIC
    private int _blockAddress;
    private readonly FileBlockManager<T> _fileBlockManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="DHExternalNode{T}"/> class.
    /// </summary>
    /// <param name="dynamicHashing">The dynamic hashing instance.</param>
    /// <param name="parent">The parent node of this external node.</param>
    /// <param name="blockAddress">The block address of this external node.</param>
    public DHExternalNode(DynamicHashing<T> dynamicHashing, DHNode<T> parent,int blockAdress) : base(dynamicHashing,parent)
    {
        _recordsCount = 0;
        _blockAddress = blockAdress;
        _fileBlockManager = dynamicHashing.FileBlockManager;
    }

    /// <summary>
    /// Sets the block address of this external node.
    /// </summary>
    /// <param name="blockAddress">The new block address.</param>
    public void SetBlockAddress(int blockAddress)
    {
        _blockAddress = blockAddress;
    }

    /// <summary>
    /// Attempts to insert a record into the external node. If the node is full, it handles splitting and redistribution.
    /// </summary>
    /// <param name="record">The record to insert.</param>
    /// <returns>True if the record was inserted, or false if it needs to be handled as an overflow.</returns>
    public override bool Insert(IDHRecord<T> record)
    {
        if (_recordsCount < dynamicHashing.MainBlockFactor)
        {
            AddRecord(record, dynamicHashing.MainBlockFactor);
            return true;
        }

        return Depth < MaxHashSize ? SplitNodeAndInsert(record) : HandleOverflow(record, _fileBlockManager, _blockAddress);
    }

    /// <summary>
    /// Attempts to find a record in the external node.
    /// </summary>
    /// <param name="record">The record to find.</param>
    /// <param name="foundRecord">When this method returns, contains the found record if it exists; otherwise null.</param>
    /// <returns>True if the record is found in the current block; otherwise, false.</returns>
    /// <remarks>
    /// This method first checks if the current node is valid for the operation.
    /// If valid, it proceeds to search for the record in the current block.
    /// </remarks>
    public override bool TryFind(IDHRecord<T> record, out IDHRecord<T>? foundRecord)
    {
        if (!IsValidNode())
        {
            foundRecord = null;
            return false;
        }

        var block = ReadCurrentBlock();
        return block.TryFind(record, out foundRecord);
    }

    /// <summary>
    /// Deletes a specified record from the external node.
    /// </summary>
    /// <param name="record">The record to delete.</param>
    /// <returns>The deleted record if it was successfully deleted; otherwise, null.</returns>
    /// <remarks>
    /// This method checks if the current node is valid and if the main file size is non-zero.
    /// If the conditions are met, it attempts to delete the record from the current block.
    /// After deletion, it performs necessary post-deletion actions like updating the block in the file.
    /// </remarks>
    public override IDHRecord<T>? Delete(IDHRecord<T> record)
    {
        if (!IsValidNode())
        {
            return null;
        }

        var block = ReadCurrentBlock();
        var deletedRecord = block.Delete((T)record);
        if(deletedRecord != null)
        {
            HandlePostDeletionActions(block);

            ////Maybe use for striasanie, but check parents reffereces
            //if (_recordsCount == 0 && Parent is DHInternalNode<T> parentInternal && parentInternal.Depth != 0)
            //{
            //    ShortenChain(parentInternal);
            //}
        }
        return deletedRecord;
    }

    private void ShortenChain(DHInternalNode<T> parentInternal)
    {
        DHExternalNode<T> nonEmptyChild = parentInternal.LeftChild == this ?
                                  (DHExternalNode<T>)parentInternal.RightChild :
                                  (DHExternalNode<T>)parentInternal.LeftChild;


        var isLeftChild = ((DHInternalNode<T>)parentInternal.Parent).LeftChild == parentInternal;

        if(isLeftChild)
        {
            ((DHInternalNode<T>)parentInternal.Parent).LeftChild = nonEmptyChild;
        }
        else
        {
            ((DHInternalNode<T>)parentInternal.Parent).RightChild = nonEmptyChild;
        }

        //nonEmptyChild.Depth--; just for now
    }


    /// <summary>
    /// Handles actions required after a record is deleted from the node.
    /// </summary>
    /// <param name="block">The block from which the record was deleted.</param>
    private void HandlePostDeletionActions(DHBlock<T> block)
    {
        var oldAddress = _blockAddress;
        // If there are no valid records left in the block after deletion
        if (block.ValidRecordsCount == 0)
        {
            // Release the block if it's no longer needed
            dynamicHashing.FileBlockManager.ReleaseBlock(block, false);

            _blockAddress = -1;
        }
        else
        {
            // If there are still valid records, update the block in the file
            block.WriteToBinaryFile(dynamicHashing.FileBlockManager.MainFilePath, _blockAddress);
        }

        //block.WriteToBinaryFile(dynamicHashing.FileBlockManager.MainFilePath, oldAddress);

        // Decrement the count of records in the node
        _recordsCount--;
    }


    /// <summary>
    /// Checks if the current node is valid for operations like finding or deleting records.
    /// </summary>
    /// <returns>True if the node is valid; otherwise, false.</returns>
    private bool IsValidNode()
    {
        return _recordsCount >= 0 && _blockAddress != GlobalConstants.InvalidAddress && _fileBlockManager.CurrentMainFileSize != 0;
    }


    /// <summary>
    /// Splits the current node and redistributes records, handling the creation of new child nodes as necessary.
    /// </summary>
    /// <param name="record">The record to insert after splitting.</param>
    private bool SplitNodeAndInsert(IDHRecord<T> record)
    {
        var currentBlock = ReadCurrentBlock();
        var allRecords = new List<IDHRecord<T>>(currentBlock.RecordsList.Count + 1);
        bool recordExists = false;

        foreach (var existingRecord in currentBlock.RecordsList)
        {
            if (existingRecord.MyEquals((T)record))
            {
                recordExists = true;
                return false;
            }
            allRecords.Add(existingRecord);
        }

        if (!recordExists)
        {
            allRecords.Add(record);
        }

        var currentNode = this as DHExternalNode<T>;

        while (ShouldSplit(allRecords,currentNode))
        {
            // Redistribute records between left and right child nodes
            var (leftRecords, rightRecords) = RedistributeRecords(allRecords, currentNode);

            // Split the node and get the child nodes for redistribution
            var (leftChild, rightChild) = SplitNode(leftRecords.Any(),rightRecords.Any(),currentNode, currentBlock);

            // Handle next iteration or final insertion
            (currentNode, allRecords) = PrepareNextIterationOrFinalInsertion(leftChild, rightChild, leftRecords, rightRecords);
        }

        if(allRecords.Any() && currentNode != null)
        {
            HandleFinalInsertionOrOverflow(allRecords, currentNode);
        }
        

        return true;
    }

    /// <summary>
    /// Reads the current block associated with this external node from the file.
    /// </summary>
    /// <returns>The block read from the file.</returns>
    private DHBlock<T> ReadCurrentBlock()
    {
        var currentBlock = new DHBlock<T>(dynamicHashing.MainBlockFactor, _blockAddress);
        currentBlock.ReadFromBinaryFile(dynamicHashing.FileBlockManager.MainFilePath, _blockAddress);
        return currentBlock;
    }

    /// <summary>
    /// Determines if the current node should be split based on the number of records and the depth.
    /// </summary>
    /// <param name="records">The list of records.</param>
    /// <param name="node">The current node being evaluated.</param>
    /// <returns>True if the node should be split; otherwise, false.</returns>
    private bool ShouldSplit(List<IDHRecord<T>> records, DHNode<T> node)
    {
        return records.Count > dynamicHashing.MainBlockFactor && node.Depth < MaxHashSize;
    }

    /// <summary>
    /// Redistributes records into left and right child nodes based on their hash values at the current depth.
    /// </summary>
    /// <param name="records">The list of records to redistribute.</param>
    /// <param name="node">The current node being evaluated.</param>
    /// <returns>A tuple containing lists of left and right records after redistribution.</returns>
    private (List<IDHRecord<T>> leftRecords, List<IDHRecord<T>> rightRecords) RedistributeRecords(
        List<IDHRecord<T>> records,
        DHNode<T> node)
    {
        var leftRecords = new List<IDHRecord<T>>();
        var rightRecords = new List<IDHRecord<T>>();

        foreach (var rec in records)
        {
            if (rec.GetHash()[node.Depth])
            {
                rightRecords.Add(rec);
            }
            else
            {
                leftRecords.Add(rec);
            }
        }

        records.Clear();

        return (leftRecords, rightRecords);
    }

    /// <summary>
    /// Splits the current node into left and right child nodes.
    /// </summary>
    /// <param name="containsLeft">Indicates if there are records for the left child node.</param>
    /// <param name="containsRight">Indicates if there are records for the right child node.</param>
    /// <param name="node">The current node to split.</param>
    /// <returns>A tuple containing the left and right child nodes.</returns>
    private (DHExternalNode<T> leftChild, DHExternalNode<T> rightChild) SplitNode(
        bool containsLeft, 
        bool containsRight,
        DHExternalNode<T> node,
        DHBlock<T> currentBlock)
    {
        var newInternalNode = new DHInternalNode<T>(dynamicHashing, node.Parent);

        // Assign block addresses based on whether each child contains records
        int leftBlockAddress, rightBlockAddress;

        if (containsLeft && !containsRight)
        {
            leftBlockAddress = currentBlock.BlockAddress;
            rightBlockAddress = -1;
        }
        else if (!containsLeft && containsRight)
        {
            leftBlockAddress = -1;
            rightBlockAddress = currentBlock.BlockAddress;
        }
        else // Both left and right contain records
        {
            leftBlockAddress = currentBlock.BlockAddress;
            rightBlockAddress = dynamicHashing.FileBlockManager.GetFreeBlock(false);
        }

        //dynamicHashing.FileBlockManager.ReleaseBlock(currentBlock, false);

        // Assign block addresses to left and right children
        //int leftBlockAddress = containsLeft ? dynamicHashing.FileBlockManager.GetFreeBlock(false) : -1;
        //int rightBlockAddress = containsRight ? dynamicHashing.FileBlockManager.GetFreeBlock(false) : -1;

        newInternalNode.ChangeLeftExternalNodeAddress(leftBlockAddress);
        newInternalNode.ChangeRightExternalNodeAddress(rightBlockAddress);

        UpdateParentChildReference(newInternalNode, node);

        return (newInternalNode.LeftChild as DHExternalNode<T>, newInternalNode.RightChild as DHExternalNode<T>);
    }

    /// <summary>
    /// Updates the parent's child reference to point to a new internal node after splitting.
    /// </summary>
    /// <param name="newInternalNode">The new internal node replacing the current node.</param>
    /// <param name="node">The current node being replaced.</param>
    private void UpdateParentChildReference(DHInternalNode<T> newInternalNode, DHNode<T> node)
    {
        if (node.Parent != null)
        {
            var parentInternal = node.Parent as DHInternalNode<T>;
            if (parentInternal.LeftChild == node)
            {
                parentInternal.LeftChild = newInternalNode;
            }
            else
            {
                parentInternal.RightChild = newInternalNode;
            }
        }
        else
        {
            throw new NotImplementedException();
            // Update the root of the trie if this was the root node
        }
    }

    /// <summary>
    /// Prepares for the next iteration of splitting or handles the final insertion of records.
    /// </summary>
    /// <param name="leftChild">The left child node after splitting.</param>
    /// <param name="rightChild">The right child node after splitting.</param>
    /// <param name="leftRecords">Records associated with the left child node.</param>
    /// <param name="rightRecords">Records associated with the right child node.</param>
    /// <returns>A tuple containing the next node to split and the remaining records.</returns>
    private (DHExternalNode<T>,List<IDHRecord<T>>) PrepareNextIterationOrFinalInsertion(
        DHExternalNode<T> leftChild,
        DHExternalNode<T> rightChild,
        List<IDHRecord<T>> leftRecords,
        List<IDHRecord<T>> rightRecords)
    {
        var allRecords = new List<IDHRecord<T>>();
        DHExternalNode<T> nextSplittingNode = null;

        if (leftRecords.Count > dynamicHashing.MainBlockFactor)
        {
            allRecords = leftRecords;
            nextSplittingNode = leftChild;
        }
        else if (leftRecords.Count > 0)
        {
            leftChild.AddRecords(leftRecords, dynamicHashing.MainBlockFactor);
        }
        
        if (rightRecords.Count > dynamicHashing.MainBlockFactor)
        {
            allRecords = rightRecords;
            nextSplittingNode = rightChild;
        }
        else if(rightRecords.Count > 0)
        {
            rightChild.AddRecords(rightRecords, dynamicHashing.MainBlockFactor);
        }

        return (nextSplittingNode, allRecords);
    }

    /// <summary>
    /// Handles the final insertion of records into a node or manages overflow.
    /// </summary>
    /// <param name="remainingRecords">The remaining records to insert.</param>
    /// <param name="node">The current node to insert the records into.</param>
    private void HandleFinalInsertionOrOverflow(List<IDHRecord<T>> remainingRecords, DHNode<T> node)
    {
        if (node is DHExternalNode<T> finalExternalNode)
        {
            // Determine the number of records to insert without causing overflow
            int recordsToInsert = Math.Min(remainingRecords.Count, dynamicHashing.MainBlockFactor);
            var recordsForInsertion = remainingRecords.Take(recordsToInsert).ToList();

            // Add records in batch to the external node
            finalExternalNode.AddRecords(recordsForInsertion, dynamicHashing.MainBlockFactor);

            // Handle any remaining records that could not be inserted due to block factor limitations
            var overflowRecords = remainingRecords.Skip(recordsToInsert).ToList();
            if (overflowRecords.Any())
            {
                throw new NotImplementedException("Overflow not implemented.");
            }
        } 
        else
        {
            throw new NotImplementedException("Node is not an external node.");
        }
    }

    //private bool HandleOverflow(IDHRecord<T> record, FileBlockManager<T> fileBlockManager, DHBlock<T> lastMainBlock)
    private bool HandleOverflow(IDHRecord<T> record, FileBlockManager<T> fileBlockManager, int lastMainBlockAddress)
    {

        var currentBlock = new DHBlock<T>(fileBlockManager.MainFileBlockFactor, lastMainBlockAddress);
        currentBlock.ReadFromBinaryFile(fileBlockManager.MainFilePath, lastMainBlockAddress);


        if (currentBlock.NextBlockAddress == GlobalConstants.InvalidAddress)
        {
            DHBlock<T> firstOverflowBlock;
            firstOverflowBlock = new DHBlock<T>(fileBlockManager.OverflowFileBlockFactor, fileBlockManager.GetFreeBlock(true));

            firstOverflowBlock.PreviousBlockAddress = currentBlock.BlockAddress;
            currentBlock.NextBlockAddress = firstOverflowBlock.BlockAddress;
            currentBlock.WriteToBinaryFile(fileBlockManager.MainFilePath, currentBlock.BlockAddress);

            firstOverflowBlock.AddRecord(record);
            firstOverflowBlock.WriteToBinaryFile(fileBlockManager.OverflowFilePath, firstOverflowBlock.BlockAddress);

            return true;
        }

        while (currentBlock.NextBlockAddress != GlobalConstants.InvalidAddress)
        {
            //Read next block
            var nextBlock = new DHBlock<T>(fileBlockManager.OverflowFileBlockFactor, currentBlock.NextBlockAddress);        
            nextBlock.ReadFromBinaryFile(fileBlockManager.OverflowFilePath, currentBlock.NextBlockAddress);

            //Check if record already exists
            foreach (var rec in nextBlock.RecordsList)
            {
                if (rec.MyEquals((T)record))
                {
                    return false;
                }
            }

            //Check if it fits
            if (nextBlock.ValidRecordsCount < fileBlockManager.OverflowFileBlockFactor)
            {
                nextBlock.AddRecord(record);
                nextBlock.WriteToBinaryFile(fileBlockManager.OverflowFilePath, nextBlock.BlockAddress);
                return true;
            } 
            else
            {
                //It didn't fit, now we can create new one or go to the another one. 
                if(nextBlock.NextBlockAddress == GlobalConstants.InvalidAddress)
                {
                    var nextNextBlock = new DHBlock<T>(fileBlockManager.OverflowFileBlockFactor, fileBlockManager.GetFreeBlock(true));
                    nextNextBlock.PreviousBlockAddress = nextBlock.BlockAddress;
                    nextNextBlock.AddRecord(record);
                    nextNextBlock.WriteToBinaryFile(fileBlockManager.OverflowFilePath, nextNextBlock.BlockAddress);

                    nextBlock.NextBlockAddress = nextNextBlock.BlockAddress;
                    nextBlock.WriteToBinaryFile(fileBlockManager.OverflowFilePath, nextBlock.BlockAddress);
                    return true;
                } 
                else
                {
                    currentBlock = nextBlock;
                }
            }
        }


        return true;
    }

    /// <summary>
    /// Adds a record to the block associated with this external node.
    /// </summary>
    /// <param name="record">The record to add.</param>
    /// <param name="blockFactor">The block factor to be used.</param>
    /// <exception cref="Exception">Thrown when the record already exists.</exception>
    private void AddRecord(IDHRecord<T> record, int blockFactor)
    {
        EnsureBlockAddress();
        var block = ReadOrCreateBlock(blockFactor);
        foreach (var rec in block.RecordsList)
        {
            if (rec.Equals(record))
            {
                throw new Exception("Record already exists.");
            }
        }
        block.AddRecord(record);
        block.WriteToBinaryFile(dynamicHashing.FileBlockManager.MainFilePath, _blockAddress);
        _recordsCount++;
    }

    private void AddRecords(List<IDHRecord<T>> records, int blockFactor)
    {
        //It's ok that we don't check for duplicates here, because we are adding records from a split node.
        EnsureBlockAddress();
        var block = ReadOrCreateBlock(blockFactor);
        block.AddRecords(records);
        block.WriteToBinaryFile(dynamicHashing.FileBlockManager.MainFilePath, _blockAddress);
        _recordsCount += records.Count;
    }

    /// <summary>
    /// Ensures that a valid block address is assigned to this external node.
    /// </summary>
    private void EnsureBlockAddress()
    {
        if (_blockAddress == -1)
        {
            _blockAddress = dynamicHashing.FileBlockManager.GetFreeBlock(false);
        }
    }


    /// <summary>
    /// Reads or creates a new block associated with this external node.
    /// </summary>
    /// <param name="blockFactor">The block factor to be used.</param>
    /// <returns>The block associated with this node.</returns>
    private DHBlock<T> ReadOrCreateBlock(int blockFactor)
    {
        var block = new DHBlock<T>(blockFactor, _blockAddress);
        if (_recordsCount > 0)
        {
            block.ReadFromBinaryFile(dynamicHashing.FileBlockManager.MainFilePath, _blockAddress);
        }
        return block;
    }

    public override string ToString()
    {
        string childType = "Root"; // Default to "Root" if there is no parent
        if (Parent != null)
        {
            var internalParent = Parent as DHInternalNode<T>;
            childType = internalParent != null && internalParent.LeftChild == this ? "LeftChild" : "RightChild";
        }

        return $"ExternalNode({childType}, Depth: {Depth}, Records: {_recordsCount}, BlockAddress: {_blockAddress})";
    }
}
