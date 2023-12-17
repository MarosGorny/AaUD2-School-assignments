using DynamicHashingDS.Data;
using DynamicHashingDS.DH;
using QuadTreeDS.SpatialItems;
using SemesterAssignment2.RealtyObjects;

namespace SemesterAssignment2;
public class ApplicationLogicDH
{
    private DynamicHashing<Parcel> _dynamicHashingParcels;
    private DynamicHashing<Property> _dynamicHashingProperties;
    private Rectangle _boundary;

    public ApplicationLogicDH(string parcelMainFilePath, string parcelOverflowFilePath,
                              string propertyMainFilePath, string propertyOverflowFilePath,
                              int mainBlockFactor, int overflowBlockFactor)
    {
        var boundaryPointBottomLeft = new GPSPoint(LatitudeDirection.S, 90, LongitudeDirection.W, 180);
        var boundaryPointTopRight = new GPSPoint(LatitudeDirection.N, 90, LongitudeDirection.E, 180);
        _boundary = new Rectangle(boundaryPointBottomLeft, boundaryPointTopRight);

        _dynamicHashingParcels = new DynamicHashing<Parcel>(mainBlockFactor,overflowBlockFactor,parcelMainFilePath,parcelOverflowFilePath);
        _dynamicHashingProperties = new DynamicHashing<Property>(mainBlockFactor, overflowBlockFactor, propertyMainFilePath, propertyOverflowFilePath);
    }

    public void SetPath(bool parcelPath, string mainPath, string overFlowPath)
    {
        if(parcelPath)
        {
            _dynamicHashingParcels = new DynamicHashing<Parcel>(1, 1, mainPath, overFlowPath);
        }
        else
        {
            _dynamicHashingProperties = new DynamicHashing<Property>(1, 1, mainPath, overFlowPath);
        }
    }

    public void SetDynamicHashingSettings(bool parcelSettings, int bucketSize, int overflowBucketSize)
    {
        if (parcelSettings)
        {
            _dynamicHashingParcels = new DynamicHashing<Parcel>(bucketSize, overflowBucketSize, "./", "./");
        }
        else
        {
            _dynamicHashingProperties = new DynamicHashing<Property>(bucketSize, overflowBucketSize, "./", "./");
        }
    }

    public List<IDHRecord<Property>> GetAllProperties()
    {
        var properties = _dynamicHashingProperties.GetAllRecords();
        return properties;
    }

    public List<IDHRecord<Parcel>> GetAllParcels()
    {
        var parcels = _dynamicHashingParcels.GetAllRecords();
        return parcels;
    }

    public void AddObject(RealtyObject realtyObject)
    {
        if(realtyObject is Property property)
        {
            _dynamicHashingProperties.Insert(property);
        }
        else if(realtyObject is Parcel parcel)
        {
            _dynamicHashingParcels.Insert(parcel);
        }
        else
        {
            throw new Exception("Unknown realty object type");
        }
    }

    public Parcel TryFindParcel(int conscriptionNumber)
    {
        var parcel = _dynamicHashingParcels.TryFind(new Parcel(conscriptionNumber, "", new GPSRectangle(new GPSPoint(), new GPSPoint())),out var foundParcel, out var foundBlock, out var isOverFlowBlock);
        return foundParcel as Parcel;
    }

    public Property TryFindProperty(int propertyNumber)
    {
        var property = _dynamicHashingProperties.TryFind(new Property(propertyNumber, -1, "", new GPSRectangle(new GPSPoint(), new GPSPoint())) , out var foundProperty, out var foundBlock, out var isOverFlowBlock);
        return foundProperty as Property;
    }

    public bool DeleteParcel(int conscriptionNumber)
    {
        var parcel = _dynamicHashingParcels.Delete(new Parcel(conscriptionNumber, "", new GPSRectangle(new GPSPoint(), new GPSPoint())));
        return parcel != null;
    }

    public bool DeleteProperty(int propertyNumber)
    {
        var property = _dynamicHashingProperties.Delete(new Property(propertyNumber, -1, "", new GPSRectangle(new GPSPoint(), new GPSPoint())) );
        return property != null;
    }

    public void EditParcel(Parcel parcel)
    {
        _dynamicHashingParcels.Edit(parcel);
    }

    public void EditProperty(Property property)
    {
        _dynamicHashingProperties.Edit(property);
    }
}

