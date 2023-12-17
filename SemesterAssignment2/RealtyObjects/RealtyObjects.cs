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

public class PropertyQuadObject
{
    public int PropertyNumber { get; set; }
    public string Description { get; set; }
    public GPSRectangle Bounds { get; set; }

    public PropertyQuadObject(int propertyNumber, int conscriptionNumber, string description, GPSRectangle bounds)
    {
        PropertyNumber = propertyNumber;
        Description = description;
        Bounds = bounds;
    }
}

/// <summary>
/// Represents a real estate property.
/// </summary>
public class Property : RealtyObject, IDHRecord<Property>
{
    public int ConscriptionNumber { get; set; }

    [Key]
    public int PropertyNumber { get; set; }

    [StringLength(15)]
    public string Description { get; set; } = "";
    public List<int> PositionedOnParcels { get; set; } = new List<int>();

    public Property() : base("Default Description", new GPSRectangle(new GPSPoint(), new GPSPoint()))
    {
    }

    public Property(int propertyNumber, int conscriptionNumber, string description, GPSRectangle gpsRectangle)
        : base(description, gpsRectangle)
    {
        ConscriptionNumber = conscriptionNumber;
        PropertyNumber = propertyNumber;
        Description = description;
        this.SetLowerLeft(gpsRectangle.LowerLeft as GPSPoint);
        this.SetUpperRight(gpsRectangle.UpperRight as GPSPoint);
        Bounds = gpsRectangle;
    }

    public bool TryAddParcel(int parcel)
    {
        if (PositionedOnParcels.Count < 6)
        {
            PositionedOnParcels.Add(parcel);
            return true;
        }
        throw new Exception("Property can only be positioned on 6 parcels.");
    }

    public bool TryRemoveParcel(int parcel)
    {
        if (PositionedOnParcels.Count > 0)
        {
            PositionedOnParcels.Remove(parcel);
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
            + ((GPSPoint)Bounds.LowerLeft).GetSize()
            + ((GPSPoint)Bounds.UpperRight).GetSize()
            + sizeof(int) * 6; // Size for PositionedOnParcels (6 integers)
    }



    public BitArray GetHash()
    {
        int hashCode = PropertyNumber.GetHashCode();
        byte[] bytes = BitConverter.GetBytes(hashCode);
        return new BitArray(bytes);
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
            ((GPSPoint)Bounds.LowerLeft).Serialize(writer);
            ((GPSPoint)Bounds.UpperRight).Serialize(writer);
            //SerializeGPSPoint(writer, (GPSPoint)Bounds.LowerLeft);
            //SerializeGPSPoint(writer, (GPSPoint)Bounds.UpperRight);

            // Serialize PositionedOnParcels
            foreach (var parcelId in PositionedOnParcels)
            {
                writer.Write(parcelId);
            }
            // Pad remaining slots for parcels
            for (int i = PositionedOnParcels.Count; i < 6; i++)
            {
                writer.Write(-1); // Placeholder for empty parcel IDs
            }

            return ms.ToArray();
        }
    }
    public bool MyEquals(Property other)
    {
        if (other is null)
            return false;

        //return this.ConscriptionNumber == (other as Property).ConscriptionNumber
        //    && this.PropertyNumber == (other as Property).PropertyNumber;

        return this.PropertyNumber == (other as Property).PropertyNumber;
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
            //var lowerLeft = DeserializeGPSPoint(reader);
            //var upperRight = DeserializeGPSPoint(reader);
            //Bounds = new GPSRectangle(lowerLeft, upperRight);

            // Deserialize GPSRectangle
            var lowerLeft = GPSPoint.Deserialize(reader);
            var upperRight = GPSPoint.Deserialize(reader);

            this.SetLowerLeft(lowerLeft);
            this.SetUpperRight(upperRight);
            Bounds = new GPSRectangle(lowerLeft, upperRight);

            PositionedOnParcels.Clear();
            for (int i = 0; i < 6; i++)
            {
                int parcelId = reader.ReadInt32();
                if (parcelId != -1)
                {
                    PositionedOnParcels.Add(parcelId);
                }
            }
        }
        return this;
    }
}

public class ParcelQuadObject
{
    public int ParcelNumber { get; set; }
    public string Description { get; set; }
    public GPSRectangle Bounds { get; set; }

