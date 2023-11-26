﻿using DynamicHashingDS.Data;
using DynamicHashingDS.DH;
using System.Collections;

namespace DynamicHashingDS.Nodes;

/// <summary>
/// Represents an external node in a dynamic hashing trie structure.
/// </summary>
/// <typeparam name="T">The type of the record.</typeparam>
public class DHExternalNode<T> : DHNode<T> where T : IDHRecord<T>, new()
{
    private int _recordsCount;
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
        int blockFactor = dynamicHashing.MainBlockFactor;
        // Existing logic to add record if there is space
        if (_recordsCount < blockFactor)
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
                HandleOverflow(record, _fileBlockManager);
                return true;
            }
        }
    }

    /// <summary>
    /// Splits the current node and redistributes records, handling the creation of new child nodes as necessary.
    /// </summary>
    /// <param name="record">The record to insert after splitting.</param>
    private void SplitNodeAndInsert(IDHRecord<T> record)
    {
        // Read the current block to redistribute records
        var currentBlock = ReadCurrentBlock();

        // Add the new record for redistribution
        var allRecords = new List<IDHRecord<T>>(currentBlock.RecordsList) { record };

        var currentNode = this as DHExternalNode<T>;

        while (ShouldSplit(allRecords,currentNode))
        {
            // Redistribute records between left and right child nodes
            var (leftRecords, rightRecords) = RedistributeRecords(allRecords, currentNode);

            // Split the node and get the child nodes for redistribution
            var (leftChild, rightChild) = SplitNode(leftRecords.Any(),rightRecords.Any(),currentNode);



            // Handle next iteration or final insertion
            (currentNode, allRecords) = PrepareNextIterationOrFinalInsertion(leftChild, rightChild, leftRecords, rightRecords);
        }

        HandleFinalInsertionOrOverflow(allRecords, currentNode);
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
        DHExternalNode<T> node)
    {
        var newInternalNode = new DHInternalNode<T>(dynamicHashing, node.Parent);
        dynamicHashing.FileBlockManager.ReleaseBlock(node._blockAddress, false);

        // Assign block addresses to left and right children
        int leftBlockAddress = containsLeft ? dynamicHashing.FileBlockManager.GetFreeBlock(false) : -1;
        int rightBlockAddress = containsRight ? dynamicHashing.FileBlockManager.GetFreeBlock(false) : -1;

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
        else
        {
            foreach (var rec in leftRecords) leftChild.AddRecord(rec, dynamicHashing.MainBlockFactor);
        }
        
        if (rightRecords.Count > dynamicHashing.MainBlockFactor)
        {
            allRecords = rightRecords;
            nextSplittingNode = rightChild;
        }
        else
        {
            foreach (var rec in rightRecords) rightChild.AddRecord(rec, dynamicHashing.MainBlockFactor);
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
            for (int i = 0; i < remainingRecords.Count; i++)
            {
                var rec = remainingRecords[i];
                if (i < dynamicHashing.MainBlockFactor)
                {
                    finalExternalNode.AddRecord(rec, dynamicHashing.MainBlockFactor);
                }
                else
                {
                    throw new NotImplementedException();
                    //Add to overflow file
                }
            }
        }
    }

    /// <summary>
    /// Handles overflow situations by directing the record to an overflow mechanism.
    /// </summary>
    /// <param name="record">The record causing overflow.</param>
    /// <param name="fileBlockManager">The file block manager handling overflow.</param>
    private void HandleOverflow(IDHRecord<T> record, FileBlockManager<T> fileBlockManager)
    {
        // Implement logic to handle overflow
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds a record to the block associated with this external node.
    /// </summary>
    /// <param name="record">The record to add.</param>
    /// <param name="blockFactor">The block factor to be used.</param>
    private void AddRecord(IDHRecord<T> record, int blockFactor)
    {
        EnsureBlockAddress();
        var block = ReadOrCreateBlock(blockFactor);
        block.AddRecord(record);
        block.WriteToBinaryFile(dynamicHashing.FileBlockManager.MainFilePath, _blockAddress);
        _recordsCount++;
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
