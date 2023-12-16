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
/// Interface representing an item that exists within a spatial context.
/// </summary>
public interface ISpatialItem
{
    Point LowerLeft { get; }
    Point UpperRight { get; }

    double GetLongestSide();
    double GetHeight();
    double GetWidth();
    bool ContainsStrict(ISpatialItem other);

    bool Intersects(ISpatialItem other);

    bool OverlapsBorder(ISpatialItem other);

    //public double GetLongestSide()
    //{
    //    double width = GetWidth();
    //    double height = GetHeight();
    //    return System.Math.Max(width, height);
    //}

    //public double GetHeight()
    //{
    //    return UpperRight.Y - LowerLeft.Y;
    //}

    //public double GetWidth()
    //{
    //    return UpperRight.X - LowerLeft.X;
    //}
}

/// <summary>
/// Represents a 2D point.
/// </summary>
public class Point : ISpatialItem
{
    public double X { get; set; }
    public double Y { get; set; }

    public Point LowerLeft => this;
    public Point UpperRight => this;

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    #region Interface Implementation
    #endregion
    public double GetLongestSide()
    {
        return 0.0;
    }

    public double GetHeight()
    {
        return 0.0;
    }

    public double GetWidth()
    {
        return 0.0;
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

    public bool ContainsStrict(ISpatialItem other)
    {
        return SpatialItemExtensions.ContainsStrict(this, other);
    }

    public bool Intersects(ISpatialItem other)
    {
        return SpatialItemExtensions.Intersects(this, other);
    }

    public bool OverlapsBorder(ISpatialItem other)
    {
        return SpatialItemExtensions.OverlapsBorder(this, other);
    }
    #endregion

}

/// <summary>
/// Represents a 2D rectangle.
/// </summary>
public class Rectangle : ISpatialItem
{
    public Point LowerLeft { get; set; }
    public Point UpperRight { get; set; }

    public Rectangle(Point bottomLeft, Point topRight)
    {
        LowerLeft = bottomLeft;
        UpperRight = topRight;
    }

    public void SetLowerLeft(Point newLowerLeft)
    {
        LowerLeft = newLowerLeft;
    }

    public void SetUpperRight(Point newUpperRight)
    {
        UpperRight = newUpperRight;
    }


    public double GetLongestSide()
    {
        double width = GetWidth();
        double height = GetHeight();
        return System.Math.Max(width, height);
    }

    public double GetHeight()
    {
        return UpperRight.Y - LowerLeft.Y;
    }

    public double GetWidth()
    {
        return UpperRight.X - LowerLeft.X;
    }

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

    public bool ContainsStrict(ISpatialItem other)
    {
        return SpatialItemExtensions.ContainsStrict(this, other);
    }

    public bool Intersects(ISpatialItem other)
    {
        return SpatialItemExtensions.Intersects(this, other);
    }

    public bool OverlapsBorder(ISpatialItem other)
    {
        return SpatialItemExtensions.OverlapsBorder(this, other);
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

    public GPSPoint()
    : base(0.0, 0.0) // Default values for X and Y
    {

        LatitudeDirection = LatitudeDirection.N; 
        LongitudeDirection = LongitudeDirection.E; 
        X = 0.0 * (double)LatitudeDirection;
        Y = 0.0 * (double)LongitudeDirection;
    }

    public GPSPoint(LatitudeDirection latDir, double latVal, LongitudeDirection longDir, double longVal)
        : base(latVal, longVal)
    {
        LatitudeDirection = latDir;
        LongitudeDirection = longDir;

        // Compute the relative coordinates 
        X = latVal * (double)latDir;
        Y = longVal * (double)longDir;
    }

    public void Serialize(BinaryWriter writer)
    {
        writer.Write((int)LatitudeDirection);
        writer.Write((int)LongitudeDirection);
        writer.Write(X);
        writer.Write(Y);
    }

    public static GPSPoint Deserialize(BinaryReader reader)
    {
        var latDir = (LatitudeDirection)reader.ReadInt32();
        var longDir = (LongitudeDirection)reader.ReadInt32();
        var x = reader.ReadDouble();
        var y = reader.ReadDouble();

        if (latDir == LatitudeDirection.S)
        {
            x *= -1;
        }

        if (longDir == LongitudeDirection.W)
        {
            y *= -1;
        }
        return new GPSPoint(latDir, x, longDir, y);
    }

    public override string ToString()
    {
        string xString = X.ToString("F13").TrimEnd('0');
        xString = xString.TrimEnd(',');

        string yString = Y.ToString("F13").TrimEnd('0');
        yString = yString.TrimEnd(',');

        return $"{LatitudeDirection} {xString}, {LongitudeDirection} {yString}";
    }

    public int GetSize()
    {
        int size = 0;

        size += sizeof(int); // Size for LatitudeDirection
        size += sizeof(int); // Size for LongitudeDirection
        size += sizeof(double); // Size for X
        size += sizeof(double); // Size for Y

        //size += sizeof(int); // Size for LatitudeDirection
        //size += sizeof(int); // Size for LongitudeDirection
        //size += sizeof(double); // Size for X
        //size += sizeof(double); // Size for Y

        return size;
    }
}

public class GPSRectangle : Rectangle
{

    public GPSRectangle(GPSPoint bottomLeft, GPSPoint topRight)
        : base(bottomLeft, topRight)
    {
    }
}