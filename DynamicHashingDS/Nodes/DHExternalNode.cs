using DynamicHashingDS.Data;

namespace DynamicHashingDS.Nodes;

public class DHExternalNode<T> : DHNode where T : DHRecord, new()
{
    public int RecordsCount { get; set; }
    public int BlockAddress { get; set; }

    public DHExternalNode()
    {
        
    }

}
