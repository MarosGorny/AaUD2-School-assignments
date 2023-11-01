using QuadTreeDS.SpatialItems;

namespace QuadTreeDS.QuadTree;

public class QuadTree<K, V> where K : IComparable<K>
{
    public QuadTreeNode<K, V> Root { get; private set; }
    public int MaxAllowedDepth { get; set; }
    public readonly int QUADRANT_COUNT = 4;

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

    public bool Delete(QuadTreeObject<K, V> quadTreeObject)
    {
        return Root.Delete(quadTreeObject);
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

    public int CalculateIdealDepth(int totalDataPoints)
    {
        int depth = 0;
        int capacity = 1; // root
        while (capacity < totalDataPoints)
        {
            depth++;
            capacity += (int)Math.Pow(4, depth);
        }
        return depth;
    }

    public double CalculateTreeHealth()
    {
        double totalDataScore = 0;
        int nodeCount = 0;
        int maxDepth = 0;
        int totalDataPoints = 0;

        Root.InOrderTraversal(node => {
            totalDataScore += node.DataScore();
            nodeCount++;
            totalDataPoints += node.Data.Count;
            if (node.Depth > maxDepth)
                maxDepth = node.Depth;
        });

        double avgDataScore = totalDataScore / nodeCount;
        int idealDepth = CalculateIdealDepth(totalDataPoints);
        double depthScore = (maxDepth <= idealDepth) ? 1 : 1.0 / (1 + maxDepth - idealDepth);

        double combinedScore = (avgDataScore + depthScore) / 2;

        return combinedScore;
    }
}