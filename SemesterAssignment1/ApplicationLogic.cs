using SemesterAssignment1.RealtyObjects;
using QuadTreeDS.QuadTree;
using QuadTreeDS.SpatialItems;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SemesterAssignment1;
public class ApplicationLogic
{
    private QuadTree<int, string> _propertyQuadTree;
    private QuadTree<int, string> _parcelQuadTree;
    private QuadTree<int, string> _mixedQuadTree;
    private Rectangle _boundary;

    public ApplicationLogic()
    {
        var boundaryPointBottomLeft = new GPSPoint(LatitudeDirection.S, 90, LongitudeDirection.W, 180);
        var boundaryPointTopRight = new GPSPoint(LatitudeDirection.N, 90, LongitudeDirection.E, 180);
        _boundary = new Rectangle(boundaryPointBottomLeft, boundaryPointTopRight);

        _propertyQuadTree = new QuadTree<int, string>(_boundary);
        _parcelQuadTree = new QuadTree<int, string>(_boundary);
        _mixedQuadTree = new QuadTree<int, string>(_boundary);
    }

    public List<RealtyObject> GetAllRealtyObjects()
    {
        var allRealtyObjects = new List<RealtyObject>();
        foreach (var nodes in _mixedQuadTree.Root.InOrderTraversal())
        {
            foreach (var realtyObject in nodes.Data)
            {
                allRealtyObjects.Add(realtyObject.Item as RealtyObject);
            }
        }
        return allRealtyObjects;
    }

    public void CreateOptimalizedTrees(List<RealtyObject> objectsList)
    {
        var propertyObjectsList = new List<QuadTreeObject<int, string>>();
        var parcelObjectsList = new List<QuadTreeObject<int, string>>();
        var mixedObjectsList = new List<QuadTreeObject<int, string>>();


        foreach(var realtyObject in objectsList)
        {
            if (realtyObject is Property property)
            {
                propertyObjectsList.Add(new QuadTreeObject<int, string>(property.ConscriptionNumber, property.Description, property));
                mixedObjectsList.Add(new QuadTreeObject<int, string>(property.ConscriptionNumber, property.Description, property));
            }
            else if (realtyObject is Parcel parcel)
            {
                parcelObjectsList.Add(new QuadTreeObject<int, string>(parcel.ParcelNumber , parcel.Description, parcel));
                mixedObjectsList.Add(new QuadTreeObject<int, string>(parcel.ParcelNumber * -1, parcel.Description, parcel));
            }

            
        }

        var

        _parcelQuadTree = new QuadTree<int, string>(_boundary,parcelObjectsList, parcelObjectsList.Count, 2);
        _propertyQuadTree = new QuadTree<int, string>(_boundary, propertyObjectsList, propertyObjectsList.Count, 2);
        _mixedQuadTree = new QuadTree<int, string>(_boundary, mixedObjectsList, objectsList.Count, 2);
    }

    public void ClearAll()
    {
        _propertyQuadTree = new QuadTree<int, string>(_propertyQuadTree.Boundary);
        _parcelQuadTree = new QuadTree<int, string>(_parcelQuadTree.Boundary);
        _mixedQuadTree = new QuadTree<int, string>(_mixedQuadTree.Boundary);
    }

    public void ExportCSV(List<RealtyObject> realtyList, string fullPath)
    {
        RealtyObjectCSVHelper.ExportToCSV(realtyList, fullPath);
    }

    public List<RealtyObject> ImportCSV(string filePath)
    {
        return RealtyObjectCSVHelper.ImportFromCSV(filePath);
    }

    public List<RealtyObject> FindObjectsInArea(Rectangle area)
    {
        var foundObjects = new List<RealtyObject>();

        foreach (var foundObject in _mixedQuadTree.Find(area))
        {
            if (foundObject.Item is RealtyObject realtyObject)
            {
                foundObjects.Add(realtyObject);
            }
        }

        return foundObjects;
    }

    public List<Property> FindProperties(Point point)
    {
        var foundObjects = new List<Property>();

        foreach (var foundProperty in _propertyQuadTree.Find(point))
        {
            if (foundProperty.Item is Property property)
            {
                foundObjects.Add(property);
            }
        }

        return foundObjects;
    }

    public List<Parcel> FindParcels(Point point)
    {
        var foundObjects = new List<Parcel>();

        foreach (var foundParcel in _parcelQuadTree.Find(point))
        {
            if (foundParcel.Item is Parcel parcel)
            {
                foundObjects.Add(parcel);
            }
        }

        return foundObjects;
    }

    public void AddObject(RealtyObject realtyObject)
    {
        if(realtyObject is Property property)
        {
            AddProperty(property);
        }
        else if(realtyObject is Parcel parcel)
        {
            AddParcel(parcel);
        }
        else
        {
            throw new Exception("SpatialItem is not of type Property or Parcel");
        }
    }
    
