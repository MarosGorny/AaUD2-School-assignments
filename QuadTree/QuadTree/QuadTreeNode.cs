using QuadTreeDS.SpatialItems;
using System;

namespace QuadTreeDS.QuadTree;

public class QuadTreeNode<K, V> where K : IComparable<K>
{
    #region Constructor and properties
    public Rectangle Boundary { get; }
    public double HorizontalCut { get; private set; }
    public double VerticalCut { get; private set; }
    public int Depth { get; }
    public int MaxSubtreeDepth { get; private set; } = 0;
    public List<QuadTreeObject<K, V>> Data { get; private set; } = new List<QuadTreeObject<K, V>>(); //TODO: Rectangle Data and point Data should be seperated
    public List<QuadTreeObject<K, V>> PointData { get; private set; } = new List<QuadTreeObject<K, V>>();
    public List<QuadTreeObject<K, V>> RectangleData { get; private set; } = new List<QuadTreeObject<K, V>>();
    public QuadTreeNode<K, V>[]? Children { get; private set; }
    public QuadTreeNode<K, V>? Parent { get; }
    public QuadTree<K, V> QuadTree { get; private set; }

    public QuadTreeNode(Rectangle boundary, QuadTreeNode<K, V>? parent, QuadTree<K, V> quadTree, double? newVerticalCut = null, double? newHorizontalCut =null)
    {
        Boundary = boundary;
        Parent = parent;
        Depth = parent?.Depth + 1 ?? 0;
        QuadTree = quadTree;

        if(newVerticalCut is not null && newHorizontalCut is not null)
        {
            VerticalCut = newVerticalCut.Value;
            HorizontalCut = newHorizontalCut.Value;
        }
        else
        {
            (VerticalCut, HorizontalCut) = CalculateMidPoints();
        }
    }
    #endregion

    #region Node Type Checks
    public bool IsRoot() => Parent is null;
    public bool IsLeaf() => Children is null;
    #endregion

    #region Node Manipulation
    private void MakeLeaf()
    {
        Children = null;
    }

    public void ChangeCuts(double newVerticalCut, double newHorizontalCut)
    {
        VerticalCut = newVerticalCut;
        HorizontalCut = newHorizontalCut;
    }


    private bool TryMakeLeaf()
    {
        if(IsLeaf())
        {
            return true;
        }

        if(AreChildrenLeavesAndEmpty())
        {
            MakeLeaf();
            return true;
        }
        return false;
    }

    private void Subdivide(Rectangle[] boundaries, double? verticalCut = null, double? horizontalCut = null)
    {
        if (IsLeaf())
        {
            Children = new QuadTreeNode<K, V>[QuadTree.QUADRANT_COUNT];

            for (int i = 0; i < QuadTree.QUADRANT_COUNT; i++)
            {
                if(horizontalCut.HasValue && verticalCut.HasValue)
                {
                    Children[i] = new QuadTreeNode<K, V>(boundaries[i], this, QuadTree, verticalCut.Value,horizontalCut.Value );
                }
                else
                {
                    Children[i] = new QuadTreeNode<K, V>(boundaries[i], this, QuadTree);
                }
            }
            UpdateMaxSubtreeDepthAfterSubdivision();
        }
    }

    private void UpdateMaxSubtreeDepthAfterSubdivision()
    {
        MaxSubtreeDepth = 1;
        UpdateAncestorsMaxSubtreeDepth();
    }

    private void UpdateAncestorsMaxSubtreeDepth()
    {
        QuadTreeNode<K, V>? currentNode = Parent;
        int childDepth = MaxSubtreeDepth;

        while (currentNode != null)
        {
            if (currentNode.MaxSubtreeDepth < childDepth + 1)
            {
                currentNode.MaxSubtreeDepth = childDepth + 1;
                childDepth = currentNode.MaxSubtreeDepth;
                currentNode = currentNode.Parent;
            }
            else
            {
                break;
            }
        }
    }

