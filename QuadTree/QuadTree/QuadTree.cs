﻿using QuadTreeDS.BinarySearchTree;
using QuadTreeDS.SpatialItems;


namespace QuadTreeDS.QuadTree;

public class QuadTree<K, V> where K : IComparable<K>
{
    public QuadTreeNode<K, V> Root { get; private set; }
    public int MaxAllowedDepth { get; set; }
    public Rectangle Boundary => Root.Boundary;
    public QuadTreeOptimalization<K, V>.SubdivisionResult? OptimalizationData { get; }

    public int portions { get; set;}

    public readonly int QUADRANT_COUNT = 4;

    public bool CheckKeysDuplicate { get; set; } = true;

    private BinarySearchTree<K> _bst = new BinarySearchTree<K>();

    public QuadTree(Rectangle boundary, int maxAllowedDepth = 10)
    {
        Root = new QuadTreeNode<K, V>(boundary, null, this);
        MaxAllowedDepth = maxAllowedDepth;
    }

    public QuadTree(Rectangle boundary, List<QuadTreeObject<K, V>> knownItems, int maxAllowedDepth = 1000, int portions = 2)
    {
        MaxAllowedDepth = maxAllowedDepth;

        if (knownItems is not null)
        {
            var subdivisionResult = QuadTreeOptimalization<K, V>.BestSubdivision(boundary, knownItems, portions);
            Root = new QuadTreeNode<K, V>(boundary, null, this, subdivisionResult.VerticalCut, subdivisionResult.HorizontalCut );
            Root.InsertOptimalizedIterative(subdivisionResult, portions);
        }
    }

    public bool SearchKey(K key)
    {
        return _bst.Search(key);
    }
    public bool InsertKey(K key)
    {
        return _bst.TryInsert(key);
    }

    public void Insert(QuadTreeObject<K, V> quadTreeObject)
    {
        Root.Insert(quadTreeObject);
    }

    public List<QuadTreeObject<K, V>>? Find(ISpatialItem rectangle)
    {
        return Root.Find(rectangle);
    }

    public (QuadTreeNode<K, V>? foundNode, QuadTreeObject<K, V>? foundObject) FindNode(QuadTreeObject<K, V> quadTreeObject)
    {
        return Root.LocateNodeAndObjectForItem(quadTreeObject.Item, quadTreeObject.Key);
    }

    public ISpatialItem? Delete(QuadTreeObject<K, V> quadTreeObject)
    {
        return Root.Delete(quadTreeObject);
    }

    public bool DeleteKey(K key)
    {
        return _bst.Delete(key);
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

    public (double combinedScore, double dataScore,double depthScore) CalculateTreeHealth()
    {
        double totalDataScore = 0;
        int nodeCount = 0;
        int maxDepth = 0;
        int totalDataPoints = 0;

        Root.InOrderTraversal(node => {
            if(node.IsLeaf() && node.Data.Count == 0)
                return;

            totalDataScore += node.DataScore();
            nodeCount++;
            totalDataPoints += node.Data.Count;
            if (node.Depth > maxDepth)
                maxDepth = node.Depth;
        });

        var dataScore = totalDataScore / nodeCount;
        int idealDepth = CalculateIdealDepth(totalDataPoints);

        // Adjust the depth score calculation here
        // Adding a scaling factor to the logarithmic function
        var depthDifference = maxDepth - idealDepth;
        var depthScore = (depthDifference <= 0) ? 1 : 1.0 / (1 + Math.Log(depthDifference) / Math.Log(2) * QuadTreeOptimalization<K,V>.ScalingFactor);


        double combinedScore = (dataScore + depthScore) / 2;

        return (combinedScore, dataScore, depthScore);
    }
}