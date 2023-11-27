using DynamicHashingDS.Data;
using QuadTreeDS.SpatialItems;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
public class Property : RealtyObject, IDHRecord<Property>
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
        this.SetLowerLeft(gpsRectangle.LowerLeft as GPSPoint);
        this.SetUpperRight(gpsRectangle.UpperRight as GPSPoint);
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
        return sizeof(int)  // Size for ConscriptionNumber
            + sizeof(int)  // Size for PropertyNumber
            + 15 * sizeof(char)  // Size for Description (15 characters)
            + sizeof(double) * 4  // Size for 2 GPS Points (each with 2 doubles for X and Y)
            + sizeof(int) * 2 * 2  // Size for 2 GPS Point directions (Latitude and Longitude)
            + sizeof(int) * 6; // Size for PositionedOnParcels (6 integers)
    }

    public BitArray GetHash()
    {
        throw new NotImplementedException();
    }

    public byte[] ToByteArray()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write(ConscriptionNumber);
            writer.Write(PropertyNumber);
            writer.Write(Encoding.Unicode.GetBytes(Description.PadRight(15, '\0')));

            // Serialize GPSRectangle
            SerializeGPSPoint(writer, (GPSPoint)Bounds.LowerLeft);
            SerializeGPSPoint(writer, (GPSPoint)Bounds.UpperRight);

            // Serialize PositionedOnParcels
            foreach (var parcelId in PositionedOnParcels)
            {
                writer.Write(parcelId);
            }
            // Pad remaining slots for parcels
            for (int i = PositionedOnParcels.Count; i < 6; i++)
            {
                writer.Write(0); // Placeholder for empty parcel IDs
            }

            return ms.ToArray();
        }
    }

    private void SerializeGPSPoint(BinaryWriter writer, GPSPoint point)
    {
        writer.Write((int)point.LatitudeDirection);
        writer.Write((int)point.LongitudeDirection);
        writer.Write(point.X);
        writer.Write(point.Y);
    }

    private GPSPoint DeserializeGPSPoint(BinaryReader reader)
    {
        var latDir = (LatitudeDirection)reader.ReadInt32();
        var longDir = (LongitudeDirection)reader.ReadInt32();
        var x = reader.ReadDouble();
        var y = reader.ReadDouble();
        return new GPSPoint(latDir, x, longDir, y);
    }

    public bool MyEquals(Property other)
    {
        return this.ConscriptionNumber == (other as Property).ConscriptionNumber
            && this.PropertyNumber == (other as Property).PropertyNumber;
    }

    public Property FromByteArray(byte[] byteArray)
    {
        using (var ms = new MemoryStream(byteArray))
        using (var reader = new BinaryReader(ms))
        {
            ConscriptionNumber = reader.ReadInt32();
            PropertyNumber = reader.ReadInt32();
            Description = Encoding.Unicode.GetString(reader.ReadBytes(15 * sizeof(char))).TrimEnd('\0');

            // Deserialize GPSRectangle
            var lowerLeft = DeserializeGPSPoint(reader);
            var upperRight = DeserializeGPSPoint(reader);
            Bounds = new GPSRectangle(lowerLeft, upperRight);

            PositionedOnParcels.Clear();
            for (int i = 0; i < 6; i++)
            {
                int parcelId = reader.ReadInt32();
                if (parcelId != 0)
                {
                    PositionedOnParcels.Add(parcelId);
                }
            }
        }
        return this;
    }
}

/// <summary>
/// Represents a parcel of land.
/// </summary>
public class Parcel : RealtyObject, IDHRecord<Parcel>
{
    public int ParcelNumber { get; set; }
    [StringLength(15)]
    public string Description { get; set; }
    public List<int> OccupiedByProperties { get; set; } = new List<int>();

    public Parcel(int parcelNumber, string description, GPSRectangle gpsRectangle)
        : base(description, gpsRectangle)
    {
        ParcelNumber = parcelNumber;
        this.SetLowerLeft(gpsRectangle.LowerLeft as GPSPoint);
        this.SetUpperRight(gpsRectangle.UpperRight as GPSPoint);
        Bounds = gpsRectangle;

        this.SetUpperRight(gpsRectangle.UpperRight as GPSPoint);
        this.SetLowerLeft(gpsRectangle.LowerLeft as GPSPoint);
    }

    public bool TryAddProperty(int property)
    {
        if (OccupiedByProperties.Count < 5)
        {
            OccupiedByProperties.Add(property);
            return true;
        }
        return false;
    }

    public void RemoveProperty(int property)
    {
        OccupiedByProperties.Remove(property);
    }

    public int GetSize()
    {
        throw new NotImplementedException();
    }

    public BitArray GetHash()
    {
        throw new NotImplementedException();
    }

    public bool MyEquals(Parcel other)
    {
        throw new NotImplementedException();
    }

    public byte[] ToByteArray()
    {
        throw new NotImplementedException();
    }

    public Parcel FromByteArray(byte[] byteArray)
    {
        throw new NotImplementedException();
    }

    //public void ReleaseProperties()
    //{
    //    OccupiedByProperties.ForEach(property => property.RemoveParcel(this));
    //    OccupiedByProperties.Clear();
    //}
}