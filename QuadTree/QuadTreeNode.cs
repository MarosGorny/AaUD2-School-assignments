using QuadTree.SpatialItems;

namespace QuadTree;

public class QuadTreeNode<K, V> where K : IComparable<K>
{
    #region Constructor and properties
    private const int QUADRANT_COUNT = 4;
    public Rectangle Boundary { get; }
    public int Depth { get; }
    public int MaxSubtreeDepth { get; private set; } = 0;
    public List<QuadTreeObject<K, V>> Data { get; private set; } = new List<QuadTreeObject<K, V>>();
    public QuadTreeNode<K, V>[]? Children { get; private set; }
    public QuadTreeNode<K, V>? Parent { get; }

    public QuadTreeNode(Rectangle boundary, QuadTreeNode<K, V>? parent)
    {
        Boundary = boundary;
        Parent = parent;
        Depth = parent?.Depth + 1 ?? 0;
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
                Children[i] = new QuadTreeNode<K, V>(boundaries[i], this);
            }
            UpdateMaxSubtreeDepthAfterSubdivision();
        }
    }

    private void UpdateMaxSubtreeDepthAfterSubdivision()
    {
        this.MaxSubtreeDepth = 1;
        UpdateAncestorsMaxSubtreeDepth();
    }

    private void UpdateAncestorsMaxSubtreeDepth()
    {
        QuadTreeNode<K, V>? currentNode = this.Parent;
        int childDepth = this.MaxSubtreeDepth;

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
        if (this.Data.Count == 0 && !IsLeaf())
        {
            bool canSimplify = true;

            foreach (var child in Children)
            {
                if (!child.IsLeaf() || child.Data.Count > 0)
                {
                    canSimplify = false;
                    break;
                }
            }

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

    private void TransferDataFromDeepestLeaf()
    {
        var deepestLeaf = GetDeepestLeafWithMinimalData();
        if (deepestLeaf != null)
        {
            this.Data = deepestLeaf.Data;
            deepestLeaf.Data = new List<QuadTreeObject<K, V>>();
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
            InsertPoint(quadTreeObject);
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
        Point itemPoint = (Point)quadTreeObject.Item;

        while (true)
        {
            if (!currentNode.Boundary.ContainsPoint(itemPoint))
                return;

            if (currentNode.IsLeaf())
            {
                if (currentNode.TryAddItem(quadTreeObject))
                {
                    return;
                }

                currentNode.Subdivide();

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
        Rectangle rectangle = (Rectangle)quadTreeObject.Item;

        while (true)
        {
            if (!currentNode.Boundary.Contains(rectangle))
                return;

            if (currentNode.IsLeaf())
            {
                if (currentNode.TryAddItem(quadTreeObject))
                {
                    return;
                }

                if (ShouldSubdivide(currentNode, rectangle))
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
            DeletePoint(quadTreeObject);
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
        Rectangle rectangle = quadTreeObject.Item as Rectangle;
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

                if (childNodeBoundary.Contains(rectangle))
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
                node.SimplifyIfEmpty(); // Or whatever you renamed ReduceIfEmpty to.
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
            return FindPoint((Point)quadTreeObject.Item);
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

    private List<QuadTreeObject<K, V>> FindRectangle(Rectangle rectangle)
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
                if ((kvp.Item is Rectangle rect && rectangle.Contains(rect)) ||
                    (kvp.Item is Point pt && rectangle.ContainsPoint(pt)))
                {
                    foundItems.Add(kvp);
                }
            }

            // If the current node doesn't have children, proceed to the next node
            if (currentNode.Children is null) continue;

            foreach (Quadrant quadrant in Enum.GetValues(typeof(Quadrant)))
            {
                var childNodeBoundary = currentNode.Children[(int)quadrant].Boundary;

                if (rectangle.Contains(childNodeBoundary) || childNodeBoundary.Contains(rectangle))
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
        ((Boundary.BottomLeft.X + Boundary.TopRight.X) / 2.0, (Boundary.BottomLeft.Y + Boundary.TopRight.Y) / 2.0);

    private List<Rectangle> CalculateSubquadrantsBoundaries()
    {
        (double midX, double midY) = CalculateMidPoints();

        return new List<Rectangle>
        {
            // NorthWest
            new Rectangle(new Point(Boundary.BottomLeft.X, midY), new Point(midX, Boundary.TopRight.Y)),
            // NorthEast
            new Rectangle(new Point(midX, midY), Boundary.TopRight),
            // SouthWest
            new Rectangle(Boundary.BottomLeft, new Point(midX, midY)),
            // SouthEast
            new Rectangle(new Point(midX, Boundary.BottomLeft.Y), new Point(Boundary.TopRight.X, midY))
        };
    }

    private Quadrant? DetermineQuadrant(Point point)
    {
        (double midX, double midY) = CalculateMidPoints();

        return point switch
        {
            _ when point.X < midX && point.Y >= midY => Quadrant.NorthWest,
            _ when point.X >= midX && point.Y >= midY => Quadrant.NorthEast,
            _ when point.X < midX && point.Y < midY => Quadrant.SouthWest,
            _ when point.X >= midX && point.Y < midY => Quadrant.SouthEast,
            _ => null
        };
    }

    private bool ShouldSubdivide(QuadTreeNode<K, V> node, Rectangle rectangle)
    {
        return node.Data.Count != 0 && node.CalculateSubquadrantsBoundaries().Any(boundary => boundary.Contains(rectangle));
    }

    private bool TryMoveToChildContainingRectangle(ref QuadTreeNode<K, V> node, Rectangle rectangle)
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
    private bool TryGetFittingQuadrant(QuadTreeNode<K, V> node, Rectangle rectangle, out Quadrant? fittingQuadrant)
    {
        foreach (Quadrant quadrant in Enum.GetValues(typeof(Quadrant)))
        {
            if (node.Children[(int)quadrant].Boundary.Contains(rectangle))
            {
                fittingQuadrant = quadrant;
                return true;
            }
        }

        fittingQuadrant = null;
        return false;
    }
    #endregion
}