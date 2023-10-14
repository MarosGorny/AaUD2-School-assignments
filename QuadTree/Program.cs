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

        Point A = new Point(10, 10);
        Point B = new Point(50, 50);
        Point C = new Point(90, 90);
        Point C1 = new Point(90, 90);
        Point D = new Point(60, 90);
        Point X = new Point(60, 120);

        quadTree.Insert(A, "Point A");
        quadTree.Insert(B, "Point B");
        quadTree.Insert(C, "Point C");
        quadTree.Insert(C1, "Point C1");
        quadTree.Insert(D, "Point D");

        var findA = quadTree.Find(A);
        var findB = quadTree.Find(B);
        var findC = quadTree.Find(C);
        var findC1 = quadTree.Find(C1);
        var findD = quadTree.Find(D);
        var findX = quadTree.Find(X);

        Rectangle Rectangle1 = new Rectangle(new Point(20, 20), new Point(40, 40));
        Rectangle Rectangle2 = new Rectangle(new Point(80, 40), new Point(90, 60));
        Rectangle RectangleFull = new Rectangle(new Point(0, 0), new Point(100, 100));
        Rectangle RectangleX = new Rectangle(new Point(60, 120), new Point(70, 130));

        // Inserting a rectangle with associated data
        quadTree.Insert(Rectangle1, "Rectangle 1");
        quadTree.Insert(Rectangle2, "Rectangle 2");

        var findRectangle1 = quadTree.Find(Rectangle1);
        var findRectangle2 = quadTree.Find(Rectangle2);
        var findRectangleFull = quadTree.Find(RectangleFull);
        var findRectangleX = quadTree.Find(RectangleX);

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
