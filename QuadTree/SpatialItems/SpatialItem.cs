namespace QuadTree.SpatialItems;

/// <summary>
/// Enum representing the four possible quadrants in a 2D space.
/// </summary>
public enum Quadrant
{
    NorthWest = 0,
    NorthEast = 1,
    SouthWest = 2,
    SouthEast = 3
}


/// <summary>
/// Abstract class representing an item that exists within a spatial context.
/// </summary>
public abstract class SpatialItem
{
    /// <summary>
    /// Determines if the spatial item contains the given point.
    /// </summary>
    /// <param name="p">Point to be checked.</param>
    /// <returns>True if the point is contained within the item, otherwise false.</returns>
    public abstract bool ContainsPoint(Point p);
}

/// <summary>
/// Represents a 2D point.
/// </summary>
public class Point : SpatialItem
{
    public double X { get; set; }
    public double Y { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Point"/> class.
    /// </summary>
    /// <param name="x">X coordinate of the point.</param>
    /// <param name="y">Y coordinate of the point.</param>
    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    public override bool ContainsPoint(Point p)
    {
        return p.X == X && p.Y == Y;
    }

    #region Overrides
    public static bool operator ==(Point left, Point right)
    {
        return left.X == right.X && left.Y == right.Y;
    }

    public static bool operator !=(Point left, Point right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Point other)
        {
            return X == other.X && Y == other.Y;
        }
        return false;
    }
    #endregion
}

/// <summary>
/// Represents a 2D rectangle.
/// </summary>
public class Rectangle : SpatialItem
{
    public Point BottomLeft { get; set; }
    public Point TopRight { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> class.
    /// </summary>
    /// <param name="bottomLeft">The bottom left corner of the rectangle.</param>
    /// <param name="topRight">The top right corner of the rectangle.</param>
    public Rectangle(Point bottomLeft, Point topRight)
    {
        BottomLeft = bottomLeft;
        TopRight = topRight;
    }

    public override bool ContainsPoint(Point p)
    {
        return (p.X >= BottomLeft.X && p.X <= TopRight.X &&
                p.Y >= BottomLeft.Y && p.Y <= TopRight.Y);
    }

    //public bool IntersectsRectangle(Rectangle other)
    //{
    //    return (BottomLeft.X <= other.TopRight.X && TopRight.X >= other.BottomLeft.X &&
    //            BottomLeft.Y <= other.TopRight.Y && TopRight.Y >= other.BottomLeft.Y);
    //}

    public bool ContainsRectangle(Rectangle other)
    {
        return (BottomLeft.X <= other.BottomLeft.X && TopRight.X >= other.TopRight.X &&
                BottomLeft.Y <= other.BottomLeft.Y && TopRight.Y >= other.TopRight.Y);
    }

    #region Overrides
    public static bool operator ==(Rectangle left, Rectangle right)
    {
        return left.BottomLeft == right.BottomLeft && left.TopRight == right.TopRight;
    }

    public static bool operator !=(Rectangle left, Rectangle right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Rectangle other)
        {
            return BottomLeft.Equals(other.BottomLeft) && TopRight.Equals(other.TopRight);
        }
        return false;
    }
    #endregion
}