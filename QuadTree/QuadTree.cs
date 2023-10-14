using QuadTree.SpatialItems;

namespace QuadTree;


public class QuadTreeNode<T> //where T : IComparable
{
    /// <summary>
    /// The area represented by this node.
    /// </summary>
    public Rectangle Boundary { get; private set; }

    /// <summary>
    /// List of data stored at this node. Each entry is a KeyValuePair where the key is 
    /// an SpatialItem (either a Point or a Rectangle) and the value is of type T.
    /// </summary>
    public List<KeyValuePair<SpatialItem,T>> Data { get; private set; }

    /// <summary>
    /// Array of child nodes for the current node.
    /// </summary>
    /// <remarks>
    /// Each node can have up to four children, representing the four quadrants: NorthWest, NorthEast, SouthWest, and SouthEast.
    /// This array will be null if the node hasn't been subdivided.
    /// </remarks>
    public QuadTreeNode<T>[] Children { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuadTreeNode{T}"/> class.
    /// </summary>
    /// <param name="boundary">The spatial boundary represented by this node.</param>
    public QuadTreeNode(Rectangle boundary)
    {
        this.Boundary = boundary;
        this.Data = new List<KeyValuePair<SpatialItem, T>>();
    }

    /// <summary>
    /// Determines in which quadrant a given point lies.
    /// </summary>
    /// <param name="point">The point for which the quadrant is to be determined.</param>
    /// <returns>The quadrant in which the point lies.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the point does not fit into any known quadrant.</exception>

    public Quadrant DetermineQuadrant(Point point)
    {
        double midX = (Boundary.BottomLeft.X + Boundary.TopRight.X) / 2.0;
        double midY = (Boundary.BottomLeft.Y + Boundary.TopRight.Y) / 2.0;

        Random rand = new Random();
        if (midX == point.X)
            midX += (rand.NextDouble() - 0.5) * midX;
        if (midY == point.Y)
            midY += (rand.NextDouble() - 0.5) * midY;

        if (point.X < midX && point.Y >= midY)
            return Quadrant.NorthWest;
        else if (point.X >= midX && point.Y >= midY)
            return Quadrant.NorthEast;
        else if (point.X < midX && point.Y < midY)
            return Quadrant.SouthWest;
        else if (point.X >= midX && point.Y < midY)
            return Quadrant.SouthEast;

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
            Children = new QuadTreeNode<T>[4];

            double midX = (Boundary.BottomLeft.X + Boundary.TopRight.X) / 2.0;
            double midY = (Boundary.BottomLeft.Y + Boundary.TopRight.Y) / 2.0;

            Children[(int)Quadrant.NorthWest] = new QuadTreeNode<T>(new Rectangle(new Point(Boundary.BottomLeft.X, midY), new Point(midX, Boundary.TopRight.Y)));
            Children[(int)Quadrant.NorthEast] = new QuadTreeNode<T>(new Rectangle(new Point(midX, midY), Boundary.TopRight));
            Children[(int)Quadrant.SouthWest] = new QuadTreeNode<T>(new Rectangle(Boundary.BottomLeft, new Point(midX, midY)));
            Children[(int)Quadrant.SouthEast] = new QuadTreeNode<T>(new Rectangle(new Point(midX, Boundary.BottomLeft.Y), new Point(Boundary.TopRight.X, midY)));
        }
    }
}



/// <summary>
/// Represents a QuadTree data structure for spatial partitioning.
/// </summary>
/// <typeparam name="T">The type of data associated with spatial items in the quadtree.</typeparam>
public class QuadTree<T> // where T : IComparable
{
    /// <summary>
    /// Gets the root node of the QuadTree.
    /// </summary>
    public QuadTreeNode<T> Root { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuadTree{T}"/> class and subdivides the root node.
    /// </summary>
    /// <param name="boundary">The boundary rectangle that defines the spatial limits of the quadtree.</param>
    public QuadTree(Rectangle boundary)
    {
        Root = new QuadTreeNode<T>(boundary);
        Root.Subdivide();
    }

    /// <summary>
    /// Adds a spatial item to the current node or subdivides the node if needed.
    /// </summary>
    /// <param name="currentNode">The current node being examined.</param>
    /// <param name="item">The spatial item to be added.</param>
    /// <param name="value">The associated data for the spatial item.</param>
    /// <returns>A KeyValuePair if the item is added, null if the node is subdivided.</returns>
    private KeyValuePair<SpatialItem, T>? AddOrSubdivide(QuadTreeNode<T> currentNode, SpatialItem item, T value)
    {
        if (currentNode.Data[0].Key == item)
        {
            KeyValuePair<SpatialItem, T> kvp = KeyValuePair.Create(item, value);
            return kvp;
        }

        currentNode.Subdivide();
        return null;        
    }

    /// <summary>
    /// Inserts a point and its associated data into the quadtree.
    /// </summary>
    /// <param name="point">The point to be inserted.</param>
    /// <param name="value">The data associated with the point.</param>
    public void Insert(Point point, T value)
    {
        QuadTreeNode<T> currentNode = Root;

        while (true)
        {
            if (!currentNode.Boundary.ContainsPoint(point))
                return;

            if (currentNode.Children == null)
            {
                if (currentNode.Data.Count != 0)
                {
                    if(AddOrSubdivide(currentNode, point, value) != null)
                        return;
                }
                else
                {
                    KeyValuePair<SpatialItem, T> kvp = KeyValuePair.Create((SpatialItem)point, value);
                    currentNode.Data.Add(kvp);
                    return;
                }
            }

            Quadrant targetQuadrant = currentNode.DetermineQuadrant(point);
            currentNode = currentNode.Children[(int)targetQuadrant];
        }
    }

    /// <summary>
    /// Inserts a rectangle and its associated data into the quadtree.
    /// </summary>
    /// <param name="rectangle">The rectangle to be inserted.</param>
    /// <param name="value">The data associated with the rectangle.</param>
    public void Insert(Rectangle rectangle, T value)
    {
        QuadTreeNode<T> currentNode = Root;

        while (true)
        {
            if (!currentNode.Boundary.ContainsRectangle(rectangle))
                return;

            if (currentNode.Children == null)
            {
                if (currentNode.Data.Count != 0)
                {
                    if (AddOrSubdivide(currentNode, rectangle, value) != null)
                        return;
                }
                else
                {
                    KeyValuePair<SpatialItem, T> kvp = KeyValuePair.Create((SpatialItem)rectangle, value);
                    currentNode.Data.Add(kvp);
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
                KeyValuePair<SpatialItem, T> kvp = KeyValuePair.Create((SpatialItem)rectangle, value);
                currentNode.Data.Add(kvp);
                return;
            }
        }
    }
}
