using DynamicHashingDS.Data;
using DynamicHashingDS.DH;
using System.Collections;

namespace DynamicHashingDS.Nodes;

public class DHExternalNode<T> : DHNode<T> where T : IDHRecord<T>, new()
{
    public int RecordsCount { get; private set; }
    public int BlockAddress { get; private set; }

    private FileBlockManager<T> fileBlockManager;

    public DHExternalNode(DynamicHashing<T> dynamicHashing, DHNode<T> parent,int blockAdress) : base(dynamicHashing,parent)
    {
        RecordsCount = 0;
        BlockAddress = blockAdress;
        fileBlockManager = dynamicHashing.FileBlockManager;
    }

    public override bool Insert(IDHRecord<T> record)
    {
        int blockFactor = dynamicHashing.MainBlockFactor;
        // Existing logic to add record if there is space
        if (RecordsCount < blockFactor)
        {
            this.AddRecord(record, blockFactor);
            return true;
        }
        else
        {
            // Handle the case when the block is full
            if (Depth < MaxHashSize) // Assuming MaxHashBits is the maximum depth based on the hash size
            {
                // Split the node and redistribute records
                SplitNodeAndInsert(record);
                return true;
            }
            else
            {
                // Handle overflow to overflow file
                HandleOverflow(record, fileBlockManager);
                return true;
            }
        }
    }

    private void SplitNodeAndInsert(IDHRecord<T> record)
    {
        // Read the current block to redistribute records
        DHBlock<T> currentBlock = new DHBlock<T>(dynamicHashing.MainBlockFactor, BlockAddress);
        currentBlock.ReadFromBinaryFile(dynamicHashing.FileBlockManager.MainFilePath, BlockAddress);

        List<IDHRecord<T>> allRecords = new List<IDHRecord<T>>(currentBlock.RecordsList);
        allRecords.Add(record); // Adding new record for redistribution

        List<IDHRecord<T>> leftRecords = new List<IDHRecord<T>>();
        List<IDHRecord<T>> rightRecords = new List<IDHRecord<T>>();

        DHExternalNode<T> currentNode = this as DHExternalNode<T>; ; // Start with the current external node



        while (allRecords.Count > dynamicHashing.MainBlockFactor)
        {
            if (currentNode.Depth >= MaxHashSize) break; // Stop splitting if maximum depth reached

            // Determine whether records belong to the left or right child based on the hash bit
            bool containsLeft = false, containsRight = false;

            foreach (var rec in allRecords)
            {
                if (rec.GetHash()[currentNode.Depth])
                {
                    rightRecords.Add(rec);
                }
                else
                {
                    leftRecords.Add(rec);
                }
            }

            allRecords.Clear();

            if(leftRecords.Any())
            {
                containsLeft = true;
            }

            if(rightRecords.Any())
            {
                containsRight = true;
            }

            // Create a new internal node to replace the current external node
            var newInternalNode = new DHInternalNode<T>(dynamicHashing, currentNode.Parent);
            dynamicHashing.FileBlockManager.ReleaseBlock(currentNode.BlockAddress, false);

            if(containsLeft)
            {
                var newBlockAddress = dynamicHashing.FileBlockManager.GetFreeBlock(false);
                newInternalNode.AddLeftExternalNode(newBlockAddress);
            } 
            else
            {
                newInternalNode.AddLeftExternalNode(-1);
            }

            if(containsRight)
            {
                var newBlockAddress = dynamicHashing.FileBlockManager.GetFreeBlock(false);
                newInternalNode.AddRightExternalNode(newBlockAddress);
            }
            else
            {
                newInternalNode.AddRightExternalNode(-1);
            }
            

            // Update parent's child reference
            if (currentNode.Parent != null)
            {
                var parentInternal = currentNode.Parent as DHInternalNode<T>;
                if (parentInternal != null)
                {
                    if (parentInternal.LeftChild == currentNode) parentInternal.LeftChild = newInternalNode;
                    else parentInternal.RightChild = newInternalNode;
                }
            }
            else
            {
                throw new NotImplementedException();
                //dynamicHashing.Root = newInternalNode; // Update the root of the trie if this was the root node
            }

            DHExternalNode<T> nextSplittingNode = null;
            var newLeftNode = newInternalNode.LeftChild as DHExternalNode<T>;
            var newRightNode = newInternalNode.RightChild as DHExternalNode<T>;
            if (leftRecords.Count > dynamicHashing.MainBlockFactor)
            {
                nextSplittingNode = newInternalNode.LeftChild as DHExternalNode<T>;
                allRecords = leftRecords;
            } else
            {
                foreach (var rec in leftRecords) newLeftNode.AddRecord(rec, dynamicHashing.MainBlockFactor);
            }

            if (rightRecords.Count > dynamicHashing.MainBlockFactor)
            {
                nextSplittingNode = newInternalNode.RightChild as DHExternalNode<T>;
                allRecords = rightRecords;
            } else
            {
                foreach (var rec in rightRecords) newRightNode.AddRecord(rec, dynamicHashing.MainBlockFactor);
            }
           

            // Prepare for the next iteration
            currentNode = nextSplittingNode;
        }

        //Handle final insertion or overflow
        if (currentNode is DHExternalNode<T> finalExternalNode)
        {
            for(int i = 0; i < allRecords.Count; i++)
            {
                var rec = allRecords[i];
                if (i < dynamicHashing.MainBlockFactor)
                {
                    finalExternalNode.AddRecord(rec, dynamicHashing.MainBlockFactor);
                } else
                {
                    throw new NotImplementedException();
                    //Add to overflow file
                }
            }
        }
    }




