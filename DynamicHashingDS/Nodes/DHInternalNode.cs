using DynamicHashingDS.Data;
using DynamicHashingDS.DH;
using System.Collections;

namespace DynamicHashingDS.Nodes;

/// <summary>
/// Represents an internal node in the dynamic hashing trie structure.
/// </summary>
/// <typeparam name="T">The type of records stored in the dynamic hashing structure.</typeparam>
public class DHInternalNode<T> : DHNode<T> where T : IDHRecord<T>, new()
{
    public DHNode<T> LeftChild { get; set; }

    public DHNode<T> RightChild { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DHInternalNode{T}"/> class with default child nodes.
    /// </summary>
    /// <param name="dynamicHashing">The dynamic hashing instance associated with this node.</param>
    /// <param name="parent">The parent node of this internal node.</param>
    public DHInternalNode(DynamicHashing<T> dynamicHashing, DHNode<T>? parent) : base(dynamicHashing, parent!)
    {
        LeftChild = new DHExternalNode<T>(this.dynamicHashing, this, -1);
        RightChild = new DHExternalNode<T>(this.dynamicHashing, this, -1);
    }

    /// <summary>
    /// Inserts a record into the appropriate child node based on the hash.
    /// </summary>
    /// <param name="record">The record to insert.</param>
    /// <returns>True if the insertion is successful, otherwise false.</returns>
    public override bool Insert(IDHRecord<T> record)
    {
        BitArray hash = record.GetHash();
        DHNode<T> nextNode = Navigate(hash);
        return nextNode.Insert(record);
    }

    /// <summary>
    /// Changes the block address of the left external child node.
    /// Throws an exception if the left child is not an external node.
    /// </summary>
    /// <param name="blockAddress">The new block address for the left external node.</param>
    /// <exception cref="InvalidOperationException">Thrown when the LeftChild is not an external node.</exception>
    public void ChangeLeftExternalNodeAddress(int blockAddress)
    {
        if (LeftChild is DHExternalNode<T> leftChild)
        {
            leftChild.SetBlockAddress(blockAddress);
        }
        else
        {
            throw new InvalidOperationException("LeftChild is not an external node.");
        }
    }

    /// <summary>
    /// Changes the block address of the right external child node.
    /// Throws an exception if the right child is not an external node.
    /// </summary>
    /// <param name="blockAddress">The new block address for the right external node.</param>
    /// <exception cref="InvalidOperationException">Thrown when the RightChild is not an external node.</exception>
    public void ChangeRightExternalNodeAddress(int blockAddress)
    {
        if (RightChild is DHExternalNode<T> rightChild)
        {
            rightChild.SetBlockAddress(blockAddress);
        }
        else
        {
            throw new InvalidOperationException("RightChild is not an external node.");
        }
    }

    /// <summary>
    /// Navigates to the next child node based on the current bit in the hash.
    /// </summary>
    /// <param name="hash">The hash of the record being inserted.</param>
    /// <returns>The next child node to continue the insertion.</returns>
    private DHNode<T> Navigate(BitArray hash)
    {
        var position = Depth < MaxHashSize ? Depth : MaxHashSize - 1;
        return hash[position] ? RightChild : LeftChild;
    }

    /// <summary>
    /// Provides a string representation of the internal node for debugging purposes.
    /// </summary>
    /// <returns>A string that represents the current internal node.</returns>
    public override string ToString()
    {
        var leftChildDesc = LeftChild != null ? $"Left: {LeftChild}" : "Left: null";
        var rightChildDesc = RightChild != null ? $"Right: {RightChild}" : "Right: null";
        return $"InternalNode(Depth: {Depth}, {leftChildDesc}, {rightChildDesc})";
    }
}
