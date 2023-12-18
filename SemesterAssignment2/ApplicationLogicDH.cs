using DynamicHashingDS.Data;
using DynamicHashingDS.DH;
using QuadTreeDS.QuadTree;
using QuadTreeDS.SpatialItems;
using SemesterAssignment2.RealtyObjects;
using System.Text;

namespace SemesterAssignment2;
public class ApplicationLogicDH
{
    private DynamicHashing<Parcel> _dynamicHashingParcels;
    private DynamicHashing<Property> _dynamicHashingProperties;
    private QuadTree<int, string> _quadTreeParcels;
    private QuadTree<int, string> _quadTreeProperties;

    private Rectangle _boundary;

    public ApplicationLogicDH()
    {
        var boundaryPointBottomLeft = new GPSPoint(LatitudeDirection.S, 90, LongitudeDirection.W, 180);
        var boundaryPointTopRight = new GPSPoint(LatitudeDirection.N, 90, LongitudeDirection.E, 180);
        _boundary = new Rectangle(boundaryPointBottomLeft, boundaryPointTopRight);


        _quadTreeParcels = new QuadTree<int, string>(_boundary);
        _quadTreeProperties = new QuadTree<int, string>(_boundary);

    }

    public void CreateDynamicHashings(string parcelMainFilePath, string parcelOverflowFilePath,
                                        string propertyMainFilePath, string propertyOverflowFilePath,
                                        int mainBlockFactor, int overflowBlockFactor, int? maxHashSize)
    {
        _dynamicHashingParcels = new DynamicHashing<Parcel>(mainBlockFactor, overflowBlockFactor, parcelMainFilePath, parcelOverflowFilePath, maxHashSize: maxHashSize);
        _dynamicHashingProperties = new DynamicHashing<Property>(mainBlockFactor, overflowBlockFactor, propertyMainFilePath, propertyOverflowFilePath, maxHashSize: maxHashSize);
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
        switch (realtyObject)
        {
            case Property property:
                AddProperty(property);
                break;
            case Parcel newParcel:
                AddParcel(newParcel);
                break;
            default:
                throw new Exception("Unknown realty object type");
        }
    }


    private void AddParcel(Parcel newParcel)
    {
        var properties = _quadTreeProperties.Find(newParcel.Bounds);
        if (properties.Count >= 5)
        {
            throw new Exception("Parcel can only be occupied by 5 properties.");
        }

        var (propertiesStack, blocksStack, isOverflowStack) = PrepareProperties(properties);

        var newItem = new QuadTreeObject<int, string>(newParcel.ParcelNumber, newParcel.Description, newParcel.Bounds);
        _quadTreeParcels.Insert(newItem);

        List<int> propertiesIds = new List<int>();

        while (propertiesStack.Count > 0)
        {
            var property = propertiesStack.Pop();
            var block = blocksStack.Pop();
            var isOverflowBlock = isOverflowStack.Pop();

            property.TryAddParcel(newParcel.ParcelNumber);
            propertiesIds.Add(property.PropertyNumber);

            block.RecordsList[block.RecordsList.IndexOf(property)] = property;
            if (isOverflowBlock)
            {
                block.WriteToBinaryFile(_dynamicHashingProperties.FileBlockManager.OverFlowFileStream, block.BlockAddress);
            }
            else
            {
                block.WriteToBinaryFile(_dynamicHashingProperties.FileBlockManager.MainFileStream, block.BlockAddress);
            }
        }

        newParcel.OccupiedByProperties = propertiesIds;
        _dynamicHashingParcels.Insert(newParcel);
    }

