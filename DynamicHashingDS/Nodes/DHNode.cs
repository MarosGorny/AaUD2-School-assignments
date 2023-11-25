using DynamicHashingDS.Data;
using DynamicHashingDS.DH;

namespace DynamicHashingDS.Nodes;

public abstract class DHNode<T> where T : IDHRecord<T>, new()
{
    public DHNode<T>? Parent { get; set; }
    public int Depth { get; set; }
    protected int MaxHashSize { get; private set; }

    protected DynamicHashing<T> dynamicHashing;

    public DHNode(DynamicHashing<T> dynamicHashing, DHNode<T> parent)
    {
        this.dynamicHashing = dynamicHashing;
        MaxHashSize = dynamicHashing.MaxHashSize;

        Parent = parent;

        if(Parent is null)
        {
            Depth = 0;
        }
        else
        {
            Depth = Parent.Depth + 1;
        }
            
    }

    //public abstract bool Insert(IDHRecord<T> record, List<DHBlock<T>> blocks, int blockFactor);
    public abstract bool Insert(IDHRecord<T> record);
}