    private void SimplifyIfEmpty()
    {
        if (Data.Count == 0 && !IsLeaf())
        {
            bool canSimplify = AreChildrenLeavesAndEmpty();

            if (canSimplify)
            {
                MakeLeaf();
            }
            else
            {
                TransferDataFromDeepestLeaf();
            }
        }
    }

    private bool AreChildrenLeavesAndEmpty()
    {
        foreach (var child in Children)
        {
            if (!child.IsLeaf() || child.Data.Count > 0)
            {
                return false;
            }
        }
        return true;
    }

    private void TransferDataFromDeepestLeaf()
    {
        var deepestLeaf = GetDeepestLeafWithMinimalData();
        if (deepestLeaf != null)
        {
            Data = deepestLeaf.Data;
            PointData = deepestLeaf.PointData;
            RectangleData = deepestLeaf.RectangleData;

            deepestLeaf.Data = new List<QuadTreeObject<K, V>>(); 
            deepestLeaf.PointData = new List<QuadTreeObject<K, V>>();
            deepestLeaf.RectangleData = new List<QuadTreeObject<K, V>>();

            if(deepestLeaf.Data.Count == 0 && !deepestLeaf.IsRoot() && deepestLeaf.Parent.AreChildrenLeavesAndEmpty())
            {
                deepestLeaf.Parent.MakeLeaf();
            }
        }
    }

    private QuadTreeNode<K, V>? GetDeepestLeafWithMinimalData()
    {
        QuadTreeNode<K, V> currentNode = this;

        while (!currentNode.IsLeaf())
        {
            currentNode = currentNode.Children.MaxBy(child => child.MaxSubtreeDepth);
            if (currentNode is null) return null;
        }

        // At this point, currentNode's siblings including itself are the leaf nodes at the deepest level.
        QuadTreeNode<K, V>? leafWithMinimalData = null;
        int minimalDataCount = int.MaxValue;

        foreach (var sibling in currentNode.Parent.Children)
        {
            if (sibling.IsLeaf() && sibling.Data.Count > 0 && sibling.Data.Count < minimalDataCount)
            {
                leafWithMinimalData = sibling;
                minimalDataCount = sibling.Data.Count;
            }
        }

        return leafWithMinimalData;
    }
    #endregion

    #region Insertion
    public void Insert(QuadTreeObject<K, V> quadTreeObject)
    {
        var insertedKey = true;
        if(QuadTree.CheckKeysDuplicate)
        {
            insertedKey = QuadTree.InsertKey(quadTreeObject.Key);
        }

        if(!insertedKey)
        {
            throw new ArgumentException($"Key {quadTreeObject.Key} already exists in QuadTree");
        }

        QuadTreeNode<K, V> currentNode = this;
        var insertItem = quadTreeObject.Item;

        while (true)
        {
            if (!currentNode.Boundary.ContainsStrict(insertItem))
                return;

            if (currentNode.IsLeaf())
            {
                if (currentNode.TryAddItem(quadTreeObject))
                {
                    return;
                }

                if (ShouldSubdivide(currentNode, insertItem) && currentNode.Depth < currentNode.QuadTree.MaxAllowedDepth)
                {
                    currentNode.Subdivide(CalculateSubquadrantsBoundaries());
                }
                else
                {
                    // If the spatialITem doesnt fit in any subquadrant, add it to the data
                    currentNode.AddSpatialItemToData(quadTreeObject);
                    //currentNode.Data.Add(quadTreeObject);
                    return;
                }
            }

            //If the SpatialItem fits in any subquadrant, set the currentNode to that subquadrant
            if (currentNode.Depth < currentNode.QuadTree.MaxAllowedDepth && TryMoveToChildContainingRectangle(ref currentNode, insertItem))
            {
                if (currentNode.TryAddItem(quadTreeObject))
                {
                    return;
                }
            }
            else
            {
                currentNode.AddSpatialItemToData(quadTreeObject);
                //currentNode.Data.Add(quadTreeObject);
                return;
            }
        }
    }

