using DynamicHashingDS.Data;

namespace DynamicHashingDS.Nodes;

public abstract class DHNode
{
    public DHNode? Parent { get; set; }
    public int Depth { get; set; }

    public abstract bool Insert(DHRecord record);
}
