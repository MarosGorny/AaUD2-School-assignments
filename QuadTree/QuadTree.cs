using QuadTree.SpatialItems;

namespace QuadTree;


public class QuadTreeNode<K,V> where K : IComparable<K>
{
    /// <summary>
    /// The area represented by this node.
    /// </summary>
    public Rectangle Boundary { get; private set; }

    /// <summary>
    /// List of data stored at this node. Each entry is a KeyValuePair where the key is 
    /// an SpatialItem (either a Point or a Rectangle) and the value is of type T.
    /// </summary>
    //public List<KeyValuePair<SpatialItem,T>> Data { get; private set; }
    public List<QuadTreeObject<K,V>> Data { get; private set; }

    /// <summary>
    /// Array of child nodes for the current node.
    /// </summary>
    /// <remarks>
    /// Each node can have up to four children, representing the four quadrants: NorthWest, NorthEast, SouthWest, and SouthEast.
    /// This array will be null if the node hasn't been subdivided.
    /// </remarks>
    public QuadTreeNode<K,V>[] Children { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuadTreeNode{T}"/> class.
    /// </summary>
    /// <param name="boundary">The spatial boundary represented by this node.</param>
    public QuadTreeNode(Rectangle boundary)
    {
        this.Boundary = boundary;
        this.Data = new List<QuadTreeObject<K, V>>();
    }

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

    /// <summary>
    /// Determines in which quadrant a given point lies.
    /// </summary>
    /// <param name="point">The point for which the quadrant is to be determined.</param>
    /// <returns>The quadrant in which the point lies.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the point does not fit into any known quadrant.</exception>
    public Quadrant? DetermineQuadrant(Point point)
    {
        double midX = (Boundary.BottomLeft.X + Boundary.TopRight.X) / 2.0;
        double midY = (Boundary.BottomLeft.Y + Boundary.TopRight.Y) / 2.0;

        //Random rand = new Random();
        //if (midX == point.X)
        //    midX += (rand.NextDouble() - 0.5) * midX;
        //if (midY == point.Y)
        //    midY += (rand.NextDouble() - 0.5) * midY;

        if (point.X < midX && point.Y >= midY)
            return Quadrant.NorthWest;
        else if (point.X >= midX && point.Y >= midY)
            return Quadrant.NorthEast;
        else if (point.X < midX && point.Y < midY)
            return Quadrant.SouthWest;
        else if (point.X >= midX && point.Y < midY)
            return Quadrant.SouthEast;

        //TODO: CHANGE TO RETURN NULL
        return null;
        throw new ArgumentOutOfRangeException(nameof(point));
    }

    /// <summary>
    /// Subdivides the current node into four child nodes representing the four quadrants.
    /// </summary>
    /// <remarks>
    /// If the node has already been subdivided, this method does nothing.
    /// </remarks>
    public void Subdivide()
    {
        if (Children == null)
        {
            Children = new QuadTreeNode<K,V>[4];

            double midX = (Boundary.BottomLeft.X + Boundary.TopRight.X) / 2.0;
            double midY = (Boundary.BottomLeft.Y + Boundary.TopRight.Y) / 2.0;

            Children[(int)Quadrant.NorthWest] = new QuadTreeNode<K, V>(new Rectangle(new Point(Boundary.BottomLeft.X, midY), new Point(midX, Boundary.TopRight.Y)));
            Children[(int)Quadrant.NorthEast] = new QuadTreeNode<K, V>(new Rectangle(new Point(midX, midY), Boundary.TopRight));
            Children[(int)Quadrant.SouthWest] = new QuadTreeNode<K, V>(new Rectangle(Boundary.BottomLeft, new Point(midX, midY)));
            Children[(int)Quadrant.SouthEast] = new QuadTreeNode<K, V>(new Rectangle(new Point(midX, Boundary.BottomLeft.Y), new Point(Boundary.TopRight.X, midY)));
        }
    }

    public List<Rectangle>? CalculateSubquadrantsBoundaries()
    {
        double midX = (Boundary.BottomLeft.X + Boundary.TopRight.X) / 2.0;
        double midY = (Boundary.BottomLeft.Y + Boundary.TopRight.Y) / 2.0;

        List<Rectangle> subquadrants = new List<Rectangle>();
        subquadrants.Add(new Rectangle(new Point(Boundary.BottomLeft.X, midY), new Point(midX, Boundary.TopRight.Y)));
        subquadrants.Add(new Rectangle(new Point(midX, midY), Boundary.TopRight));
        subquadrants.Add(new Rectangle(Boundary.BottomLeft, new Point(midX, midY)));
        subquadrants.Add(new Rectangle(new Point(midX, Boundary.BottomLeft.Y), new Point(Boundary.TopRight.X, midY)));
        return subquadrants;
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
        Root = new QuadTreeNode<K, V>(boundary);
        Root.Subdivide();
    }

    /// <summary>
    /// Adds a spatial item to the current node or subdivides the node if needed.
    /// </summary>
    /// <param name="currentNode">The current node being examined.</param>
    /// <param name="item">The spatial item to be added.</param>
    /// <param name="value">The associated data for the spatial item.</param>
    /// <returns>A KeyValuePair if the item is added, null if the node is subdivided.</returns>

    private bool AddToData(QuadTreeNode<K,V> currentNode, QuadTreeObject<K,V> quadTreeObject) 
    {
        if (currentNode.Data.Count > 0 && currentNode.Data[0].Item.Equals(quadTreeObject.Item))
        {
            foreach (var kvp in currentNode.Data)
            {
                if (kvp.Key.Equals(quadTreeObject.Key))
                {
                    throw new ArgumentException(String.Format("Key {0} already exists in QuadTree", kvp.Key.ToString()));
                }
            }
            currentNode.Data.Add(quadTreeObject);
            return true;
        } else if (currentNode.Data.Count == 0)
        {
            currentNode.Data.Add(quadTreeObject);
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
        QuadTreeNode<K,V> currentNode = Root;

        while (true)
        {
            if (!currentNode.Boundary.ContainsPoint((Point)quadTreeObject.Item))
                return;

            if (currentNode.Children == null)
            {
                if (currentNode.Data.Count != 0)
                {
                    if(AddToData(currentNode, quadTreeObject))
                    {
                        return;
                    }
                        currentNode.Subdivide();
                }
                else
                {
                    AddToData(currentNode, quadTreeObject);
                    return;
                }
            }

            Quadrant? targetQuadrant = currentNode.DetermineQuadrant((Point)quadTreeObject.Item);
            currentNode = currentNode.Children[(int)targetQuadrant];
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
            if (!currentNode.Boundary.ContainsRectangle(rectangle))
                return;

            if (currentNode.Children == null)
            {
                if (currentNode.Data.Count != 0)
                {

                    bool fitsInAQuadrant1 = false;
                    foreach(Rectangle boundary in currentNode.CalculateSubquadrantsBoundaries() )
                    {
                        if(boundary.ContainsRectangle(rectangle))
                        {
                            fitsInAQuadrant1 = true;
                            currentNode.Subdivide();
                            break;
                        }
                    }

                    if(!fitsInAQuadrant1)
                    {
                        AddToData(currentNode, quadTreeObject);
                    }

                    return;
                }
                else
                {
                    AddToData(currentNode, quadTreeObject);
                    return;
                }
            }

            bool fitsInAQuadrant = false;

            foreach (Quadrant quadrant in Enum.GetValues(typeof(Quadrant)))
            {
                if (currentNode.Children[(int)quadrant].Boundary.ContainsRectangle(rectangle))
                {                   
                    currentNode = currentNode.Children[(int)quadrant];
                    fitsInAQuadrant = true;
                    break;
                }
            }

            if (!fitsInAQuadrant)
            {
                AddToData(currentNode, quadTreeObject);
                return;
            }
        }
    }

    private bool FitsInAQuadrant(QuadTreeNode<K,V> currentNode, Rectangle rectangle, out Quadrant? returnedQuadrant)
    {
        foreach (Quadrant quadrant in Enum.GetValues(typeof(Quadrant)))
        {
            if (currentNode.Children[(int)quadrant].Boundary.ContainsRectangle(rectangle))
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
                    if (kvp.Item is Rectangle && rectangle.ContainsRectangle((Rectangle)kvp.Item))
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
                if (rectangle.ContainsRectangle(currentNode.Children[(int)quadrant].Boundary) 
                    ||
                    currentNode.Children[(int)quadrant].Boundary.ContainsRectangle(rectangle))
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
}