    public void InsertOptimalizedIterative(QuadTreeOptimalization<K, V>.SubdivisionResult optimalizationData, int portion)
    {
        var nodeStack = new Stack<QuadTreeNode<K, V>>();
        var dataStack = new Stack<QuadTreeOptimalization<K, V>.SubdivisionResult>();

        nodeStack.Push(this);
        dataStack.Push(optimalizationData);

        while (nodeStack.Count > 0)
        {
            var currentNode = nodeStack.Pop();
            var currentData = dataStack.Pop();

            // Check and handle non-empty quadrants
            bool hasChildrenItems = HasNonEmptyQuadrants(currentData.SortedItems);
            if (hasChildrenItems)
            {
                currentNode.Subdivide(currentData.Quadrants, currentData.VerticalCut, currentData.HorizontalCut);
            }

            // Handle items that don’t fit into any quadrant
            HandleNonFittingItems(currentNode, currentData.SortedItems[4]);

            // If no items are left outside, proceed to find the quadrant with the most items
            if (hasChildrenItems)
            {
                InsertFromLargestQuadrant(currentNode, currentData.SortedItems);
            }

            // Continue with children nodes if necessary
            ProcessChildrenNodes(currentNode, nodeStack, dataStack, currentData.SortedItems, portion);
        }
    }

    private void ProcessChildrenNodes(
        QuadTreeNode<K, V> currentNode,
        Stack<QuadTreeNode<K, V>> nodeStack,
        Stack<QuadTreeOptimalization<K, V>.SubdivisionResult> dataStack,
        List<QuadTreeObject<K, V>>[] sortedItems,
        int portion)
    {
        if (!currentNode.IsLeaf())
        {
            for (int i = 0; i < currentNode.Children.Length; i++)
            {
                var child = currentNode.Children[i];
                if (sortedItems[i].Count == 0) continue;

                var optimalization = QuadTreeOptimalization<K, V>.BestSubdivision(child.Boundary, sortedItems[i], portion);
                nodeStack.Push(child);
                dataStack.Push(optimalization);
            }
        }
    }

    private void InsertFromLargestQuadrant(QuadTreeNode<K, V> currentNode, List<QuadTreeObject<K, V>>[] sortedItems)
    {
        int maxIndex = -1;
        int maxCount = -1;

        for (int i = 0; i < sortedItems.Length - 1; i++)
        {
            if (sortedItems[i].Count > maxCount)
            {
                maxIndex = i;
                maxCount = sortedItems[i].Count;
            }
        }

        if (maxIndex >= 0)
        {
            // Select the last item from the most populated list
            var lastIndex = sortedItems[maxIndex].Count - 1;
            var elementToInsert = sortedItems[maxIndex][lastIndex];

            currentNode.Insert(elementToInsert);
            sortedItems[maxIndex].RemoveAt(lastIndex); // Remove the last item
        }
    }

    private void HandleNonFittingItems(QuadTreeNode<K, V> currentNode, List<QuadTreeObject<K, V>> nonFittingItems)
    {
        foreach (var item in nonFittingItems)
        {
            currentNode.Insert(item);
        }
        nonFittingItems.Clear();
    }

    private bool HasNonEmptyQuadrants(List<QuadTreeObject<K, V>>[] sortedItems)
    {
        return sortedItems.Take(4).Any(quadrantItems => quadrantItems.Count > 0);
    }
    #endregion

    #region Deletion
    public ISpatialItem? Delete(QuadTreeObject<K, V> quadTreeObject) //TODO: Return bool or object
    {
        var deletedKey = true;
        if(QuadTree.CheckKeysDuplicate)
        {
            deletedKey = QuadTree.DeleteKey(quadTreeObject.Key);
        }

        if(!deletedKey)
        {
            return null;
        }

        var result = LocateNodeAndObjectForItem(quadTreeObject.Item, quadTreeObject.Key);
        QuadTreeNode<K, V>? nodeContainingObject = result.foundNode;
        var foundQuadTreeObject = result.foundObject;
        if (nodeContainingObject != null)
        {
            return TryRemoveDataFromNode(nodeContainingObject, foundQuadTreeObject);
        }
        return null;
    }

