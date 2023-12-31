﻿using DynamicHashingDS.Data;
using DynamicHashingDS.DH;
using Newtonsoft.Json;

namespace DynamicHashingDS.Nodes;

/// <summary>
/// Represents an abstract node in a dynamic hashing trie.
/// </summary>
/// <typeparam name="T">The type of record stored in the dynamic hashing trie.</typeparam>
public abstract class DHNode<T> where T : IDHRecord<T>, new()
{
    /// <summary>
    /// Gets or sets the parent of this node.
    /// </summary>
    public DHNode<T>? Parent { get; set; }

    /// <summary>
    /// Gets the depth of this node in the trie.
    /// </summary>
    public int Depth { get; set; }


    /// <summary>
    /// The DynamicHashing instance to which this node belongs.
    /// </summary>
    [JsonIgnore]
    public DynamicHashing<T> dynamicHashing { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DHNode{T}"/> class.
    /// </summary>
    /// <param name="dynamicHashing">The DynamicHashing instance to which this node belongs.</param>
    /// <param name="parent">The parent node of this node.</param>
    public DHNode(DynamicHashing<T> dynamicHashing, DHNode<T> parent)
    {
        this.dynamicHashing = dynamicHashing;

        Parent = parent;
        Depth = Parent == null ? 0 : Parent.Depth + 1;
    }

    /// <summary>
    /// Inserts a record into the node.
    /// This is an abstract method that must be implemented in derived classes.
    /// </summary>
    /// <param name="record">The record to insert.</param>
    /// <returns>True if the insertion is successful, otherwise false.</returns>
    public abstract bool Insert(IDHRecord<T> record);

    /// <summary>
    /// Deletes a record from the node.
    /// This is an abstract method that must be implemented in derived classes.
    /// </summary>
    /// <param name="record">The record to delete.</param>
    /// <returns>The deleted record if successful; otherwise, null.</returns>
    public abstract IDHRecord<T>? Delete(IDHRecord<T> record);

    /// <summary>
    /// Attempts to find a record in the node.
    /// This is an abstract method that must be implemented in derived classes.
    /// </summary>
    /// <param name="record">The record to find.</param>
    /// <param name="foundRecord">When this method returns, contains the found record if it exists; otherwise null.</param>
    /// <returns>True if a record was found; otherwise, false.</returns>
    public abstract bool TryFind(IDHRecord<T> record, out IDHRecord<T>? foundRecord, out DHBlock<T> foundBlock, out bool isOverflowBlock);

}