    public ParcelQuadObject(int parcelNumber, string description, GPSRectangle bounds)
    {
        ParcelNumber = parcelNumber;
        Description = description;
        Bounds = bounds;    
    }
}

/// <summary>
/// Represents a parcel of land.
/// </summary>
public class Parcel : RealtyObject, IDHRecord<Parcel>
{
    [Key]
    public int ParcelNumber { get; set; }
    [StringLength(11)]
    public string Description { get; set; } = "";
    public List<int> OccupiedByProperties { get; set; } = new List<int>();

    public Parcel(int parcelNumber, string description, GPSRectangle gpsRectangle)
        : base(description, gpsRectangle)
    {
        ParcelNumber = parcelNumber;
        Description = description;
        this.SetLowerLeft(gpsRectangle.LowerLeft as GPSPoint);
        this.SetUpperRight(gpsRectangle.UpperRight as GPSPoint);
        Bounds = gpsRectangle;

        //this.SetUpperRight(gpsRectangle.UpperRight as GPSPoint);
        //this.SetLowerLeft(gpsRectangle.LowerLeft as GPSPoint);
    }

    public Parcel() : base("Default Description", new GPSRectangle(new GPSPoint(), new GPSPoint()))
    {
    }


    public bool TryAddProperty(int property)
    {
        if (OccupiedByProperties.Count < 5)
        {
            OccupiedByProperties.Add(property);
            return true;
        }
        throw new Exception("Parcel can only be occupied by 5 properties.");
    }

    public bool TryRemoveProperty(int property)
    {
        if (OccupiedByProperties.Count > 0)
        {
            OccupiedByProperties.Remove(property);
            return true;
        }
        return false;
    }

    public int GetSize()
    {
        int size = sizeof(int)  // Size for ParcelNumber
            + 11 * sizeof(char)  // Size for Description (11 characters)
            + ((GPSPoint)Bounds.LowerLeft).GetSize()
            + ((GPSPoint)Bounds.UpperRight).GetSize()
            + sizeof(int) * 5; // Size for OccupiedByProperties (5 integers)
        return size;
    }

    public BitArray GetHash()
    {
        int hashCode = ParcelNumber.GetHashCode();
        byte[] bytes = BitConverter.GetBytes(hashCode);
        return new BitArray(bytes);
    }


    public bool MyEquals(Parcel other)
    {
        return this.ParcelNumber == other.ParcelNumber;
    }


    public byte[] ToByteArray()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write(ParcelNumber);
            writer.Write(Encoding.Unicode.GetBytes(Description.PadRight(11, '\0')));

            ((GPSPoint)Bounds.LowerLeft).Serialize(writer);
            ((GPSPoint)Bounds.UpperRight).Serialize(writer);

            foreach (var propertyId in OccupiedByProperties)
            {
                writer.Write(propertyId);
            }

            for (int i = OccupiedByProperties.Count; i < 5; i++)
            {
                writer.Write(-1); // Placeholder for empty parcel IDs
            }


            return ms.ToArray();
        }
    }

    public Parcel FromByteArray(byte[] byteArray)
    {
        using (var ms = new MemoryStream(byteArray))
        using (var reader = new BinaryReader(ms))
        {
            ParcelNumber = reader.ReadInt32();
            Description = Encoding.Unicode.GetString(reader.ReadBytes(11 * sizeof(char))).TrimEnd('\0');

            var lowerLeft = GPSPoint.Deserialize(reader);
            var upperRight = GPSPoint.Deserialize(reader);

            this.SetLowerLeft(lowerLeft);
            this.SetUpperRight(upperRight);
            Bounds = new GPSRectangle(lowerLeft, upperRight);

            OccupiedByProperties.Clear();
            // Assuming a fixed number of properties; adjust as needed
            for (int i = 0; i < 5; i++)
            {
                int propertyId = reader.ReadInt32();
                if (propertyId != -1)
                {
                    OccupiedByProperties.Add(propertyId);
                }
            }

            return this;
        }
    }

    //public void ReleaseProperties()
    //{
    //    OccupiedByProperties.ForEach(property => property.RemoveParcel(this));
    //    OccupiedByProperties.Clear();
    //}
}