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
            case Parcel parcel:
                AddParcel(parcel);
                break;
            default:
                throw new Exception("Unknown realty object type");
        }
    }

    public void AddObjectToQuadTree(RealtyObject realtyObject)
    {
        switch (realtyObject)
        {

            case PropertyQuadObject propertyQuadObject:

                _quadTreeProperties.Insert(new QuadTreeObject<int, string>(propertyQuadObject.PropertyNumber, propertyQuadObject.Description, propertyQuadObject.Bounds));
                break;

            case ParcelQuadObject parcelQuadObject:
                _quadTreeParcels.Insert(new QuadTreeObject<int, string>(parcelQuadObject.ParcelNumber, parcelQuadObject.Description, parcelQuadObject.Bounds));
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
            return;
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

            if (isOverflowBlock)
            {
                block.ReadFromBinaryFile(_dynamicHashingProperties.FileBlockManager.OverFlowFileStream, block.BlockAddress);
            }
            else
            {
                block.ReadFromBinaryFile(_dynamicHashingProperties.FileBlockManager.MainFileStream, block.BlockAddress);
            }

            property.TryAddParcel(newParcel.ParcelNumber);
            propertiesIds.Add(property.PropertyNumber);


            for (int i = 0; i < block.RecordsList.Count; i++)
            {
                if (block.RecordsList[i].MyEquals(property))
                {
                    block.RecordsList[i] = property;
                    break;
                }
            }

            //block.RecordsList[block.RecordsList.IndexOf(property)] = property;
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
                    continue;
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
            if(isOverflowBlock)
            {
                block.ReadFromBinaryFile(_dynamicHashingParcels.FileBlockManager.OverFlowFileStream, block.BlockAddress);
            }
            else
            {
                block.ReadFromBinaryFile(_dynamicHashingParcels.FileBlockManager.MainFileStream, block.BlockAddress);
            }


            parcel.TryAddProperty(property.PropertyNumber);
            parcelsIds.Add(parcel.ParcelNumber);

            for (int i = 0; i < block.RecordsList.Count; i++)
            {
                if (block.RecordsList[i].MyEquals(parcel))
                {
                    block.RecordsList[i] = parcel;
                    break;
                }
            }

            //block.RecordsList[block.RecordsList.IndexOf(parcel)] = parcel;
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


                for (int i = 0; i < foundBlock.RecordsList.Count; i++)
                {
                    if (foundBlock.RecordsList[i].MyEquals((Property)foundProperty))
                    {
                        foundBlock.RecordsList[i] = foundProperty;
                        break;
                    }
                }

                //foundBlock.RecordsList[foundBlock.RecordsList.IndexOf(foundProperty)] = foundProperty;
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

                for (int i = 0; i < foundBlock.RecordsList.Count; i++)
                {
                    if (foundBlock.RecordsList[i].MyEquals((Parcel)foundParcel))
                    {
                        foundBlock.RecordsList[i] = foundParcel;
                        break;
                    }
                }

                //foundBlock.RecordsList[foundBlock.RecordsList.IndexOf(foundParcel)] = foundParcel;
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

    public void EditParcel(Parcel oldParcel ,Parcel parcel)
    {
        _dynamicHashingParcels.Edit(parcel);

        if(parcel.Bounds != oldParcel.Bounds)
        {
            _quadTreeParcels.Delete(new QuadTreeObject<int, string>(oldParcel.ParcelNumber, "", oldParcel.Bounds));
            _quadTreeParcels.Insert(new QuadTreeObject<int, string>(parcel.ParcelNumber, "", parcel.Bounds));
        }
    }

    public void EditProperty(Property oldProperty, Property property)
    {
        _dynamicHashingProperties.Edit(property);

        if(property.Bounds != oldProperty.Bounds)
        {
            _quadTreeProperties.Delete(new QuadTreeObject<int, string>(oldProperty.PropertyNumber, "", oldProperty.Bounds));
            _quadTreeProperties.Insert(new QuadTreeObject<int, string>(property.PropertyNumber, "", property.Bounds));
        }
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

    public void ExportProgress(string filePath)
    {
        this.ExportTrie();
        this.ExportQuadTreeCSV(filePath);
        //this.ExportQuadTreeCSV("./quadTreeAllObjects.csv");
    }

    public List<RealtyObject> ImportProgress(string filePath)
    {
        this.ImportTrie();
        //return this.ImportQuadTreeCSV("./quadTreeAllObjects.csv");
        return this.ImportQuadTreeCSV(filePath);
    }

    public List<RealtyObject> GetAllQuadTreeRealtyObjects()
    {
        var allRealtyObjects = new List<RealtyObject>();
        allRealtyObjects.AddRange(GetAllQuadTreeParcels().Cast<RealtyObject>().ToList());
        allRealtyObjects.AddRange(GetAllQuadTreeProperties().Cast<RealtyObject>().ToList());
        return allRealtyObjects;
    }


    public void ExportQuadTreeCSV(string filePath)
    {
        var allRealtyObjects = GetAllQuadTreeParcels().Cast<RealtyObject>().ToList();
        allRealtyObjects.AddRange(GetAllQuadTreeProperties().Cast<RealtyObject>().ToList());

        RealtyObjectCSVHelper.ExportToCSV(allRealtyObjects, filePath);
    }

    public List<RealtyObject> ImportQuadTreeCSV(string filePath)
    {
        return RealtyObjectCSVHelper.ImportFromCSV(filePath);
    }

    private List<ParcelQuadObject> GetAllQuadTreeParcels()
    {
        var allParcelObjects = new List<ParcelQuadObject>();
        foreach (var nodes in _quadTreeParcels.Root.InOrderTraversal())
        {
            foreach (var parcel in nodes.Data)
            {
                var id = parcel.Key;
                var description = parcel.Value;
                var bounds = (GPSRectangle)parcel.Item;
                allParcelObjects.Add(new ParcelQuadObject(id, description, bounds));
            }
        }
        return allParcelObjects;
    }

    private List<PropertyQuadObject> GetAllQuadTreeProperties()
    {
        var allPropertyObjects = new List<PropertyQuadObject>();
        foreach (var nodes in _quadTreeProperties.Root.InOrderTraversal())
        {
            foreach (var property in nodes.Data)
            {
                var id = property.Key;
                var description = property.Value;
                var bounds = (GPSRectangle)property.Item;
                allPropertyObjects.Add(new PropertyQuadObject(id, description, bounds));
            }
        }
        return allPropertyObjects;
    }

    public void GenerateNewData(int propertyCount, int parcelCount, GPSRectangle boundary, int mainBlockFactor, int overflowBlockFactor, int? maxHashSize = null)
    {
        var realtyObjectsGenerator = new RealtyObjectsGenerator();
        var (parcels, properties) = realtyObjectsGenerator.GenerateRealtyObjects(parcelCount, propertyCount, boundary);


        ClosesFiles();

        DeleteFileIfExists("./ParcelMain.dat");
        DeleteFileIfExists("./ParcelOverflow.dat");

        DeleteFileIfExists("./PropertyMain.dat");
        DeleteFileIfExists("./PropertyOverflow.dat");

        _dynamicHashingParcels = new DynamicHashing<Parcel>(mainBlockFactor, overflowBlockFactor, "./ParcelMain.dat", "./ParcelOverflow.dat", maxHashSize: maxHashSize);
        _dynamicHashingProperties = new DynamicHashing<Property>(mainBlockFactor, overflowBlockFactor, "./PropertyMain.dat", "./PropertyOverflow.dat", maxHashSize: maxHashSize);

        _quadTreeParcels = new QuadTree<int, string>(_quadTreeParcels.Boundary);
        _quadTreeProperties = new QuadTree<int, string>(_quadTreeProperties.Boundary);

        //DeleteFileIfExists(_dynamicHashingParcels?.MainFilePath);
        //DeleteFileIfExists(_dynamicHashingParcels?.OverflowFilePath);

        //DeleteFileIfExists(_dynamicHashingProperties?.MainFilePath);
        //DeleteFileIfExists(_dynamicHashingProperties?.OverflowFilePath);

        if (parcelCount <= propertyCount)
        {
            for (int i = 0; i < parcelCount; i++)
            {
                AddParcel(parcels[i]);
                AddProperty(properties[i]);
            }

            for (int i = parcelCount; i < propertyCount; i++)
            {
                AddProperty(properties[i]);
            }
        }
        else
        {
            for (int i = 0; i < propertyCount; i++)
            {
                AddParcel(parcels[i]);
                AddProperty(properties[i]);
            }

            for (int i = propertyCount; i < parcelCount; i++)
            {
                AddParcel(parcels[i]);
            }
        }


        //foreach (var parcel in parcels)
        //{
        //    AddParcel(parcel);
        //}

        //foreach (var property in properties)
        //{
        //    AddProperty(property);
        //}

    }

    private void DeleteFileIfExists(string filePath)
    {
        //string currentDirectory = Environment.CurrentDirectory;
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    private void ClosesFiles()
    {
        _dynamicHashingParcels?.CloseFileStreams();
        _dynamicHashingProperties?.CloseFileStreams();
    }

    public IEnumerable<PropertyQuadObject> FindProperties(Rectangle rectangle)
    {
        var foundObjects = new List<PropertyQuadObject>();

        foreach (var foundProperty in _quadTreeProperties.Find(rectangle))
        {
            var newItem = new PropertyQuadObject(foundProperty.Key, foundProperty.Value, (GPSRectangle)foundProperty.Item);

            foundObjects.Add(newItem);
        }

        return foundObjects;
    }

    public IEnumerable<ParcelQuadObject> FindParcels(Rectangle rectangle)
    {
        var foundObjects = new List<ParcelQuadObject>();

        foreach (var foundParcel in _quadTreeParcels.Find(rectangle))
        {
            var newItem = new ParcelQuadObject(foundParcel.Key, foundParcel.Value, (GPSRectangle)foundParcel.Item);

            foundObjects.Add(newItem);
        }
        return foundObjects;
    }

    public IEnumerable<ParcelQuadObject> FindParcels(GPSPoint searchPoint)
    {
        var foundObjects = new List<ParcelQuadObject>();

        foreach (var foundParcel in _quadTreeParcels.Find(searchPoint))
        {
            var newItem = new ParcelQuadObject(foundParcel.Key, foundParcel.Value, (GPSRectangle)foundParcel.Item);

            foundObjects.Add(newItem);
        }

        return foundObjects;
    }

    public IEnumerable<PropertyQuadObject> FindProperties(GPSPoint searchPoint)
    {
        var foundObjects = new List<PropertyQuadObject>();

        foreach (var foundProperty in _quadTreeProperties.Find(searchPoint))
        {
            var newItem = new PropertyQuadObject(foundProperty.Key, foundProperty.Value, (GPSRectangle)foundProperty.Item);

            foundObjects.Add(newItem);

            //if (foundProperty.Item is PropertyQuadObject property)
            //{
            //    foundObjects.Add(property);
            //}
        }

        return foundObjects;
    }

    public IEnumerable<RealtyObject> FindObjectsInArea(Rectangle area)
    {
        var foundObjects = new List<RealtyObject>();
        foundObjects.AddRange(FindParcels(area).Cast<RealtyObject>().ToList());
        foundObjects.AddRange(FindProperties(area).Cast<RealtyObject>().ToList());
        return foundObjects;

    }
}