    private ISpatialItem? TryRemoveDataFromNode(QuadTreeNode<K, V> node, QuadTreeObject<K, V> targetObject)
    {
        ISpatialItem? removedItem = null;

        if (targetObject.Item is Point && node.PointData.Remove(targetObject))
        {
            removedItem = targetObject.Item;
        }
        else if (targetObject.Item is Rectangle && node.RectangleData.Remove(targetObject))
        {
            removedItem = targetObject.Item;
        }

        if (removedItem != null)
        {
            node.Data.Remove(targetObject);
            node.SimplifyIfEmpty();
        }

        return removedItem;
    }
    #endregion

    #region Find
    public List<QuadTreeObject<K, V>>? Find(ISpatialItem rectangle)
    {
        List<QuadTreeObject<K, V>>? foundItems = new List<QuadTreeObject<K, V>>();
        Queue<QuadTreeNode<K, V>> nodesToCheck = new Queue<QuadTreeNode<K, V>>();
        nodesToCheck.Enqueue(this);

        while (nodesToCheck.Count > 0)
        {
            QuadTreeNode<K, V> currentNode = nodesToCheck.Dequeue();
                        
            // Check and add data items contained within the rectangle
            foreach (var kvp in currentNode.Data)
            {
                if (kvp.Item is Rectangle rect && rectangle.ContainsStrict(rect) ||
                    kvp.Item is Point pt && rectangle.ContainsStrict(pt))
                {
                    foundItems.Add(kvp);
                }
                else if(kvp.Item is ISpatialItem spatialItem && rectangle.Intersects(spatialItem))
                {
                    foundItems.Add(kvp);
                }
            }

            // If the current node doesn't have children, proceed to the next node
            if (currentNode.Children is null) continue;

            foreach (Quadrant quadrant in Enum.GetValues(typeof(Quadrant)))
            {
                var childNodeBoundary = currentNode.Children[(int)quadrant].Boundary;
                //TODO: Can use intersects instead of containsstrict?
                if (rectangle.ContainsStrict(childNodeBoundary) || childNodeBoundary.ContainsStrict(rectangle))
                {
                    nodesToCheck.Enqueue(currentNode.Children[(int)quadrant]);
                }
            }
        }
        return foundItems;
    }

    public (QuadTreeNode<K, V>? foundNode, QuadTreeObject<K,V>? foundObject) LocateNodeAndObjectForItem(ISpatialItem searchItem, K key)
    {
        Queue<QuadTreeNode<K, V>> nodesToCheck = new Queue<QuadTreeNode<K, V>>();
        nodesToCheck.Enqueue(this);

        while (nodesToCheck.Count > 0)
        {
            QuadTreeNode<K, V> currentNode = nodesToCheck.Dequeue();

            var quadTreeOject = ContainsDataItem(currentNode, searchItem, key);
            if (quadTreeOject is not null)
            {
                return (currentNode, quadTreeOject);
            }

            if (currentNode.Children != null)
            {
                foreach (Quadrant quadrant in Enum.GetValues(typeof(Quadrant)))
                {
                    var childNodeBoundary = currentNode.Children[(int)quadrant].Boundary;
                    if (childNodeBoundary.ContainsStrict(searchItem) || searchItem.Intersects(childNodeBoundary))
                    {
                        nodesToCheck.Enqueue(currentNode.Children[(int)quadrant]);
                    }
                }
            }
        }
        return (null,null); 
    }
    #endregion

    #region Utility and Helper Methods

