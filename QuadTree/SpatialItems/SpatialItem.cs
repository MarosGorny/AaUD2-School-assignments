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

    public double GetLongestSide()
    {
        double width = UpperRight.X - LowerLeft.X;
        double height = UpperRight.Y - LowerLeft.Y;
        return System.Math.Max(width, height);
    }

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

    public bool OverlapsBorder(SpatialItem other)
    {
        // Check if the two items intersect in general.
        bool generalIntersection = LowerLeft.X <= other.UpperRight.X &&
                                   UpperRight.X >= other.LowerLeft.X &&
                                   LowerLeft.Y <= other.UpperRight.Y &&
                                   UpperRight.Y >= other.LowerLeft.Y;

        if (generalIntersection)
        {
            // Check if one item is fully contained within the other.
            bool thisContainsOther = ContainsStrict(other);
            bool otherContainsThis = other.ContainsStrict(this);

            // Return true only if neither item fully contains the other (indicating a border overlap).
            return !(thisContainsOther || otherContainsThis);
        }
        return false;
    }
}

/// <summary>
/// Represents a 2D point.
/// </summary>
public class Point : SpatialItem
{
    public double X { get; set; }
    public double Y { get; set; }

    public Point(double x, double y)
    {
        X = x;
        Y = y;
        LowerLeft = this;
        UpperRight = this;
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

    public Rectangle(Point bottomLeft, Point topRight)
    {
        LowerLeft = bottomLeft;
        UpperRight = topRight;
    }
    #region Overrides
    public override double GetLongestSide()
    {
        double width = UpperRight.X - LowerLeft.X;
        double height = UpperRight.Y - LowerLeft.Y;
        return System.Math.Max(width, height);
    }
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

    public GPSRectangle(GPSPoint bottomLeft, GPSPoint topRight)
        : base(bottomLeft, topRight)
    {

    }

}