    private (Stack<Property> propertiesStack, Stack<DHBlock<Property>> blocksStack, Stack<bool> isOverflowStack) PrepareProperties(IEnumerable<QuadTreeObject<int, string>> properties)
    {
        var propertiesStack = new Stack<Property>();
        var blocksStack = new Stack<DHBlock<Property>>();
        var isOverflowStack = new Stack<bool>();

        foreach (var foundProperty in properties)
        {
            bool found = _dynamicHashingProperties.TryFind(new Property(foundProperty.Key, -1, "", new GPSRectangle(new GPSPoint(), new GPSPoint())), out var property, out var foundBlock, out var isOverFlowBlock);
            if (found)
            {
                var parcelsList = ((Property)property).PositionedOnParcels;
                if (parcelsList.Count >= 6)
                {
                    throw new Exception("Property can only be positioned on 6 parcels.");
                }

                propertiesStack.Push((Property)property);
                blocksStack.Push(foundBlock);
                isOverflowStack.Push(isOverFlowBlock);
            }
            else
            {
                throw new Exception("Property not found.");
            }
        }

        return (propertiesStack, blocksStack, isOverflowStack);
    }



    private void AddProperty(Property property)
    {
        var parcels = _quadTreeParcels.Find(property.Bounds);
        if (parcels.Count >= 6)
        {
            throw new Exception("Property can only be positioned on 6 parcels.");
        }

        var (parcelsStack, blocksStack, isOverflowStack) = PrepareParcels(parcels);

        var newItem = new QuadTreeObject<int, string>(property.PropertyNumber, property.Description, property.Bounds);
        _quadTreeProperties.Insert(newItem);

        List<int> parcelsIds = new List<int>();

        while (parcelsStack.Count > 0)
        {
            var parcel = parcelsStack.Pop();
            var block = blocksStack.Pop();
            var isOverflowBlock = isOverflowStack.Pop();

            parcel.TryAddProperty(property.PropertyNumber);
            parcelsIds.Add(parcel.ParcelNumber);

            block.RecordsList[block.RecordsList.IndexOf(parcel)] = parcel;
            if (isOverflowBlock)
            {
                block.WriteToBinaryFile(_dynamicHashingParcels.FileBlockManager.OverFlowFileStream, block.BlockAddress);
            }
            else
            {
                block.WriteToBinaryFile(_dynamicHashingParcels.FileBlockManager.MainFileStream, block.BlockAddress);
            }
        }

        property.PositionedOnParcels = parcelsIds;
        _dynamicHashingProperties.Insert(property);
    }


    private (Stack<Parcel> parcelsStack, Stack<DHBlock<Parcel>> blocksStack, Stack<bool> isOverflowStack) PrepareParcels(IEnumerable<QuadTreeObject<int, string>> parcels)
    {
        var parcelsStack = new Stack<Parcel>();
        var blocksStack = new Stack<DHBlock<Parcel>>();
        var isOverflowStack = new Stack<bool>();

        foreach (var foundParcel in parcels)
        {
            bool found = _dynamicHashingParcels.TryFind(new Parcel(foundParcel.Key, "", new GPSRectangle(new GPSPoint(), new GPSPoint())), out var parcel, out var foundBlock, out var isOverFlowBlock);
            if (found)
            {
                var propertiesList = ((Parcel)parcel).OccupiedByProperties;
                if (propertiesList.Count >= 5)
                {
                    throw new Exception("Parcel can only be occupied by 5 properties.");
                }

                parcelsStack.Push((Parcel)parcel);
                blocksStack.Push(foundBlock);
                isOverflowStack.Push(isOverFlowBlock);
            }
            else
            {
                throw new Exception("Parcel not found.");
            }
        }

        return (parcelsStack, blocksStack, isOverflowStack);
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
        var parcel = _dynamicHashingParcels.Delete(new Parcel(conscriptionNumber, "", new GPSRectangle(new GPSPoint(), new GPSPoint()))) as Parcel;
        if(parcel == null)
        {
            return false;
        }
        
        
        var quadTreeParcel = _quadTreeParcels.Delete(new QuadTreeObject<int, string>(conscriptionNumber, "",parcel.Bounds));

        var properties = _quadTreeProperties.Find(quadTreeParcel);

        foreach(var connectedProperty in properties)
        {
            var found = _dynamicHashingProperties.TryFind(new Property(connectedProperty.Key, -1, "", new GPSRectangle(new GPSPoint(), new GPSPoint())), out var foundProperty, out var foundBlock, out var isOverFlowBlock);
            if(found)
            {
                ((Property)foundProperty).TryRemoveParcel(conscriptionNumber);
                foundBlock.RecordsList[foundBlock.RecordsList.IndexOf(foundProperty)] = foundProperty;
                if (isOverFlowBlock)
                {
                    foundBlock.WriteToBinaryFile(_dynamicHashingProperties.FileBlockManager.OverFlowFileStream, foundBlock.BlockAddress);
                }
                else
                {
                    foundBlock.WriteToBinaryFile(_dynamicHashingProperties.FileBlockManager.MainFileStream, foundBlock.BlockAddress);
                }
            }
        }      

        return parcel != null;
    }

