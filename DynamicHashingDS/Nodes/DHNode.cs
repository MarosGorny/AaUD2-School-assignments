using DynamicHashingDS.Data;
using DynamicHashingDS.DH;

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
    public int Depth { get; private set; }

    /// <summary>
    /// Gets the maximum size of the hash used for indexing.
    /// </summary>
    protected int MaxHashSize { get; private set; }

    /// <summary>
    /// The DynamicHashing instance to which this node belongs.
    /// </summary>
    protected DynamicHashing<T> dynamicHashing { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DHNode{T}"/> class.
    /// </summary>
    /// <param name="dynamicHashing">The DynamicHashing instance to which this node belongs.</param>
    /// <param name="parent">The parent node of this node.</param>
    public DHNode(DynamicHashing<T> dynamicHashing, DHNode<T> parent)
    {
        this.dynamicHashing = dynamicHashing;
        MaxHashSize = dynamicHashing.MaxHashSize;

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
}
