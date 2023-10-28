using QuadTreeDS.SpatialItems;

namespace QuadTreeDS.QuadTree;

public class QuadTreeNode<K, V> where K : IComparable<K>
{
    #region Constructor and properties
    public Rectangle Boundary { get; }
    public int Depth { get; }
    public int MaxSubtreeDepth { get; private set; } = 0;
    public List<QuadTreeObject<K, V>> Data { get; private set; } = new List<QuadTreeObject<K, V>>(); //TODO: Rectangle Data and point Data should be seperated
    public QuadTreeNode<K, V>[]? Children { get; private set; }
    public QuadTreeNode<K, V>? Parent { get; }

    private const int QUADRANT_COUNT = 4;
    public QuadTree<K, V> QuadTree { get; private set; }

    public QuadTreeNode(Rectangle boundary, QuadTreeNode<K, V>? parent, QuadTree<K, V> quadTree)
    {
        Boundary = boundary;
        Parent = parent;
        Depth = parent?.Depth + 1 ?? 0;
        QuadTree = quadTree;
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
    private void Subdivide()
    {
        if (IsLeaf())
        {
            Children = new QuadTreeNode<K, V>[QUADRANT_COUNT];
            var boundaries = CalculateSubquadrantsBoundaries();

            for (int i = 0; i < QUADRANT_COUNT; i++)
            {
                Children[i] = new QuadTreeNode<K, V>(boundaries[i], this, QuadTree);
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
            deepestLeaf.Data = new List<QuadTreeObject<K, V>>(); 
            //TODO: look at the siblings of the deepest leaf and see if they can be simplified
            if(deepestLeaf.Data.Count == 0 && deepestLeaf.Parent.AreChildrenLeavesAndEmpty())
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
        if (quadTreeObject.Item is Point)
        {
            //InsertPoint(quadTreeObject);
            InsertRectangle(quadTreeObject);
        }
        else if (quadTreeObject.Item is Rectangle)
        {
            InsertRectangle(quadTreeObject);
        }
        else
        {
            throw new ArgumentException("Item in object must be either a Point or a Rectangle");
        }
    }

    private void InsertPoint(QuadTreeObject<K, V> quadTreeObject)
    {
        QuadTreeNode<K, V> currentNode = this;
        var itemPoint = quadTreeObject.Item;

        while (true)
        {
            if (!currentNode.Boundary.ContainsStrict(itemPoint))
                return;

            if (currentNode.IsLeaf())
            {
                if (currentNode.TryAddItem(quadTreeObject))
                {
                    return;
                }

                if (currentNode.Depth < currentNode.QuadTree.MaxAllowedDepth)
                {
                    currentNode.Subdivide();
                }
                else
                {
                    currentNode.Data.Add(quadTreeObject);
                    return;
                }

            }

            Quadrant? targetQuadrant = currentNode.DetermineQuadrant(itemPoint);
            if (targetQuadrant is not null)
            {
                currentNode = currentNode.Children[(int)targetQuadrant.Value];
            }
            else
            {
                throw new InvalidOperationException("DetermineQuadrant returned null");
            }
        }
    }

    private void InsertRectangle(QuadTreeObject<K, V> quadTreeObject)
    {
        QuadTreeNode<K, V> currentNode = this;
        //Rectangle rectangle = (Rectangle)quadTreeObject.Item;
        var rectangle = quadTreeObject.Item;

        while (true)
        {
            if (!currentNode.Boundary.ContainsStrict(rectangle))
                return;

            if (currentNode.IsLeaf())
            {
                if (currentNode.TryAddItem(quadTreeObject))
                {
                    return;
                }

                if (ShouldSubdivide(currentNode, rectangle) && currentNode.Depth < currentNode.QuadTree.MaxAllowedDepth)
                {
                    currentNode.Subdivide();
                }
                else
                {
                    // If the rectangle doesnt fit in any subquadrant, add it to the data
                    currentNode.Data.Add(quadTreeObject);
                    return;
                }
            }


            //If the rectangle fits in any subquadrant, set the currentNode to that subquadrant
            if (TryMoveToChildContainingRectangle(ref currentNode, rectangle))
            {
                if (currentNode.TryAddItem(quadTreeObject))
                {
                    return;
                }
            }
            else
            {
                currentNode.Data.Add(quadTreeObject);
                return;
            }
        }
    }
    #endregion

    #region Deletion
    public void Delete(QuadTreeObject<K, V> quadTreeObject) //TODO: Return bool or object
    {
        if (quadTreeObject.Item is Point)
        {
            //DeletePoint(quadTreeObject);
            DeleteRectangle(quadTreeObject);
        }
        else if (quadTreeObject.Item is Rectangle)
        {
            DeleteRectangle(quadTreeObject);
        }
        else
        {
            throw new ArgumentException("Item in object must be either a Point or a Rectangle");
        }
    }

    private void DeletePoint(QuadTreeObject<K, V> quadTreeObject)
    {
        QuadTreeNode<K, V> currentNode = this;

        while (currentNode != null)
        {
            if (TryRemoveDataFromNode(currentNode, quadTreeObject))
                return;

            if (currentNode.IsLeaf())
            {
                return;
            }

            Point point = quadTreeObject.Item as Point;
            Quadrant? chosenQuadrant = currentNode.DetermineQuadrant(point);
            currentNode = chosenQuadrant is not null ? currentNode.Children[(int)chosenQuadrant.Value] : null;
        }
    }

    private void DeleteRectangle(QuadTreeObject<K, V> quadTreeObject)
    {
        SpatialItem rectangle = quadTreeObject.Item;// as Rectangle;
        Queue<QuadTreeNode<K, V>> nodesToCheck = new Queue<QuadTreeNode<K, V>>();
        nodesToCheck.Enqueue(this);

        while (nodesToCheck.Count > 0)
        {
            QuadTreeNode<K, V> currentNode = nodesToCheck.Dequeue();

            if (TryRemoveDataFromNode(currentNode, quadTreeObject))
                return;

            if (currentNode.Children is null) continue;

            foreach (Quadrant quadrant in Enum.GetValues(typeof(Quadrant)))
            {
                var childNodeBoundary = currentNode.Children[(int)quadrant].Boundary;

                if (childNodeBoundary.ContainsStrict(rectangle))
                {
                    nodesToCheck.Enqueue(currentNode.Children[(int)quadrant]);
                }
            }
        }
    }

    private bool TryRemoveDataFromNode(QuadTreeNode<K, V> node, QuadTreeObject<K, V> targetObject)
    {
        for (int i = 0; i < node.Data.Count; i++)
        {
            if (node.Data[i] == targetObject)
            {
                node.Data.RemoveAt(i);
                node.SimplifyIfEmpty();
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Find
    public List<QuadTreeObject<K, V>>? Find(QuadTreeObject<K, V> quadTreeObject)
    {
        if (quadTreeObject.Item is Point)
        {
            //return FindPoint((Point)quadTreeObject.Item);
            return FindRectangle(quadTreeObject.Item);
        }
        else if (quadTreeObject.Item is Rectangle)
        {
            return FindRectangle((Rectangle)quadTreeObject.Item);
        }
        else
        {
            throw new ArgumentException("Item in object must be either a Point or a Rectangle");
        }
    }

    private List<QuadTreeObject<K, V>>? FindPoint(Point point)
    {
        List<QuadTreeObject<K, V>>? foundItems = new List<QuadTreeObject<K, V>>();
        QuadTreeNode<K, V> currentNode = this;

        while (currentNode != null)
        {
            foreach (var kvp in currentNode.Data)
            {
                if (kvp.Item is Point currentPoint && currentPoint.Equals(point))
                {
                    foundItems.Add(kvp);
                }
            }

            if (foundItems.Count > 0 || currentNode.IsLeaf())
            {
                return foundItems;
            }

            Quadrant? chosenQuadrant = currentNode.DetermineQuadrant(point);
            currentNode = chosenQuadrant is not null ? currentNode.Children[(int)chosenQuadrant.Value] : null;
        }
        return null;
    }

    private List<QuadTreeObject<K, V>> FindRectangle(SpatialItem rectangle)
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
            }

            // If the current node doesn't have children, proceed to the next node
            if (currentNode.Children is null) continue;

            foreach (Quadrant quadrant in Enum.GetValues(typeof(Quadrant)))
            {
                var childNodeBoundary = currentNode.Children[(int)quadrant].Boundary;

                if (rectangle.ContainsStrict(childNodeBoundary) || childNodeBoundary.ContainsStrict(rectangle))
                {
                    nodesToCheck.Enqueue(currentNode.Children[(int)quadrant]);
                }
            }
        }
        return foundItems;
    }
    #endregion

    #region Utility and Helper Methods
    private bool TryAddItem(QuadTreeObject<K, V> item)
    {

        foreach (var existingItem in Data)
        {

            if (existingItem.Key.Equals(item.Key))
            {
                throw new ArgumentException($"Key {item.Key} already exists in QuadTree");
            }

            if (existingItem.Item.Equals(item.Item))
            {
                Data.Add(item);
                return true;
            }
        }

        // If we reached here, then there's no matching item.
        // So, if the node data was empty, we add the new item.
        if (Data.Count == 0)
        {
            Data.Add(item);
            return true;
        }

        return false;
    }

    private (double, double) CalculateMidPoints() =>
        ((Boundary.LowerLeft.X + Boundary.UpperRight.X) / 2.0, (Boundary.LowerLeft.Y + Boundary.UpperRight.Y) / 2.0);

    private List<Rectangle> CalculateSubquadrantsBoundaries()
    {
        (double midX, double midY) = CalculateMidPoints();

        return new List<Rectangle>
        {
            // NorthWest
            new Rectangle(new Point(Boundary.LowerLeft.X, midY), new Point(midX, Boundary.UpperRight.Y)),
            // NorthEast
            new Rectangle(new Point(midX, midY), Boundary.UpperRight),
            // SouthWest
            new Rectangle(Boundary.LowerLeft, new Point(midX, midY)),
            // SouthEast
            new Rectangle(new Point(midX, Boundary.LowerLeft.Y), new Point(Boundary.UpperRight.X, midY))
        };
    }

    private Quadrant? DetermineQuadrant(SpatialItem point)
    {
        (double midX, double midY) = CalculateMidPoints();
        point = point as Point ?? throw new ArgumentException("SpatialItem must be a Point");

        return point switch
        {
            _ when point.LowerLeft.X < midX && point.LowerLeft.Y >= midY => Quadrant.NorthWest,
            _ when point.LowerLeft.X >= midX && point.LowerLeft.Y >= midY => Quadrant.NorthEast,
            _ when point.LowerLeft.X < midX && point.LowerLeft.Y < midY => Quadrant.SouthWest,
            _ when point.LowerLeft.X >= midX && point.LowerLeft.Y < midY => Quadrant.SouthEast,
            _ => null
        };
    }

    private bool ShouldSubdivide(QuadTreeNode<K, V> node, SpatialItem rectangle)
    {
        return node.Data.Count != 0 && node.CalculateSubquadrantsBoundaries().Any(boundary => boundary.ContainsStrict(rectangle));
    }

    private bool TryMoveToChildContainingRectangle(ref QuadTreeNode<K, V> node, SpatialItem rectangle)
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
    private bool TryGetFittingQuadrant(QuadTreeNode<K, V> node, SpatialItem rectangle, out Quadrant? fittingQuadrant)
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
    public List<QuadTreeNode<K, V>> PreorderTraversal()
    {
        List<QuadTreeNode<K, V>> result = new List<QuadTreeNode<K, V>>();
        Stack<QuadTreeNode<K, V>> stack = new Stack<QuadTreeNode<K, V>>();
        stack.Push(this);

        while (stack.Count > 0)
        {
            QuadTreeNode<K, V> currentNode = stack.Pop();
            result.Add(currentNode);

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

    public List<QuadTreeNode<K, V>> InOrderTraversal()
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

    //First left subtree then right subtree then root
    public List<QuadTreeNode<K, V>> PostOrderTraversal()
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
            result.Add(stack2.Pop());
        }

        return result;
    }



    #endregion
}