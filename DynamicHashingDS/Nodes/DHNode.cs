using DynamicHashingDS.Data;

namespace DynamicHashingDS.Nodes;

public abstract class DHNode<T> where T : IDHRecord, new()
{
    public DHNode<T>? Parent { get; set; }
    public int Depth { get; set; }

    public abstract bool Insert(IDHRecord record, List<DHBlock<T>> blocks, int blockFactor);
}
