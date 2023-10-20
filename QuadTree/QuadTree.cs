using QuadTree.SpatialItems;

namespace QuadTree;


public class QuadTreeNode<K,V> where K : IComparable<K>
{
    private const int QUADRANT_COUNT = 4;

    public Rectangle Boundary { get;}
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

    public void NullChildren()
    {
        Children = null;
    }

    public void Insert(QuadTreeObject<K,V> quadTreeObject)
    {
        if (quadTreeObject.Item is Point)
        {
            //InsertPoint(quadTreeObject);
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

    /// <summary>
    /// Inserts a rectangle and its associated data into the quadtree.
    /// </summary>
    /// <param name="rectangle">The rectangle to be inserted.</param>
    /// <param name="value">The data associated with the rectangle.</param>
    public void InsertRectangle(QuadTreeObject<K, V> quadTreeObject)
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
            if (!TryMoveToChildContainingRectangle(ref currentNode, rectangle))
            {
                if (currentNode.TryAddItem(quadTreeObject))
                {
                    return;
                }
            }
        }
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



/// <summary>
/// Represents a QuadTree data structure for spatial partitioning.
/// </summary>
/// <typeparam name="T">The type of data associated with spatial items in the quadtree.</typeparam>
public class QuadTree<K,V> where K : IComparable<K>
{
    /// <summary>
    /// Gets the root node of the QuadTree.
    /// </summary>
    public QuadTreeNode<K,V> Root { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuadTree{T}"/> class and subdivides the root node.
    /// </summary>
    /// <param name="boundary">The boundary rectangle that defines the spatial limits of the quadtree.</param>
    public QuadTree(Rectangle boundary)
    {
        Root = new QuadTreeNode<K, V>(boundary,null);
        //Root.Subdivide();
    }

    private bool TryAddItemToNode(QuadTreeNode<K, V> node, QuadTreeObject<K, V> item)
    {
        bool nodeDataIsEmpty = true;
        foreach (var existingItem in node.Data)
        {
            nodeDataIsEmpty = false;

            if (existingItem.Key.Equals(item.Key))
            {
                throw new ArgumentException($"Key {item.Key} already exists in QuadTree");
            }

            if (existingItem.Item.Equals(item.Item))
            {
                node.Data.Add(item);
                return true;
            }
        }

        // If we reached here, then there's no matching item.
        // So, if the node data was empty, we add the new item.
        if (nodeDataIsEmpty)
        {
            node.Data.Add(item);
            return true;
        }

        return false;
    }



    public void Insert(QuadTreeObject<K, V> quadTreeObject)
    {
        if(quadTreeObject.Item is Point)
        {
            InsertPoint(quadTreeObject);
        } else if (quadTreeObject.Item is Rectangle)
        {
            InsertRectangle(quadTreeObject);
        } else
        {
            throw new ArgumentException("Item in object must be either a Point or a Rectangle");
        }
    }

    /// <summary>
    /// Inserts a point and its associated data into the quadtree.
    /// </summary>
    /// <param name="point">The point to be inserted.</param>
    /// <param name="value">The data associated with the point.</param>
    public void InsertPoint(QuadTreeObject<K,V> quadTreeObject)
    {
        QuadTreeNode<K, V> currentNode = Root;
        Point itemPoint = (Point)quadTreeObject.Item;

        while (true)
        {
            if (!currentNode.Boundary.ContainsPoint(itemPoint))
                return;

        if (currentNode.IsLeaf())
        {
            if (TryAddItemToNode(currentNode, quadTreeObject))
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

    /// <summary>
    /// Inserts a rectangle and its associated data into the quadtree.
    /// </summary>
    /// <param name="rectangle">The rectangle to be inserted.</param>
    /// <param name="value">The data associated with the rectangle.</param>
    public void InsertRectangle(QuadTreeObject<K, V> quadTreeObject)
    {
        QuadTreeNode<K,V> currentNode = Root;
        Rectangle rectangle = (Rectangle)quadTreeObject.Item;

        while (true)
        {
            if (!currentNode.Boundary.Contains(rectangle))
                return;

            if (currentNode.IsLeaf())
            {
                if (TryAddItemToNode(currentNode, quadTreeObject))
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
                if(TryAddItemToNode(currentNode, quadTreeObject))
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

    private bool CanFitInAnySubquadrant(QuadTreeNode<K, V> node, Rectangle rectangle)
    {
        foreach (Rectangle boundary in node.CalculateSubquadrantsBoundaries())
        {
            if (boundary.Contains(rectangle))
                return true;
        }
        return false;
    }




    private Quadrant? GetFittingQuadrant(QuadTreeNode<K, V> node, Rectangle rectangle)
    {
        foreach (Quadrant quadrant in Enum.GetValues(typeof(Quadrant)))
        {
            if (node.Children[(int)quadrant].Boundary.Contains(rectangle))
                return quadrant;
        }
        return null;
    }

    private bool FitsInAQuadrant(QuadTreeNode<K,V> currentNode, Rectangle rectangle, out Quadrant? returnedQuadrant)
    {
        foreach (Quadrant quadrant in Enum.GetValues(typeof(Quadrant)))
        {
            if (currentNode.Children[(int)quadrant].Boundary.Contains(rectangle))
            {
                returnedQuadrant = quadrant;
                return true;
            }
        }
        returnedQuadrant = null;
        return false;
    }

    /// <summary>
    /// Finds an item in the quadtree based on the provided spatial key.
    /// </summary>
    /// <param name="key">The spatial item (either a Point or a Rectangle) to be found.</param>
    /// <returns>A list of KeyValuePairs containing the key and its associated data if found, otherwise null.</returns>
    /// <exception cref="ArgumentException">Thrown when the key is neither a Point nor a Rectangle.</exception>
    public List<QuadTreeObject<K,V>>? Find(QuadTreeObject<K,V> quadTreeObject)
    {
        if (quadTreeObject.Item is Point)
        {
            return FindPoint((Point)quadTreeObject.Item);
        }
        else if (quadTreeObject.Item is Rectangle)
        {
            return FindRectangle((Rectangle)quadTreeObject.Item);
        } else
        {
            throw new ArgumentException("Item in object must be either a Point or a Rectangle");
        }
    }

    /// <summary>
    /// Finds a point in the quadtree and retrieves its associated data.
    /// </summary>
    /// <param name="point">The point to be found.</param>
    /// <returns>A list of KeyValuePairs containing the point and its associated data if found, otherwise null.</returns>
    public List<QuadTreeObject<K, V>>? FindPoint(Point point)
    {
        List<QuadTreeObject<K, V>>? foundItems = new List<QuadTreeObject<K, V>>();
        QuadTreeNode<K,V> currentNode = Root;

        while(true)
        {

            if (currentNode.Data.Count != 0)
            {

                foreach (var kvp in currentNode.Data)
                {
                    if (kvp.Item is Point && kvp.Item == (SpatialItem)point)
                    {
                        foundItems.Add(kvp);
                    }
                }

                if (foundItems .Count > 0)
                    return foundItems;

            }

            if (currentNode.Children == null)
                return null;

            Quadrant? choosenQuadrant = currentNode.DetermineQuadrant(point);

            //If the point is not in any quadrant, return null
            if (choosenQuadrant == null)
            {
                return null;
            }

            currentNode = currentNode.Children[(int)choosenQuadrant];
        }
    }

    /// <summary>
    /// Finds all spatial items in the quadtree that are contained within the provided rectangle.
    /// </summary>
    /// <param name="rectangle">The rectangle within which to search for items.</param>
    /// <returns>A list of KeyValuePairs containing spatial items and their associated data found within the rectangle.</returns>
    /// <remarks>
    /// This method returns all items that intersect with the provided rectangle, including items on the boundary of the rectangle. 
    /// It uses a breadth-first search approach to traverse the QuadTree nodes that intersect with the search rectangle.
    /// </remarks>
    public List<QuadTreeObject<K, V>> FindRectangle(Rectangle rectangle)
    {
        List<QuadTreeObject<K, V>>? foundItems = new List<QuadTreeObject<K, V>>();
        Queue<QuadTreeNode<K, V>> nodesToCheck = new Queue<QuadTreeNode<K, V>>();
        nodesToCheck.Enqueue(Root);

        while (nodesToCheck.Count > 0)
        {
            QuadTreeNode<K,V> currentNode = nodesToCheck.Dequeue();

            if (currentNode.Data.Count != 0)
            {

                foreach (var kvp in currentNode.Data)
                {
                    if (kvp.Item is Rectangle && rectangle.Contains((Rectangle)kvp.Item))
                    {
                        foundItems.Add(kvp);
                    } else if (kvp.Item is Point && rectangle.ContainsPoint((Point)kvp.Item))
                    {
                        foundItems.Add(kvp);
                    }
                }
            }

            if (currentNode.Children == null)
                continue;

            bool containsRectangle = false;

            foreach (Quadrant quadrant in Enum.GetValues(typeof(Quadrant)))
            {
                if (rectangle.Contains(currentNode.Children[(int)quadrant].Boundary) 
                    ||
                    currentNode.Children[(int)quadrant].Boundary.Contains(rectangle))
                {
                    nodesToCheck.Enqueue(currentNode.Children[(int)quadrant]);
                    containsRectangle = true;
                }
            }

            if (!containsRectangle)
            {
                continue;
            }
        }
        return foundItems;
    }

    //delete point
    public bool Delete(QuadTreeObject<K, V> quadTreeObject)
    {
        if (quadTreeObject.Item is Point)
        {
            DeletePoint(quadTreeObject);
            return true;
        }
        else if (quadTreeObject.Item is Rectangle)
        {
            //return DeleteRectangle((Rectangle)quadTreeObject.Item);
            return false;
        }
        else
        {
            throw new ArgumentException("Item in object must be either a Point or a Rectangle");
        }
    }

    public SpatialItem DeletePoint(QuadTreeObject<K, V> quadTreeObject)
    {
        QuadTreeNode<K, V>? foundNode = null;
        QuadTreeNode<K, V> currentNode = Root;

        while (true)
        {

            if (currentNode.Data.Count != 0)
            {

                foreach (var kvp in currentNode.Data)
                {
                    if (kvp.Key.Equals(quadTreeObject.Key))
                    {
                        foundNode = currentNode;
                        currentNode.Data.Remove(kvp); //Delete the point
                        break;

                    }
                }

                if (foundNode is not null)
                    break;

            }

            if (currentNode.Children == null)
                break;

            Quadrant? choosenQuadrant = null;

            //FIXME: What if it's rectangle?
            if (quadTreeObject.Item is Rectangle)
            {

                foreach (Quadrant quadrant in Enum.GetValues(typeof(Quadrant)))
                {
                    if (currentNode.Children[(int)quadrant].Boundary.Contains((Rectangle)quadTreeObject.Item))
                    {
                        choosenQuadrant = quadrant;
                        break;
                    }
                }
            }
            
            if(quadTreeObject.Item is Point)
                choosenQuadrant = currentNode.DetermineQuadrant((Point)quadTreeObject.Item);

            //If the point is not in any quadrant, return null
            if (choosenQuadrant == null)
            {
                break;
            }

            currentNode = currentNode.Children[(int)choosenQuadrant];
        }

        if (foundNode is null)
            return null;

        //If the node is empty, delete it
        //if (foundNode.isLeaf() && foundNode.Data.Count == 0)
        //{
            currentNode = foundNode.Parent;
        //}

        while (currentNode is not null) //Neviem ci dobra podmienka
        {

            int sumEmptyQuadrants = 0;
            foreach (var child in currentNode.Children)
            {
                if (child.Data.Count == 0)
                    sumEmptyQuadrants++;
            }

            //Ak som prazdny a list
            if (sumEmptyQuadrants == 4)
            {
                currentNode.NullChildren();
                currentNode = currentNode.Parent;
            } else
            {
                break;
            }

            
        }
        return (SpatialItem)quadTreeObject.Item;
    
    }
}