    //private bool ContainsDataItem(QuadTreeNode<K, V> node, QuadTreeObject<K, V> targetObject)
    //{
    //    if (targetObject.Item is Point && node.PointData.Contains(targetObject))
    //    {
    //        return true;
    //    }
    //    else if (targetObject.Item is Rectangle && node.RectangleData.Contains(targetObject))
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    private QuadTreeObject<K,V>? ContainsDataItem(QuadTreeNode<K, V> node, ISpatialItem searchItem, K key)
    {

        if(searchItem is Point)
        {
            foreach (var qto in node.PointData)
            {
                if (qto.Key.Equals(key))
                    return qto;
            }
        }

        if (searchItem is Rectangle)
        {
            foreach (var qto in node.RectangleData)
            {
                if (qto.Key.Equals(key))
                    return qto;
            }
        }
        return null;

    }


    private bool TryAddItem(QuadTreeObject<K, V> item)
    {
        if(item.Item is Point)
        {
            foreach (var existingItem in PointData)
            {

                if (existingItem.Key.Equals(item.Key))
                {
                    throw new ArgumentException($"Key {item.Key} already exists in QuadTree");
                }

                if (existingItem.Item.Equals(item.Item))
                {
                    AddSpatialItemToData(item);
                    return true;
                }
            }
        }
        else if(item.Item is Rectangle)
        {
            foreach (var existingItem in RectangleData)
            {

                if (existingItem.Key.Equals(item.Key))
                {
                    throw new ArgumentException($"Key {item.Key} already exists in QuadTree");
                }

                if (existingItem.Item.Equals(item.Item))
                {
                    AddSpatialItemToData(item);
                    return true;
                }
            }
        }
        else
        {
            throw new ArgumentException("Item is not a Point or Rectangle");
        }

        // If we reached here, then there's no matching item.
        // So, if the node data was empty, we add the new item.
        if (Data.Count == 0)
        {
            AddSpatialItemToData(item);
            return true;
        }

        return false;
    }

    private (double verticalCut, double horizontalCut) CalculateMidPoints() =>
        ((Boundary.LowerLeft.X + Boundary.UpperRight.X) / 2.0, (Boundary.LowerLeft.Y + Boundary.UpperRight.Y) / 2.0);

    public double DataScore()
    {
        if (IsLeaf() && Data.Count == 0)
        {
            throw new InvalidOperationException("DataScore cannot be calculated for an empty leaf node");
        }


        return 1.0 / (1 + (Math.Log(Data.Count) / Math.Log(2)) * QuadTreeOptimalization<K, V>.ScalingFactor); // Adjust the divisor as needed
    }


    private Rectangle[] CalculateSubquadrantsBoundaries()
    {
        return new Rectangle[]
        {
            // NorthWest
            new Rectangle(new Point(Boundary.LowerLeft.X, HorizontalCut), new Point(VerticalCut, Boundary.UpperRight.Y)),
            // NorthEast
            new Rectangle(new Point(VerticalCut, HorizontalCut), Boundary.UpperRight),
            // SouthWest
            new Rectangle(Boundary.LowerLeft, new Point(VerticalCut, HorizontalCut)),
            // SouthEast
            new Rectangle(new Point(VerticalCut, Boundary.LowerLeft.Y), new Point(Boundary.UpperRight.X, HorizontalCut))
        };
    }

    private bool ShouldSubdivide(QuadTreeNode<K, V> node, ISpatialItem rectangle)
    {
        return node.Data.Count != 0 && node.CalculateSubquadrantsBoundaries().Any(boundary => boundary.ContainsStrict(rectangle));
    }

    private bool TryMoveToChildContainingRectangle(ref QuadTreeNode<K, V> node, ISpatialItem rectangle)
    {
        if (TryGetFittingQuadrant(node, rectangle, out Quadrant? fittingQuadrant))
        {
            node = node.Children[(int)fittingQuadrant.Value];
            return true;
        }

        return false;
    }

