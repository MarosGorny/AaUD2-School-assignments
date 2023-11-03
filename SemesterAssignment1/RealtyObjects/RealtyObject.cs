using QuadTreeDS.SpatialItems;

namespace SemesterAssignment1.RealtyObjects;

public abstract class RealtyObject : SpatialItem
{
    public string Description { get; set; }

    public RealtyObject(string description)
    {
        Description = description;
    }
}

/// <summary>
/// Represents a real estate property.
/// </summary>
public class Property : RealtyObject
{
    public int ConscriptionNumber { get; set; }
    public List<Parcel> PositionedOnParcels { get; set; } = new List<Parcel>(); 

    public Property(int conscriptionNumber, string description, GPSRectangle gpsRectangle)
        : base(description)
    {
        ConscriptionNumber = conscriptionNumber;
        LowerLeft = gpsRectangle.LowerLeft;
        UpperRight = gpsRectangle.UpperRight;
    }

    public void AddParcel(Parcel parcel)
    {
        PositionedOnParcels.Add(parcel);
    }
}

/// <summary>
/// Represents a parcel of land.
/// </summary>
public class Parcel : RealtyObject
{
    public int ParcelNumber { get; set; }
    public List<Property> OccupiedByProperties { get; set; } = new List<Property>();
    public GPSRectangle Bounds { get; set; }

    public Parcel(int parcelNumber, string description, GPSRectangle gpsRectangle)
        :base(description)
    {
        ParcelNumber = parcelNumber;
        LowerLeft = gpsRectangle.LowerLeft;
        UpperRight = gpsRectangle.UpperRight;
        Bounds = gpsRectangle;
    }

    public void AddProperty(Property property)
    {
        OccupiedByProperties.Add(property);
    }
}