﻿using DynamicHashingDS.Data;
using QuadTreeDS.SpatialItems;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SemesterAssignment2.RealtyObjects;

public abstract class RealtyObject : Rectangle
{
    public GPSRectangle Bounds { get; set; }
    public RealtyObject(string description, GPSRectangle gpsRectangle)
        : base(gpsRectangle.LowerLeft as GPSPoint, gpsRectangle.UpperRight as GPSPoint)
    {
        Bounds = gpsRectangle;
    }
}

/// <summary>
/// Represents a real estate property.
/// </summary>
public class Property : RealtyObject, IDHRecord
{
    public int ConscriptionNumber { get; set; }
    public int PropertyNumber { get; set; }

    [StringLength(15)]
    public string Description { get; set; }
    public List<int> PositionedOnParcels { get; private set; } = new List<int>();

    public Property(int conscriptionNumber, string description, GPSRectangle gpsRectangle)
        : base(description, gpsRectangle)
    {
        ConscriptionNumber = conscriptionNumber;
        LowerLeft = gpsRectangle.LowerLeft;
        UpperRight = gpsRectangle.UpperRight;
    }

    public bool TryAddParcel(int parcel)
    {
        if (PositionedOnParcels.Count < 6)
        {
            PositionedOnParcels.Add(parcel);
            return true;
        }
        return false;
    }

    public void RemoveParcel(int parcel)
    {
        PositionedOnParcels.Remove(parcel);
    }


    public int GetSize()
    {
        throw new NotImplementedException();
    }

    public BitArray GetHash()
    {
        throw new NotImplementedException();
    }

    public bool MyEquals(IDHRecord other)
    {
        return this.ConscriptionNumber == (other as Property).ConscriptionNumber;
    }

    public byte[] ToByteArray()
    {
        throw new NotImplementedException();
    }

    public IDHRecord FromByteArray(byte[] byteArray)
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
    [StringLength(15)]
    public string Description { get; set; }
    public List<int> OccupiedByProperties { get; set; } = new List<int>();

    public Parcel(int parcelNumber, string description, GPSRectangle gpsRectangle)
        : base(description, gpsRectangle)
    {
        ParcelNumber = parcelNumber;
        LowerLeft = gpsRectangle.LowerLeft;
        UpperRight = gpsRectangle.UpperRight;
        Bounds = gpsRectangle;
    }

    public bool TryAddProperty(Property property)
    {
        if (OccupiedByProperties.Count < 5)
        {
            OccupiedByProperties.Add(property);
            return true;
        }
        return false;
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