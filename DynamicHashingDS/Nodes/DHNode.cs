using DynamicHashingDS.Data;

namespace DynamicHashingDS.Nodes;

public abstract class DHNode<T> where T : DHRecord, new()
{
    public DHNode<T>? Parent { get; set; }
    public int Depth { get; set; }

    public abstract bool Insert(DHRecord record, List<DHBlock<T>> blocks, int blockFactor);
}