    /// <summary>
    /// Attempts to find a quadrant of the specified node that fully contains the provided rectangle.
    /// </summary>
    /// <param name="node">The node whose children quadrants are to be examined.</param>
    /// <param name="rectangle">The rectangle to check for containment within the quadrants.</param>
    /// <param name="fittingQuadrant">When this method returns, contains the quadrant which fully contains the rectangle, if found; otherwise, null.</param>
    /// <returns>True if a fitting quadrant was found; otherwise, false.</returns>
    private bool TryGetFittingQuadrant(QuadTreeNode<K, V> node, ISpatialItem rectangle, out Quadrant? fittingQuadrant)
    {
        foreach (Quadrant quadrant in Enum.GetValues(typeof(Quadrant)))
        {
            if (node.Children[(int)quadrant].Boundary.ContainsStrict(rectangle))
            {
                fittingQuadrant = quadrant;
                return true;
            }
        }

        fittingQuadrant = null;
        return false;
    }
    #endregion

    #region Traversal
    public List<QuadTreeNode<K, V>> PreorderTraversal(Action<QuadTreeNode<K, V>>? action = null)
    {
        List<QuadTreeNode<K, V>> result = new List<QuadTreeNode<K, V>>();
        Stack<QuadTreeNode<K, V>> stack = new Stack<QuadTreeNode<K, V>>();
        stack.Push(this);

        while (stack.Count > 0)
        {
            QuadTreeNode<K, V> currentNode = stack.Pop();
            result.Add(currentNode);
            action?.Invoke(currentNode); // Execute the passed action on the current node if provided

            if (currentNode.Children != null)
            {
                for (int i = 3; i >= 0; i--) // pushing in reverse order so they are processed from left to right
                {
                    stack.Push(currentNode.Children[i]);
                }
            }
        }

        return result;
    }

    public List<QuadTreeNode<K, V>> InOrderTraversal(Action<QuadTreeNode<K, V>>? action = null)
    {
        List<QuadTreeNode<K, V>> result = new List<QuadTreeNode<K, V>>();
        if (this == null) return result;

        Stack<(QuadTreeNode<K, V> Node, bool Visited)> stack = new Stack<(QuadTreeNode<K, V> Node, bool Visited)>();
        stack.Push((this, false));

        while (stack.Count > 0)
        {
            var (current, visited) = stack.Pop();

            if (current == null) continue;

            if (visited)
            {
                result.Add(current);
                action?.Invoke(current); // Execute the passed action on the current node if provided

                if (current.Children is null) continue;
                stack.Push((current.Children[(int)Quadrant.SouthEast], false));
                stack.Push((current.Children[(int)Quadrant.SouthWest], false));

            }
            else
            {
                if (current.Children is null)
                {
                    stack.Push((current, true));
                    continue;
                }
                stack.Push((current, true));
                stack.Push((current.Children[(int)Quadrant.NorthEast], false));
                stack.Push((current.Children[(int)Quadrant.NorthWest], false));


            }
        }

        return result;
    }

    public List<QuadTreeNode<K, V>> PostOrderTraversal(Action<QuadTreeNode<K, V>>? action = null)
    {
        if (this is null) return new List<QuadTreeNode<K, V>>();

        Stack<QuadTreeNode<K, V>> stack1 = new Stack<QuadTreeNode<K, V>>();
        Stack<QuadTreeNode<K, V>> stack2 = new Stack<QuadTreeNode<K, V>>();
        List<QuadTreeNode<K, V>> result = new List<QuadTreeNode<K, V>>();

        stack1.Push(this);

        while (stack1.Count > 0)
        {
            QuadTreeNode<K, V> currentNode = stack1.Pop();

            if (currentNode.Children != null)
            {
                foreach (var child in currentNode.Children)
                {
                    if (child != null)
                    {
                        stack1.Push(child);
                    }
                }
            }

            stack2.Push(currentNode);
        }

        // Now pop nodes from the second stack to get them in postorder and add them to the result.
        while (stack2.Count > 0)
        {
            var node = stack2.Pop();

            result.Add(node);
            action?.Invoke(node); // Execute the passed action on the current node if provided
        }

        return result;
    }

