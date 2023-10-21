using QuadTree.SpatialItems;

namespace QuadTree;

public class QuadTree<K,V> where K : IComparable<K>
{
    public QuadTreeNode<K,V> Root { get; private set; }
    public int maxDepth { get; set; } = 10; //TODO: Need to be set by user and need to be implemented

    public QuadTree(Rectangle boundary)
    {
        Root = new QuadTreeNode<K, V>(boundary,null);
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