    //private void SplitNodeAndInsert(IDHRecord<T> record)
    //{
    //    // Read the current block to redistribute records
    //    DHBlock<T> currentBlock = new DHBlock<T>(dynamicHashing.MainBlockFactor, BlockAddress);
    //    currentBlock.ReadFromBinaryFile(dynamicHashing.FileBlockManager.MainFilePath, BlockAddress);

    //    List<IDHRecord<T>> allRecords = currentBlock.RecordsList;
    //    allRecords.Add(record);
    //    var currentNode = this;
    //    DHExternalNode<T> leftNode = null;
    //    DHExternalNode<T> rightNode = null;

    //    while (allRecords.Count > dynamicHashing.MainBlockFactor)
    //    {
    //        bool containsLeft = false;
    //        bool containsRight = false;

    //        // Create a new internal node to replace this external node
    //        var newInternalNode = new DHInternalNode<T>(dynamicHashing, this.Parent);

    //        foreach (var rec in allRecords)
    //        {

    //            if (rec.GetHash()[Depth])
    //            {
    //                containsRight = true;
    //            }
    //            else
    //            {
    //                containsLeft = true;
    //            }

    //            if (containsRight && containsLeft)
    //            {
    //                break;
    //            }
    //        }



    //        if(containsLeft && containsRight)
    //        {
    //            newInternalNode.AddLeftExternalNode(BlockAddress);
    //            int rightBlockAddress = dynamicHashing.FileBlockManager.GetFreeBlock(false);
    //            newInternalNode.AddRightExternalNode(rightBlockAddress);
    //        } else if (containsLeft)
    //        {
    //            newInternalNode.AddLeftExternalNode(BlockAddress);
    //        } else if (containsRight)
    //        {
    //            newInternalNode.AddRightExternalNode(BlockAddress);
    //        }

    //        // Update parent's child reference
    //        if (this.Parent != null)
    //        {
    //            var parentInternal = this.Parent as DHInternalNode<T>;
    //            if (parentInternal != null)
    //            {
    //                if (parentInternal.LeftChild == this)
    //                    parentInternal.LeftChild = newInternalNode;
    //                else
    //                    parentInternal.RightChild = newInternalNode;
    //            }


    //        }
    //        else
    //        {
    //            // Update the root of the trie if this was the root node
    //            throw new NotImplementedException();
    //            //dynamicHashing.Root = newInternalNode;
    //        }

    //        foreach (var rec in allRecords)
    //        {
    //            newInternalNode.Insert(rec);
    //        }

    //    }

    //}


    private void HandleOverflow(IDHRecord<T> record, FileBlockManager<T> fileBlockManager)
    {
        // Implement logic to handle overflow
        throw new NotImplementedException();
    }


    public void AddRecord(IDHRecord<T> record, int blockFactor)
    {
        if(RecordsCount == 0)
        {
            if(BlockAddress == -1)
            {
                BlockAddress = dynamicHashing.FileBlockManager.GetFreeBlock(false);
            }
            DHBlock<T> newBlock = new DHBlock<T>(blockFactor, BlockAddress);
            //newBlock.ReadFromBinaryFile(dynamicHashing.FileBlockManager.MainFilePath, BlockAddress);
            newBlock.AddRecord(record);
            newBlock.WriteToBinaryFile(dynamicHashing.FileBlockManager.MainFilePath,BlockAddress);

            this.RecordsCount++;
        } 
        else
        {
            DHBlock<T> newBlock = new DHBlock<T>(blockFactor);
            newBlock.ReadFromBinaryFile(dynamicHashing.FileBlockManager.MainFilePath, BlockAddress);
            newBlock.AddRecord(record);
            newBlock.WriteToBinaryFile(dynamicHashing.FileBlockManager.MainFilePath, BlockAddress);

            this.RecordsCount++;
        }
    }

    public override string ToString()
    {
        string childType = "Root"; // Default to "Root" if there is no parent
        if (Parent != null)
        {
            var internalParent = Parent as DHInternalNode<T>;
            childType = internalParent != null && internalParent.LeftChild == this ? "LeftChild" : "RightChild";
        }

        return $"ExternalNode({childType}, Depth: {Depth}, Records: {RecordsCount}, BlockAddress: {BlockAddress})";
    }


}