    public void ReduceDepth()
    {
        Stack<QuadTreeNode<K, V>> stack1 = new Stack<QuadTreeNode<K, V>>();
        Stack<QuadTreeNode<K, V>> stack2 = new Stack<QuadTreeNode<K, V>>();

        stack1.Push(this);

        while (stack1.Count > 0)
        {
            QuadTreeNode<K, V> currentNode = stack1.Pop();

            if (currentNode.Children != null)
            {
                foreach (var child in currentNode.Children)
                {
                    if (child != null)
                    {
                        stack1.Push(child);
                    }
                }
            }

            stack2.Push(currentNode);
        }

        // Now pop nodes from the second stack to get them in postorder and add them to the result.
        while (stack2.Count > 0)
        {
            var node = stack2.Pop();

            if(node.Depth > node.QuadTree.MaxAllowedDepth)
            {
                if (node.TryMakeLeaf())
                {
                    foreach (var data in node.Data)
                    {
                        QuadTree.Insert(data);
                    }

                    node.Data.Clear();
                    node.PointData.Clear();
                    node.RectangleData.Clear();
                }
            }
            else
            {
                node.TryMakeLeaf();
            }
        }

    }

    public void IncreaseDepth()
    {
        List<QuadTreeNode<K, V>> leafNodes = new List<QuadTreeNode<K, V>>();
        Stack<QuadTreeNode<K, V>> stack = new Stack<QuadTreeNode<K, V>>();
        stack.Push(this);

        while (stack.Count > 0)
        {
            QuadTreeNode<K, V> currentNode = stack.Pop();
            if(currentNode.IsLeaf())
            {
                leafNodes.Add(currentNode);
            }

            if (currentNode.Children != null)
            {
                for (int i = 3; i >= 0; i--) // pushing in reverse order so they are processed from left to right
                {
                    stack.Push(currentNode.Children[i]);
                }
            }
        }

        var data = new List<QuadTreeObject<K, V>>();
        foreach (var leaf in leafNodes)
        {
            foreach (var item in leaf.Data)
            {
                data.Add(item);
            }
            leaf.Data.Clear();
            leaf.PointData.Clear();
            leaf.RectangleData.Clear();
        }

        foreach (var item in data)
        {
            QuadTree.Insert(item);
        }
    }
    #endregion

    public (Point BottomLeft, Point TopRight) FindBoundaryPoints<K, V>(List<QuadTreeNode<K, V>> nodeList) where K : IComparable<K>
    {
        double minX = Double.MaxValue;
        double minY = Double.MaxValue;
        double maxX = Double.MinValue;
        double maxY = Double.MinValue;

        foreach (var node in nodeList)
        {
            foreach (var obj in node.Data)
            {
                var spatialItem = obj.Item;

                // Update minX and minY for bottom-left point
                if (spatialItem.LowerLeft.X < minX)
                {
                    minX = spatialItem.LowerLeft.X;
                }
                if (spatialItem.LowerLeft.Y < minY)
                {
                    minY = spatialItem.LowerLeft.Y;
                }

                // Update maxX and maxY for top-right point
                if (spatialItem.UpperRight.X > maxX)
                {
                    maxX = spatialItem.UpperRight.X;
                }
                if (spatialItem.UpperRight.Y > maxY)
                {
                    maxY = spatialItem.UpperRight.Y;
                }
            }
        }

        Point bottomLeft = new Point(minX, minY);
        Point topRight = new Point(maxX, maxY);

        return (bottomLeft, topRight);
    }

    private void AddSpatialItemToData(QuadTreeObject<K, V> quadTreeObject)
    {
        if (quadTreeObject.Item is Point)
        {
            PointData.Add(quadTreeObject);
        }
        else if (quadTreeObject.Item is Rectangle)
        {
            RectangleData.Add(quadTreeObject);
        }

        Data.Add(quadTreeObject);
    }
}