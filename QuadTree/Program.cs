using System;
using QuadTree;
using QuadTree.SpatialItems;

class Program
{
    static void Main()
    {
        // Initialize a QuadTree with a specified boundary
        QuadTree<string> quadTree = new QuadTree<string>(new Rectangle(new Point(0, 0), new Point(100, 100)));

        // Inserting points with associated data
        quadTree.Insert(new Point(10, 10), "Point A");
        quadTree.Insert(new Point(50, 50), "Point B");
        quadTree.Insert(new Point(90, 90), "Point C");
        quadTree.Insert(new Point(90, 90), "Point C.1");
        quadTree.Insert(new Point(60, 90), "Point C.1");

        // Inserting a rectangle with associated data
        quadTree.Insert(new Rectangle(new Point(20, 20), new Point(40, 40)), "Rectangle 1");
        quadTree.Insert(new Rectangle(new Point(80, 40), new Point(90, 60)), "Rectangle 2");

        // Checking data at the root
        Console.WriteLine($"Root data count: {quadTree.Root.Data.Count}");

        // Checking if subdivisions occurred at the root
        bool isSubdivided = quadTree.Root.Children != null;
        Console.WriteLine($"Root subdivided: {isSubdivided}");

        if (isSubdivided)
        {
            // Checking if data was stored in the NorthWest quadrant
            Console.WriteLine($"NorthWest quadrant data count: {quadTree.Root.Children[(int)Quadrant.NorthWest].Data.Count}");

            // ... Similarly, checks can be added for other quadrants as needed
        }

        // Additional checks and validations can be added as per requirements
    }
}
