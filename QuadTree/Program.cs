﻿using System;
using QuadTree;
using QuadTree.SpatialItems;

class Program
{
    static void Main()
    {
        // Initialize a QuadTree with a specified boundary
        QuadTree<Guid, string> quadTree = new QuadTree<Guid, string>(new Rectangle(new Point(0, 0), new Point(100, 100)),0);

        // Inserting points with associated data
        QuadTreeObject<Guid, string> quadTreeObjectA = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point A", new Point(10, 10));
        QuadTreeObject<Guid, string> quadTreeObjectB = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point B", new Point(50, 50));
        QuadTreeObject<Guid, string> quadTreeObjectC = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point C", new Point(90, 90));
        QuadTreeObject<Guid, string> quadTreeObjectC1 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point C1", new Point(90, 90));
        QuadTreeObject<Guid, string> quadTreeObjectD = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point D", new Point(60, 90));
        QuadTreeObject<Guid, string> quadTreeObjectX = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point X", new Point(60, 120));

        quadTree.Insert(quadTreeObjectA);
        quadTree.Insert(quadTreeObjectB);

        quadTree.Delete(quadTreeObjectA);
        quadTree.Delete(quadTreeObjectB);
        
        quadTree.Insert(quadTreeObjectC);
        quadTree.Insert(quadTreeObjectC1);
        
        quadTree.Delete(quadTreeObjectC1);
        
        quadTree.Insert(quadTreeObjectD);
        quadTree.Insert(quadTreeObjectX);

        // Finding points
        var findA = quadTree.Find(quadTreeObjectA);
        var findB = quadTree.Find(quadTreeObjectB);
        var findC = quadTree.Find(quadTreeObjectC);
        var findC1 = quadTree.Find(quadTreeObjectC1);
        var findD = quadTree.Find(quadTreeObjectD);
        var findX = quadTree.Find(quadTreeObjectX);

        Rectangle Rectangle1 = new Rectangle(new Point(20, 60), new Point(40, 80));
        Rectangle Rectangle2 = new Rectangle(new Point(80, 40), new Point(90, 60));
        Rectangle RectangleFull = new Rectangle(new Point(0, 0), new Point(100, 100));
        Rectangle RectangleX = new Rectangle(new Point(60, 120), new Point(70, 130));

        QuadTreeObject<Guid, string> quadTreeObjectRectangle1 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Rectangle 1", Rectangle1);
        QuadTreeObject<Guid, string> quadTreeObjectRectangle2 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Rectangle 2", Rectangle2);
        QuadTreeObject<Guid, string> quadTreeObjectRectangleFull = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Rectangle Full", RectangleFull);
        QuadTreeObject<Guid, string> quadTreeObjectRectangleX = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Rectangle X", RectangleX);

        // Inserting a rectangle with associated data
        quadTree.Insert(quadTreeObjectRectangle2);
        quadTree.Insert(quadTreeObjectRectangle1);

        quadTree.Delete(quadTreeObjectRectangle1);
        quadTree.Delete(quadTreeObjectRectangle2);
        quadTree.Delete(quadTreeObjectC);
        quadTree.Delete(quadTreeObjectD);

        var findRectangle1 = quadTree.Find(quadTreeObjectRectangle1);
        var findRectangle2 = quadTree.Find(quadTreeObjectRectangle2);
        var findRectangleFull = quadTree.Find(quadTreeObjectRectangleFull);
        var findRectangleX = quadTree.Find(quadTreeObjectRectangleX);


    }
}
