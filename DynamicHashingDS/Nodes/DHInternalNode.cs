namespace DynamicHashingDS.Nodes;
public class DHInternalNode : DHNode
{
    public DHNode LeftChild { get; set; }
    public DHNode RightChild { get; set; }

    public DHInternalNode()
    {
        
    }
}