    public bool DeleteProperty(int propertyNumber)
    {
        var property = _dynamicHashingProperties.Delete(new Property(propertyNumber, -1, "", new GPSRectangle(new GPSPoint(), new GPSPoint()))) as Property;
        if(property == null)
        {
            return false;
        }
        var quadTreeProperty = _quadTreeProperties.Delete(new QuadTreeObject<int, string>(propertyNumber, "", property.Bounds));

        var parcels = _quadTreeParcels.Find(quadTreeProperty);

        foreach(var connectedParcel in parcels)
        {
            var found = _dynamicHashingParcels.TryFind(new Parcel(connectedParcel.Key, "", new GPSRectangle(new GPSPoint(), new GPSPoint())), out var foundParcel, out var foundBlock, out var isOverFlowBlock);
            if(found)
            {
                ((Parcel)foundParcel).TryRemoveProperty(propertyNumber);
                foundBlock.RecordsList[foundBlock.RecordsList.IndexOf(foundParcel)] = foundParcel;
                if (isOverFlowBlock)
                {
                    foundBlock.WriteToBinaryFile(_dynamicHashingParcels.FileBlockManager.OverFlowFileStream, foundBlock.BlockAddress);
                }
                else
                {
                    foundBlock.WriteToBinaryFile(_dynamicHashingParcels.FileBlockManager.MainFileStream, foundBlock.BlockAddress);
                }
            }
        }


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


    public string GetSequentialBlockOutputForAllFiles()
    {
        StringBuilder output = new StringBuilder();

        output.AppendLine("---- Sequential Block Output for Parcels ----");
        output.Append(_dynamicHashingParcels.FileBlockManager.SequentialFileOutput(_dynamicHashingParcels.MaxHashSize));

        output.AppendLine("\n---- Sequential Block Output for Properties ----");
        output.Append(_dynamicHashingProperties.FileBlockManager.SequentialFileOutput(_dynamicHashingProperties.MaxHashSize));

        return output.ToString();
    }

    public void ExportTrie()
    {
        DynamicHashingExport<Parcel> dynamicHashingExportParcel = new DynamicHashingExport<Parcel>();
        dynamicHashingExportParcel.ExportToFile(_dynamicHashingParcels, "exportParcel.json");
        _dynamicHashingParcels.FileBlockManager.ExportToFile("exportParcelFileBlockInfo.json");

        DynamicHashingExport<Property> dynamicHashingExportProperty = new DynamicHashingExport<Property>();
        dynamicHashingExportProperty.ExportToFile(_dynamicHashingProperties, "exportProperty.json");
        _dynamicHashingProperties.FileBlockManager.ExportToFile("exportPropertyFileBlockInfo.json");
    }

    public void ImportTrie()
    {
        _dynamicHashingParcels = DynamicHashingExport<Parcel>.ImportFromFile("exportParcel.json");
        _dynamicHashingParcels.UpdateParentReferences();
        _dynamicHashingParcels.FileBlockManager.ImportFromFile("exportParcelFileBlockInfo.json");

        _dynamicHashingProperties = DynamicHashingExport<Property>.ImportFromFile("exportProperty.json");
        _dynamicHashingProperties.UpdateParentReferences();
        _dynamicHashingProperties.FileBlockManager.ImportFromFile("exportPropertyFileBlockInfo.json");
    }

    public void ClosesFiles()
    {
        _dynamicHashingParcels.CloseFileStreams();
        _dynamicHashingProperties.CloseFileStreams();
    }
}

