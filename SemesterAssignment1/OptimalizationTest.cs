using QuadTreeDS.QuadTree;
using QuadTreeDS.SpatialItems;
using System;

namespace SemesterAssignment1;
public class OptimalizationTest
{
    private QuadTree<int, string> _mixedQuadTree;
    private QuadTree<int, string> _mixedOptimalizedQuadTreePart2;
    private QuadTree<int, string> _mixedOptimalizedQuadTreePart3;
    private Random _random;
    private List<QuadTreeObject<int, string>> _quadTreeObjects;
    private GPSRectangle _boundary;
    private RealtyObjectsGenerator _generator;
    private int _parcelCount;
    private int _propertiesCount;

    public OptimalizationTest(int parcelCount, int propertiesCount)
    {
        _random = new Random();
        _boundary = new GPSRectangle(
            new GPSPoint(LatitudeDirection.S, 90, LongitudeDirection.W, 180),
            new GPSPoint(LatitudeDirection.N, 90, LongitudeDirection.E, 180));
        _parcelCount = parcelCount;
        _propertiesCount = propertiesCount;

        _generator = new RealtyObjectsGenerator();

        TestTreeHealth();
        AverageTreeHealthOverMultipleRuns(10);
        
        //PerformActions();
    }

    private void PopulateDefaultTree()
    {
        _mixedQuadTree = new QuadTree<int, string>(_boundary, _parcelCount + _propertiesCount);
        foreach (var obj in _quadTreeObjects)
        {
            _mixedQuadTree.Insert(obj);
        }
    }

    private void PopulateOptimalizedTree()
    {
        _mixedOptimalizedQuadTreePart2 = new QuadTree<int, string>(_boundary, _quadTreeObjects, _parcelCount + _propertiesCount, 2);
    }

    private void GenerateObjects()
    {
        var (parcels, properties) = _generator.GenerateRealtyObjects(_parcelCount, _propertiesCount, _boundary);
        _quadTreeObjects = new List<QuadTreeObject<int, string>>();
        _quadTreeObjects.AddRange(parcels.Select(parcel => new QuadTreeObject<int, string>(parcel.ParcelNumber, parcel.Description, parcel)));
        _quadTreeObjects.AddRange(properties.Select(property => new QuadTreeObject<int, string>(property.ConscriptionNumber * -1, property.Description, property)));
    }

    private void PerformActions()
    {
        for(int i = 0; i < 10; i++)
        {
            GenerateObjects();

            PopulateOptimalizedTree();
            PopulateDefaultTree();

            //VerifyLists();

            FindRandomObjects();

            DeleteRandomObjects();
            Console.WriteLine("Completed run " + i + "!");

        }

        Console.WriteLine("Completed actions!");

    }

    private void InsertGeneratedObjects()
    {
        
        foreach (var obj in _quadTreeObjects)
        {
            _mixedQuadTree.Insert(obj);
            _mixedOptimalizedQuadTreePart2.Insert(obj);
        }
    }

    private void DeleteRandomObjects()
    {
        for (int i = 0; i < _parcelCount + _propertiesCount; i++)
        {
            int index = _random.Next(_quadTreeObjects.Count);
            DeleteRandomObjectDefault(index);
            DeleteRandomObjectOptimalized(index);
        }
    }

    public void DeleteRandomObjectOptimalized(int index)
    {
        var obj = _quadTreeObjects[index];
        _mixedOptimalizedQuadTreePart2.Delete(obj);
    }

    public void DeleteRandomObjectDefault(int index)
    {
        var obj = _quadTreeObjects[index];
        _mixedQuadTree.Delete(obj);
    }

    private void FindRandomObjects( )
    {
        for (int i = 0; i < _parcelCount + _propertiesCount; i++)
        {
            int index = _random.Next(_quadTreeObjects.Count);
            FindRandomObjectDefault(index);
            FindRandomObjectOptimalized(index);
        }
    }

    private void FindRandomObjectOptimalized(int index)
    {
        var obj = _quadTreeObjects[index];
        _mixedOptimalizedQuadTreePart2.Find(obj.Item);
    }

