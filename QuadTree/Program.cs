﻿using System;
using QuadTree;
using QuadTree.SpatialItems;

class Program
{
    static void Main()
    {
        //Traversing the tree
        QuadTree<Guid, string> quadTree = new QuadTree<Guid, string>(new Rectangle(new Point(0, 0), new Point(100, 100)));

        // Point A will go to root
        QuadTreeObject<Guid, string> quadTreeObjectRoot = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point Root", new Point(23, 23));
        quadTree.Insert(quadTreeObjectRoot);

        //////////////////////////
        // Point C will go to NW quadrant
        QuadTreeObject<Guid, string> quadTreeObjectC = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 0", new Point(10, 90));
        quadTree.Insert(quadTreeObjectC);

        // Point D will go to NE quadrant
        QuadTreeObject<Guid, string> quadTreeObjectD = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 1", new Point(70, 90));
        quadTree.Insert(quadTreeObjectD);

        // Point A will go to SW quadrant
        QuadTreeObject<Guid, string> quadTreeObjectA = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 2", new Point(10, 10));
        quadTree.Insert(quadTreeObjectA);

        // Point B will go to SE quadrant
        QuadTreeObject<Guid, string> quadTreeObjectB = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 3", new Point(70, 10));
        quadTree.Insert(quadTreeObjectB);

        //////////////////////////
        // Point C1 to be stored in Inner NW quadrant of NW quadrant
        QuadTreeObject<Guid, string> quadTreeObjectC1 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 0.0", new Point(10, 85));
        quadTree.Insert(quadTreeObjectC1);

        // Point D1 to be stored in Inner NE quadrant of NW quadrant
        QuadTreeObject<Guid, string> quadTreeObjectD1 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 0.1", new Point(35, 85));
        quadTree.Insert(quadTreeObjectD1);

        // Point A1 to be stored in Inner SW quadrant of NW quadrant
        QuadTreeObject<Guid, string> quadTreeObjectA1 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 0.2", new Point(10, 60));
        quadTree.Insert(quadTreeObjectA1);

        // Point B1 to be stored in Inner SE quadrant of NW quadrant
        QuadTreeObject<Guid, string> quadTreeObjectB1 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 0.3", new Point(35, 60));
        quadTree.Insert(quadTreeObjectB1);

        //////////////////////////
        // Point C1 to be stored in Inner NW quadrant of NW quadrant
        QuadTreeObject<Guid, string> quadTreeObjectC11 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 3.0", new Point(10, 45));
        quadTree.Insert(quadTreeObjectC11);

        // Point D1 to be stored in Inner NE quadrant of NW quadrant
        QuadTreeObject<Guid, string> quadTreeObjectD11 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 3.1", new Point(35, 45));
        quadTree.Insert(quadTreeObjectD11);

        // Point A1 to be stored in Inner SW quadrant of NW quadrant
        QuadTreeObject<Guid, string> quadTreeObjectA11 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 3.2", new Point(10, 20));
        quadTree.Insert(quadTreeObjectA11);

        // Point B1 to be stored in Inner SE quadrant of NW quadrant
        QuadTreeObject<Guid, string> quadTreeObjectB11 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 3.3", new Point(35, 20));
        quadTree.Insert(quadTreeObjectB11);






        var traverse = quadTree.Root.PreorderTraversal();
        var traverse1 = quadTree.Root.InOrderTraversal();
        var traverse2 = quadTree.Root.PostOrderTraversal();

        Console.WriteLine("Preorder Traversal");
        for (int i = 0; i < traverse.Count ;i++)
        {
            Console.WriteLine(traverse[i].Data[0].Value);
        }

        Console.WriteLine();
        Console.WriteLine("Inorder Traversal");
        for (int i = 0; i < traverse1.Count; i++)
        {
            Console.WriteLine(traverse1[i].Data[0].Value);
        }

        Console.WriteLine();
        Console.WriteLine("Postorder Traversal");
        for (int i = 0; i < traverse2.Count; i++)
        {
            Console.WriteLine(traverse2[i].Data[0].Value);
        }
        Console.WriteLine();

        //// Initialize a QuadTree with a specified boundary
        //QuadTree<Guid, string> quadTree = new QuadTree<Guid, string>(new Rectangle(new Point(0, 0), new Point(100, 100)),0);

        //// Inserting points with associated data
        //QuadTreeObject<Guid, string> quadTreeObjectA = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point A", new Point(10, 10));
        //QuadTreeObject<Guid, string> quadTreeObjectB = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point B", new Point(50, 50));
        //QuadTreeObject<Guid, string> quadTreeObjectC = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point C", new Point(90, 90));
        //QuadTreeObject<Guid, string> quadTreeObjectC1 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point C1", new Point(90, 90));
        //QuadTreeObject<Guid, string> quadTreeObjectD = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point D", new Point(60, 90));
        //QuadTreeObject<Guid, string> quadTreeObjectX = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point X", new Point(60, 120));

        quadTree.Insert(quadTreeObjectA);
        quadTree.Insert(quadTreeObjectB);

        quadTree.Delete(quadTreeObjectA);
        quadTree.Delete(quadTreeObjectB);
        
        quadTree.Insert(quadTreeObjectC);
       // quadTree.Insert(quadTreeObjectC1);
        
       // quadTree.Delete(quadTreeObjectC1);
        
        quadTree.Insert(quadTreeObjectD);
      //  quadTree.Insert(quadTreeObjectX);

        // Finding points
        var findA = quadTree.Find(quadTreeObjectA);
        var findB = quadTree.Find(quadTreeObjectB);
        var findC = quadTree.Find(quadTreeObjectC);
      //  var findC1 = quadTree.Find(quadTreeObjectC1);
        var findD = quadTree.Find(quadTreeObjectD);
    //    var findX = quadTree.Find(quadTreeObjectX);

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
