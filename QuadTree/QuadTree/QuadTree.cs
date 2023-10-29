using QuadTreeDS.SpatialItems;

namespace QuadTreeDS.QuadTree;

public class QuadTree<K, V> where K : IComparable<K>
{
    public QuadTreeNode<K, V> Root { get; private set; }
    public int MaxAllowedDepth { get; set; }

    public QuadTree(Rectangle boundary, int maxAllowedDepth = 10)
    {
        Root = new QuadTreeNode<K, V>(boundary, null, this);
        MaxAllowedDepth = maxAllowedDepth;
    }

    public void Insert(QuadTreeObject<K, V> quadTreeObject)
    {
        Root.Insert(quadTreeObject);
    }

    public List<QuadTreeObject<K, V>>? Find(SpatialItem rectangle)
    {
        return Root.Find(rectangle);
    }

    public void Delete(QuadTreeObject<K, V> quadTreeObject)
    {
        Root.Delete(quadTreeObject);
    }

    public void ChangeMaxAllowedDepth(int maxAllowedDepth)
    {
        int newMaxAllowDepth = maxAllowedDepth;
        if (newMaxAllowDepth < MaxAllowedDepth)
        {
            MaxAllowedDepth = newMaxAllowDepth;
            Root.ReduceDepth();
        }
        else if (newMaxAllowDepth > MaxAllowedDepth)
        {
            MaxAllowedDepth = newMaxAllowDepth;
            Root.IncreaseDepth();
        } 
    }
}