    private void FindRandomObjectDefault(int index)
    {
        var obj = _quadTreeObjects[index];
        _mixedQuadTree.Find(obj.Item);
    }

    public void TestTreeHealth()
    {
        GenerateObjects();

        PopulateDefaultTree();
        PopulateOptimalizedTree();
        Console.WriteLine("Tree health test (Combined, Data, Depth)");
        Console.WriteLine("Normal tree health: " + _mixedQuadTree.CalculateTreeHealth());
        Console.WriteLine("Optimized 2 portions: " + _mixedOptimalizedQuadTreePart2.CalculateTreeHealth());

        for (int i = 3; i < 11; i++)
        {
            _mixedOptimalizedQuadTreePart3 = new QuadTree<int, string>(_boundary, _quadTreeObjects, _parcelCount + _propertiesCount, i);
            Console.WriteLine("Optimized " + i + " portions: " + _mixedOptimalizedQuadTreePart3.CalculateTreeHealth());
        }

        for (int i = 20; i < 101; i += 20)
        {
            _mixedOptimalizedQuadTreePart3 = new QuadTree<int, string>(_boundary, _quadTreeObjects, _parcelCount + _propertiesCount, i);
            Console.WriteLine("Optimized " + i + " portions: " + _mixedOptimalizedQuadTreePart3.CalculateTreeHealth());
        }
        Console.WriteLine( );
    }

    public void AverageTreeHealthOverMultipleRuns(int numberOfRuns)
    {
        var normalTreeHealthScores = new List<(double combined, double data, double depth)>();
        var optimizedTreeHealthScores = new List<(double combined, double data, double depth)>();

        for (int run = 0; run < numberOfRuns; run++)
        {
            GenerateObjects();

            PopulateDefaultTree();
            normalTreeHealthScores.Add(_mixedQuadTree.CalculateTreeHealth());


            PopulateOptimalizedTree(); // Check the portion
            optimizedTreeHealthScores.Add(_mixedOptimalizedQuadTreePart2.CalculateTreeHealth());
        }

        // Calculate and print the averages
        var normalTreeAverageHealth = CalculateAverageHealth(normalTreeHealthScores);
        var optimizedTreeAverageHealth = CalculateAverageHealth(optimizedTreeHealthScores);

        Console.WriteLine($"Normal tree average health over {numberOfRuns} runs:\n\t (Combined: {normalTreeAverageHealth.combined}, Data: {normalTreeAverageHealth.data}, Depth: {normalTreeAverageHealth.depth})");
        Console.WriteLine($"Optimized tree (portion 2) average health over {numberOfRuns} runs:\n\t (Combined: {optimizedTreeAverageHealth.combined}, Data: {optimizedTreeAverageHealth.data}, Depth: {optimizedTreeAverageHealth.depth})");
    }

    private (double combined, double data, double depth) CalculateAverageHealth(List<(double combined, double data, double depth)> healthScores)
    {
        double combined = healthScores.Average(score => score.combined);
        double data = healthScores.Average(score => score.data);
        double depth = healthScores.Average(score => score.depth);

        return (combined, data, depth);
    }

    public void VerifyLists()
    {
        var verificationList = _mixedOptimalizedQuadTreePart2.Root.PreorderTraversal();
        var defaultVerificationList = _mixedQuadTree.Root.PreorderTraversal();
        var verificationItems = new List<QuadTreeObject<int, string>>();

        foreach (var item in verificationList)
        {
            verificationItems.AddRange(item.Data);
        }

        // Both lists should have the same count
        if (_quadTreeObjects.Count != verificationItems.Count)
        {
            Console.WriteLine("Verification failed! The lists have different counts.");
            return;
        }

        // Both lists should have the same items
        foreach (var item in _quadTreeObjects)
        {
            if (!verificationItems.Contains(item))
            {
                Console.WriteLine("Verification failed! Lists contain different items.");
                return;
            }
        }

        Console.WriteLine("Verification successful! Lists match.");
    }

}