    public void AddProperty(Property property, bool linkParcels = true)
    {
        var newItem = new QuadTreeObject<int, string>(property.ConscriptionNumber, property.Description, property);
        _propertyQuadTree.Insert(newItem);
        _mixedQuadTree.Insert(newItem);

        if(!linkParcels)
        {
            return;
        }

        var foundParcels = _parcelQuadTree.Find(new Rectangle(property.LowerLeft, property.UpperRight));
        foreach (var foundParcel in foundParcels)
        {
            if (foundParcel.Item is Parcel parcel)
            {
                parcel.AddProperty(property);
                property.AddParcel(parcel);
            }
        }
    }
    public bool DeleteProperty(Property property)
    {
        var propertyQuadTreeObject = new QuadTreeObject<int, string>(property.ConscriptionNumber, property.Description, property);
        var foundPropertyNode = _propertyQuadTree.FindNode(propertyQuadTreeObject).foundNode;

        if (foundPropertyNode is not null)
        {
            var deletedProperty = foundPropertyNode.Delete(propertyQuadTreeObject) as Property;
            if (deletedProperty is not null)
            {
                deletedProperty.ReleaseParcels();
                _mixedQuadTree.Delete(propertyQuadTreeObject);
                return true;
            }
        }

        return false;
    }

    public void AddParcel(Parcel parcel, bool linkProperties = true)
    {
        var mixedQuadTreeKey = parcel.ParcelNumber * -1; //FIXME: Better to implement option in QuadTree to use duplicate keys

        var newItem = new QuadTreeObject<int, string>(mixedQuadTreeKey, parcel.Description, parcel);
        _parcelQuadTree.Insert(newItem); //FIXME: Don't forgot that the key is negative
        _mixedQuadTree.Insert(newItem); //FIXME: Don't forgot that the key is negative

        if (!linkProperties)
        {
            return;
        }

        var foundProperties = _propertyQuadTree.Find(new Rectangle(parcel.LowerLeft, parcel.UpperRight));
        foreach (var foundProperty in foundProperties)
        {
            if (foundProperty.Item is Property property)
            {
                property.AddParcel(parcel);
                parcel.AddProperty(property);
            }
        }
    }

    public bool DeleteParcel(Parcel parcel)
    {
        var mixedQuadTreeKey = parcel.ParcelNumber * -1; //FIXME: Better to implement option in QuadTree to use duplicate keys

        var parcelQuadTreeObject = new QuadTreeObject<int, string>(mixedQuadTreeKey, parcel.Description, parcel);
        var foundParcelNode = _parcelQuadTree.FindNode(parcelQuadTreeObject).foundNode;

        if (foundParcelNode is not null)
        {
            var deletedParcel = foundParcelNode.Delete(parcelQuadTreeObject) as Parcel;
            if (deletedParcel is not null)
            {
                deletedParcel.ReleaseProperties(); 
                _mixedQuadTree.Delete(parcelQuadTreeObject); 
                return true;
            }
        }

        return false;
    }

    public void GenerateNewData(int propertyCount, int parcelCount, GPSRectangle boundary)
    {
        var realtyObjectsGenerator = new RealtyObjectsGenerator();
        var (parcels, properties) = realtyObjectsGenerator.GenerateRealtyObjects(parcelCount, propertyCount, boundary);

        ClearAll();

        foreach (var property in properties)
        {
            AddProperty(property, true);
        }

        foreach (var parcel in parcels)
        {
            AddParcel(parcel, true);
        }
    }

    //need to find spatialItem from quadtree
    public (QuadTreeNode<int, string>? foundNode, SpatialItem? foundObject) FindObject(SpatialItem spatialItem)
    {
        if(spatialItem is Property property)
        {
            return FindProperty(property);
        }
        else if(spatialItem is Parcel parcel)
        {
            return FindParcel(parcel);
        }
        else
        {
            throw new Exception("SpatialItem is not of type Property or Parcel");
        }
    }

    public (QuadTreeNode<int,string>? foundNode, Parcel? parcel) FindParcel(Parcel parcel)
    {
        QuadTreeObject<int, string> quadTreeObject = new QuadTreeObject<int, string>(parcel.ParcelNumber * -1, parcel.Description, parcel);
        var result = _parcelQuadTree.FindNode(quadTreeObject);
        var foundNode = result.foundNode;
        var foundParcel=  result.foundObject?.Item as Parcel;
        return (foundNode, foundParcel);
    }

    public (QuadTreeNode<int, string>? foundNode, Property? property) FindProperty(Property property)
    {
        QuadTreeObject<int, string> quadTreeObject = new QuadTreeObject<int, string>(property.ConscriptionNumber, property.Description, property);
        var result = _propertyQuadTree.FindNode(quadTreeObject);
        var foundNode = result.foundNode;
        var foundProperty = result.foundObject?.Item as Property;
        return (foundNode, foundProperty);
    }

    public bool SearchKey(RealtyObject realtyObject, int key)
    {
        if(realtyObject is Property property)
        {
            return _propertyQuadTree.SearchKey(key);
        }
        else if(realtyObject is Parcel parcel)
        {
            return _mixedQuadTree.SearchKey(key);
        }
        else
        {
            throw new Exception("SpatialItem is not of type Property or Parcel");
        }
    }
}
