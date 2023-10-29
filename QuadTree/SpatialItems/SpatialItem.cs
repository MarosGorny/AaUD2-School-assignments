namespace QuadTreeDS.SpatialItems;

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
    public Point LowerLeft { get; protected set; }
    public Point UpperRight { get; protected set; }

    public bool ContainsStrict(SpatialItem other)
    {
        return LowerLeft.X <= other.LowerLeft.X &&
               LowerLeft.Y <= other.LowerLeft.Y &&
               UpperRight.X >= other.UpperRight.X &&
               UpperRight.Y >= other.UpperRight.Y;
    }

    public bool Intersects(SpatialItem other)
    {
        return LowerLeft.X <= other.UpperRight.X &&
               UpperRight.X >= other.LowerLeft.X &&
               LowerLeft.Y <= other.UpperRight.Y &&
               UpperRight.Y >= other.LowerLeft.Y;
    }
}

/// <summary>
/// Represents a 2D point.
/// </summary>
public class Point : SpatialItem
{
    public double X { get; set; }
    public double Y { get; set; }

    //public override SpatialItem Boundary => this;

    /// <summary>
    /// Initializes a new instance of the <see cref="Point"/> class.
    /// </summary>
    /// <param name="x">X coordinate of the point.</param>
    /// <param name="y">Y coordinate of the point.</param>
    public Point(double x, double y)
    {
        X = x;
        Y = y;
        LowerLeft = this;
        UpperRight = this;

        //Boundary = new Rectangle(this, this);
    }

    //public override bool ContainsStrict(SpatialItem spatialItem)
    //{
    //    if(spatialItem is not Point p)
    //    {
    //        return false;
    //    }

    //    return p.X == X && p.Y == Y;
    //}

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
    //public override SpatialItem Boundary {get; }

    public Rectangle(Point bottomLeft, Point topRight)
    {
        LowerLeft = bottomLeft;
        UpperRight = topRight;

        //Boundary = this;
    }

    //public override bool ContainsStrict(Point p)
    //{
    //    return (p.X >= BottomLeft.X && p.X <= TopRight.X &&
    //            p.Y >= BottomLeft.Y && p.Y <= TopRight.Y);
    //}

    //public bool Contains(Rectangle targetRectangle)
    //{
    //    Check if the X - coordinates of the target rectangle are within the current rectangle's X-coordinates
    //    bool xCoordinatesContained = BottomLeft.X <= targetRectangle.BottomLeft.X &&
    //                                 TopRight.X >= targetRectangle.TopRight.X;

    //    Check if the Y - coordinates of the target rectangle are within the current rectangle's Y-coordinates
    //    bool yCoordinatesContained = BottomLeft.Y <= targetRectangle.BottomLeft.Y &&
    //                                 TopRight.Y >= targetRectangle.TopRight.Y;

    //    The target rectangle is completely contained if both its X and Y coordinates are within the current rectangle's coordinates
    //    return xCoordinatesContained && yCoordinatesContained;
    //}

    #region Overrides
    public static bool operator ==(Rectangle left, Rectangle right)
    {
        return left.LowerLeft == right.LowerLeft && left.UpperRight == right.UpperRight;
    }

    public static bool operator !=(Rectangle left, Rectangle right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Rectangle other)
        {
            return LowerLeft.Equals(other.LowerLeft) && UpperRight.Equals(other.UpperRight);
        }
        return false;
    }
    #endregion

}

public enum LatitudeDirection
{
    N = 1,
    S = -1
}

public enum LongitudeDirection
{
    E = 1,
    W = -1
}

public class GPSPoint : Point
{
    public LatitudeDirection LatitudeDirection { get; set; }
    public LongitudeDirection LongitudeDirection { get; set; }

    public GPSPoint(LatitudeDirection latDir, double latVal, LongitudeDirection longDir, double longVal)
        : base(latVal, longVal)
    {
        LatitudeDirection = latDir;
        LongitudeDirection = longDir;

        // Compute the relative coordinates 
        X = latVal * (double)latDir;
        Y = longVal * (double)longDir;
    }
}

public class GPSRectangle : Rectangle
{
    //public GPSPoint BottomLeft { get; set; }
    //public GPSPoint TopRight { get; set; }

    //public override SpatialItem Boundary { get;}

    public GPSRectangle(GPSPoint bottomLeft, GPSPoint topRight)
        : base(bottomLeft, topRight)
    {
        //BottomLeft = bottomLeft;
        //TopRight = topRight;
    }

    //public override bool ContainsStrict(Point p)
    //{
    //    if (p is not GPSPoint gpsP)
    //    {
    //        return false;
    //    }

    //    bool latitudeContained = BottomLeft.Y <= gpsP.Y && TopRight.Y >= gpsP.Y;
    //    bool longitudeContained = BottomLeft.X <= gpsP.X && TopRight.X >= gpsP.X;

    //    return latitudeContained && longitudeContained;
    //}

    //public bool ContainsStrict(GPSRectangle targetRectangle)
    //{
    //    bool xCoordinatesContained = BottomLeft.X <= targetRectangle.BottomLeft.X && TopRight.X >= targetRectangle.TopRight.X;
    //    bool yCoordinatesContained = BottomLeft.Y <= targetRectangle.BottomLeft.Y && TopRight.Y >= targetRectangle.TopRight.Y;

    //    return xCoordinatesContained && yCoordinatesContained;
    //}

    //public bool Intersects(GPSRectangle targetRectangle)
    //{
    //    bool xOverlap = BottomLeft.X <= targetRectangle.TopRight.X && TopRight.X >= targetRectangle.BottomLeft.X;
    //    bool yOverlap = BottomLeft.Y <= targetRectangle.TopRight.Y && TopRight.Y >= targetRectangle.BottomLeft.Y;

    //    return xOverlap && yOverlap;
    //}
}