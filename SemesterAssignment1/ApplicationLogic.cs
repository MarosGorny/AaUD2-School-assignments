using SemesterAssignment1.RealtyObjects;
using QuadTreeDS.QuadTree;
using QuadTreeDS.SpatialItems;

namespace SemesterAssignment1;
public class ApplicationLogic
{
    private QuadTree<int, string> _propertyQuadTree;
    private QuadTree<int, string> _parcelQuadTree;
    private QuadTree<int, string> _mixedQuadTree;

    public ApplicationLogic()
    {
        var boundaryPointBottomLeft = new Point(-90, -90);
        var boundaryPointTopRight = new Point(90, 90);

        _propertyQuadTree = new QuadTree<int, string>(new Rectangle(boundaryPointBottomLeft, boundaryPointTopRight));
        _parcelQuadTree = new QuadTree<int, string>(new Rectangle(boundaryPointBottomLeft, boundaryPointTopRight));
        _mixedQuadTree = new QuadTree<int, string>(new Rectangle(boundaryPointBottomLeft, boundaryPointTopRight));
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

    public List<RealtyObject> FindProperties(Point point)
    {
        var foundObjects = new List<RealtyObject>();

        foreach (var foundProperty in _propertyQuadTree.Find(point))
        {
            if (foundProperty.Item is Property property)
            {
                foundObjects.Add(property);
            }
        }

        return foundObjects;
    }

    public List<RealtyObject> FindParcels(Point point)
    {
        var foundObjects = new List<RealtyObject>();

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
    
    public void AddProperty(Property property)
    {
        var newItem = new QuadTreeObject<int, string>(property.ConscriptionNumber, property.Description, property);
        _propertyQuadTree.Insert(newItem);
        _mixedQuadTree.Insert(newItem);

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

    public void AddParcel(Parcel parcel)
    {
        var mixedQuadTreeKey = parcel.ParcelNumber * -1; //FIXME: Better to implement option in QuadTree to use duplicate keys

        var newItem = new QuadTreeObject<int, string>(mixedQuadTreeKey, parcel.Description, parcel);
        _parcelQuadTree.Insert(newItem); //FIXME: Don't forgot that the key is negative
        _mixedQuadTree.Insert(newItem); //FIXME: Don't forgot that the key is negative

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
}
