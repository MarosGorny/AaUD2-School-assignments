using SemesterAssignment1.RealtyObjects;
using QuadTreeDS.QuadTree;
using QuadTreeDS.SpatialItems;

namespace SemesterAssignment1;
class ApplicationLogic
{
    //Need to create one quadtree for property and one for parcel
    private QuadTree<int, Property> _propertyQuadTree;
    private QuadTree<int, Parcel> _parcelQuadTree;
    private QuadTree<int, SpatialItem> _mixedQuadTree;

    public ApplicationLogic()
    {
        var boundaryPointBottomLeft = new Point(-90, -90);
        var boundaryPointTopRight = new Point(90, 90);


        _propertyQuadTree = new QuadTree<int, Property>(new Rectangle(boundaryPointBottomLeft, boundaryPointTopRight));
        _parcelQuadTree = new QuadTree<int, Parcel>(new Rectangle(boundaryPointBottomLeft, boundaryPointTopRight));
        _mixedQuadTree = new QuadTree<int, SpatialItem>(new Rectangle(boundaryPointBottomLeft, boundaryPointTopRight));
    }

    public void AddObject(SpatialItem spatialItem)
    {
        // Add object to quadTree
        _mixedQuadTree.Insert(new QuadTreeObject<int, SpatialItem>(spatialItem.GetHashCode(), spatialItem, spatialItem));

        if(spatialItem is Property property)
        {
            AddProperty(property);
        }
        else if(spatialItem is Parcel parcel)
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
        // property is a SpatialItem
        // Add property to quadTree
        _propertyQuadTree.Insert(new QuadTreeObject<int, Property>(property.ConscriptionNumber, property, property.Boundary));
    }

    public void AddParcel(Parcel parcel)
    {
        // Add parcel to QuadTree
        _parcelQuadTree.Insert(new QuadTreeObject<int, Parcel>(parcel.ParcelNumber, parcel, parcel.Boundary));
    }
}
