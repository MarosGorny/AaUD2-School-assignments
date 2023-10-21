using QuadTree.SpatialItems;

namespace QuadTree;

public class QuadTreeNode<K, V> where K : IComparable<K>
{
    private const int QUADRANT_COUNT = 4;

    public Rectangle Boundary { get; }
    public int Depth { get; }
    public int SubTreeMaxItemsSize { get; private set; }
    public List<QuadTreeObject<K, V>> Data { get; } = new List<QuadTreeObject<K, V>>();
    public QuadTreeNode<K, V>[]? Children { get; private set; }
    public QuadTreeNode<K, V>? Parent { get; }

    public QuadTreeNode(Rectangle boundary, QuadTreeNode<K, V>? parent)
    {
        Boundary = boundary;
        Parent = parent;
        Depth = parent?.Depth + 1 ?? 0;
    }

    public bool IsRoot() => Parent == null;
    public bool IsLeaf() => Children == null;

    public bool ContainsKey(K key, out K? returnedKey)
    {
        foreach (var kvp in Data)
        {
            if (kvp.Key.CompareTo(key) == 0)
            {
                returnedKey = kvp.Key;
                return true;
            }
        }
        returnedKey = default;
        return false;
    }

    public void MakeLeaf()
    {
        Children = null;
    }

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
            if (targetQuadrant.HasValue)
            {
                currentNode = currentNode.Children[(int)targetQuadrant.Value];
            }
            else
            {
                throw new InvalidOperationException("DetermineQuadrant returned null");
            }
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
            currentNode = chosenQuadrant.HasValue ? currentNode.Children[(int)chosenQuadrant.Value] : null;
        }
        return null;
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
            if (currentNode.Children == null) continue;

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

    private bool TryAddItem(QuadTreeObject<K, V> item)
    {
        bool nodeDataIsEmpty = true;
        foreach (var existingItem in Data)
        {
            nodeDataIsEmpty = false;

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
        if (nodeDataIsEmpty)
        {
            Data.Add(item);
            return true;
        }

        return false;
    }

    private (double, double) CalculateMidPoints() =>
        ((Boundary.BottomLeft.X + Boundary.TopRight.X) / 2.0, (Boundary.BottomLeft.Y + Boundary.TopRight.Y) / 2.0);


    public Quadrant? DetermineQuadrant(Point point)
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

    public void Subdivide()
    {
        if (IsLeaf())
        {
            Children = new QuadTreeNode<K, V>[QUADRANT_COUNT];
            var boundaries = CalculateSubquadrantsBoundaries();

            for (int i = 0; i < QUADRANT_COUNT; i++)
            {
                Children[i] = new QuadTreeNode<K, V>(boundaries[i], this);
            }
        }
    }

    public List<Rectangle> CalculateSubquadrantsBoundaries()
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
}