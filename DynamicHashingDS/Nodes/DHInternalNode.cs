using DynamicHashingDS.Data;
using System.Collections;
using System.Xml.Linq;

namespace DynamicHashingDS.Nodes;

public class DHInternalNode<T> : DHNode<T> where T : DHRecord, new()
{
    public DHNode<T> LeftChild { get; set; }
    public DHNode<T> RightChild { get; set; }

    public override bool Insert(DHRecord record, List<DHBlock<T>> blocks, int blockFactor)
    {
        BitArray hash = record.GetHash();
        DHNode<T> nextNode = Navigate(hash);
        return nextNode.Insert(record, blocks, blockFactor);
    }

    private DHNode<T> Navigate(BitArray hash)
    {
        var reversePosition = hash.Length - Depth - 1;
        return hash[reversePosition] ? RightChild : LeftChild;
    }
}
