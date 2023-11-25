using DynamicHashingDS.Data;
using DynamicHashingDS.DH;
using System.Collections;

namespace DynamicHashingDS.Nodes;

public class DHInternalNode<T> : DHNode<T> where T : IDHRecord<T>, new()
{


    public DHNode<T> LeftChild { get; set; }
    public DHNode<T> RightChild { get; set; }


    public DHInternalNode(DynamicHashing<T> dynamicHashing, DHNode<T>? parent) : base(dynamicHashing, parent)
    {
    }

    public override bool Insert(IDHRecord<T> record)
    {
        BitArray hash = record.GetHash();
        DHNode<T> nextNode = Navigate(hash);
        return nextNode.Insert(record);
    }

    public void AddLeftExternalNode(int blockAddress)
    {
        this.LeftChild = new DHExternalNode<T>(this.dynamicHashing,this,blockAddress);
    }

    public void AddRightExternalNode(int blockAddress)
    {
        this.RightChild = new DHExternalNode<T>(this.dynamicHashing, this, blockAddress);
    }

    private DHNode<T> Navigate(BitArray hash)
    {
        var position = Depth < MaxHashSize ? Depth : MaxHashSize - 1;
        return hash[position] ? RightChild : LeftChild;
    }

    public override string ToString()
    {
        var leftChildDesc = LeftChild != null ? $"Left: {LeftChild.ToString()}" : "Left: null";
        var rightChildDesc = RightChild != null ? $"Right: {RightChild.ToString()}" : "Right: null";

        return $"InternalNode(Depth: {Depth}, {leftChildDesc}, {rightChildDesc})";
    }

}
