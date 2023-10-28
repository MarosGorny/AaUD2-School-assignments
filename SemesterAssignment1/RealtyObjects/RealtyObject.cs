using QuadTree.SpatialItems;

namespace SemesterAssignment1.RealtyObjects;


/// <summary>
/// Represents a real estate property.
/// </summary>
public class Property : SpatialItem
{
    public int ConscriptionNumber { get; set; }
    public string Description { get; set; }
    public List<Parcel> PositionedOnParcels { get; set; } = new List<Parcel>(); 
    public GPSRectangle Boundary { get; private set; }

    public Property(int conscriptionNumber, string description, GPSPoint bottomLeft, GPSPoint topRight)
    {
        ConscriptionNumber = conscriptionNumber;
        Description = description;
        Boundary = new GPSRectangle(bottomLeft, topRight);
    }

    public override bool ContainsPoint(Point p)
    {
        return Boundary.ContainsPoint(p);
    }
}

/// <summary>
/// Represents a parcel of land.
/// </summary>
public class Parcel : SpatialItem
{
    public int ParcelNumber { get; set; }
    public string Description { get; set; }
    public List<Property> OccupiedByProperties { get; set; } = new List<Property>(); 

    private GPSRectangle Boundary;

    public Parcel(int parcelNumber, string description, GPSPoint bottomLeft, GPSPoint topRight)
    {
        ParcelNumber = parcelNumber;
        Description = description;
        Boundary = new GPSRectangle(bottomLeft, topRight);
    }

    public override bool ContainsPoint(Point p)
    {
        return Boundary.ContainsPoint(p);
    }
}