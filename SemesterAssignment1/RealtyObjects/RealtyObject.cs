using QuadTreeDS.SpatialItems;

namespace SemesterAssignment1.RealtyObjects;


/// <summary>
/// Represents a real estate property.
/// </summary>
public class Property : SpatialItem
{
    public int ConscriptionNumber { get; set; }
    public string Description { get; set; }
    public List<Parcel> PositionedOnParcels { get; set; } = new List<Parcel>(); 
    //public override SpatialItem Boundary {get; }

    public Property(int conscriptionNumber, string description, GPSRectangle gpsRectangle)
    {
        ConscriptionNumber = conscriptionNumber;
        Description = description;
        //Boundary = gpsRectangle.Boundary;
    }

    //public override bool ContainsPoint(Point p)
    //{
    //    return Boundary.ContainsPoint(p);
    //}
}

/// <summary>
/// Represents a parcel of land.
/// </summary>
public class Parcel : SpatialItem
{
    public int ParcelNumber { get; set; }
    public string Description { get; set; }
    public List<Property> OccupiedByProperties { get; set; } = new List<Property>();
    //public override SpatialItem Boundary { get; }

    public Parcel(int parcelNumber, string description, GPSRectangle gpsRectangle)
    {
        ParcelNumber = parcelNumber;
        Description = description;
        //Boundary = gpsRectangle.Boundary;
    }

    //public override bool ContainsPoint(Point p)
    //{
    //    return Boundary.ContainsPoint(p);
    //}
}