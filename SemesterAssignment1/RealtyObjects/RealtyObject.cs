using DynamicHashingDS.Data;
using QuadTreeDS.SpatialItems;
using System.Collections;

namespace SemesterAssignment1.RealtyObjects;

public abstract class RealtyObject : Rectangle
{
    public string Description { get; set; }

    public RealtyObject(string description, GPSRectangle gpsRectangle)
        :base(gpsRectangle.LowerLeft as GPSPoint, gpsRectangle.UpperRight as GPSPoint)
    {
        Description = description;
    }
}

/// <summary>
/// Represents a real estate property.
/// </summary>
public class Property : RealtyObject, IDHRecord<Property>
{
    public int ConscriptionNumber { get; set; }
    public List<Parcel> PositionedOnParcels { get; set; } = new List<Parcel>(); 

    public Property(int conscriptionNumber, string description, GPSRectangle gpsRectangle)
        : base(description,gpsRectangle)
    {
        ConscriptionNumber = conscriptionNumber;
        this.SetUpperRight(gpsRectangle.UpperRight as GPSPoint);
        this.SetLowerLeft(gpsRectangle.LowerLeft as GPSPoint);
    }

    public void AddParcel(Parcel parcel)
    {
        PositionedOnParcels.Add(parcel);
    }

    public void RemoveParcel(Parcel parcel)
    {
        PositionedOnParcels.Remove(parcel);
    }

    public void ReleaseParcels()
    {
        PositionedOnParcels.ForEach(parcel => parcel.RemoveProperty(this));
        PositionedOnParcels.Clear();
    }

    public int GetSize()
    {
        throw new NotImplementedException();
    }

    public BitArray GetHash()
    {
        throw new NotImplementedException();
    }

    public bool MyEquals(IDHRecord<Property> other)
    {
        return this.ConscriptionNumber == (other as Property).ConscriptionNumber;
    }

    public byte[] ToByteArray()
    {
        throw new NotImplementedException();
    }

    public IDHRecord<Property> FromByteArray(byte[] byteArray)
    {
        throw new NotImplementedException();
    }

    public bool MyEquals(Property other)
    {
        throw new NotImplementedException();
    }

    Property IDHRecord<Property>.FromByteArray(byte[] byteArray)
    {
        throw new NotImplementedException();
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
        :base(description, gpsRectangle)
    {
        ParcelNumber = parcelNumber;
        this.SetLowerLeft(gpsRectangle.LowerLeft as GPSPoint);
        this.SetUpperRight(gpsRectangle.UpperRight as GPSPoint);
        Bounds = gpsRectangle;
    }

    public void AddProperty(Property property)
    {
        OccupiedByProperties.Add(property);
    }

    public void RemoveProperty(Property property)
    {
        OccupiedByProperties.Remove(property);
    }

    public void ReleaseProperties()
    {
        OccupiedByProperties.ForEach(property => property.RemoveParcel(this));
        OccupiedByProperties.Clear();
    }
}