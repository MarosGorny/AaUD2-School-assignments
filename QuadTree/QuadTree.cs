using QuadTree.SpatialItems;

namespace QuadTree;

public class QuadTree<K,V> where K : IComparable<K>
{
    public QuadTreeNode<K,V> Root { get; private set; }
    public int MaxAllowedDepth { get; set; } //TODO: Need to be set by user and need to be implemented

    public QuadTree(Rectangle boundary, int maxAllowedDepth = 10)
    {
        Root = new QuadTreeNode<K, V>(boundary,null,this);
        MaxAllowedDepth = maxAllowedDepth;
        //Root.Subdivide();
    }

    public void Insert(QuadTreeObject<K, V> quadTreeObject)
    {
        Root.Insert(quadTreeObject);
    }
    
    public List<QuadTreeObject<K,V>>? Find(QuadTreeObject<K,V> quadTreeObject)
    {
        return Root.Find(quadTreeObject);
    }

    public void Delete(QuadTreeObject<K, V> quadTreeObject)
    {
        Root.Delete(quadTreeObject);
    }
}