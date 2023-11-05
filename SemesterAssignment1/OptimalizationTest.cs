using QuadTreeDS.QuadTree;
using QuadTreeDS.SpatialItems;

namespace SemesterAssignment1;
public class OptimalizationTest
{
    private QuadTree<int, string> _mixedQuadTree;
    private QuadTree<int, string> _mixedOptimalizedQuadTreePart2;
    private QuadTree<int, string> _mixedOptimalizedQuadTreePart3;


    public OptimalizationTest(int parcelCount, int propertiesCount)
    {
        var boundaryPointBottomLeft = new GPSPoint(LatitudeDirection.S, 90, LongitudeDirection.W, 180);
        var boundaryPointTopRight = new GPSPoint(LatitudeDirection.N, 90, LongitudeDirection.E, 180);
        GPSRectangle boundary = new GPSRectangle(boundaryPointBottomLeft, boundaryPointTopRight);

        RealtyObjectsGenerator generator = new RealtyObjectsGenerator();
        var (parcels, properties) = generator.GenerateRealtyObjects(parcelCount, propertiesCount, boundary);



        List<QuadTreeObject<int, string>> quadTreeObjects = new List<QuadTreeObject<int, string>>();
        quadTreeObjects.AddRange(parcels.Select(parcel => new QuadTreeObject<int, string>(parcel.ParcelNumber,parcel.Description, parcel)));
        quadTreeObjects.AddRange(properties.Select(property => new QuadTreeObject<int, string>(property.ConscriptionNumber * -1, property.Description, property)));

        _mixedQuadTree = new QuadTree<int, string>(boundary,parcelCount+propertiesCount);
        foreach (var quadTreeObject in quadTreeObjects)
        {
            _mixedQuadTree.Insert(quadTreeObject);
        }
        Console.WriteLine("Normal tree: " + _mixedQuadTree.CalculateTreeHealth());

        //_mixedOptimalizedQuadTreePart2 = new QuadTree<int, string>(boundary, quadTreeObjects, parcelCount+propertiesCount, 2);
        //_mixedOptimalizedQuadTreePart3 = new QuadTree<int, string>(boundary, quadTreeObjects, parcelCount+propertiesCount, 2);
        //Console.WriteLine("Optimalized 2 portions: " + _mixedOptimalizedQuadTreePart2.CalculateTreeHealth());
        //Console.WriteLine("Optimalized 3 portions: " + _mixedOptimalizedQuadTreePart3.CalculateTreeHealth());

        for (int i = 2; i < 11; i++ )
        {
            _mixedOptimalizedQuadTreePart2 = new QuadTree<int, string>(boundary, quadTreeObjects, parcelCount + propertiesCount, i);
            Console.WriteLine("Optimalized " + i + " portions: " + _mixedOptimalizedQuadTreePart2.CalculateTreeHealth());
        }

        for (int i = 20; i < 101; i += 20)
        {
            _mixedOptimalizedQuadTreePart2 = new QuadTree<int, string>(boundary, quadTreeObjects, parcelCount + propertiesCount, i);
            Console.WriteLine("Optimalized " + i + " portions: " + _mixedOptimalizedQuadTreePart2.CalculateTreeHealth());
        }

        
        Console.WriteLine(  );

    }
}
