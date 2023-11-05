using QuadTreeDS.QuadTree;
using QuadTreeDS.SpatialItems;

namespace QuadTreeDS.QuadTreeTest;
class QuadTreeTester
{
    private QuadTree<Guid, string> _quadTree;
    private List<QuadTreeObject<Guid, string>> _insertedObjects;
    private Random _random;
    private double _insertProbability, _deleteProbability, _findProbability;
    private double _pointProbability, _rectangleProbability;

    public QuadTreeTester(double insertProbability = 0.33, double deleteProbability = 0.33, double findProbability = 0.34, double pointProbability = 0.5)
    {
        _quadTree = new QuadTree<Guid, string>(new Rectangle(new Point(0, 0), new Point(100, 100)),10000);
        _insertedObjects = new List<QuadTreeObject<Guid, string>>();
        _random = new Random();

        // Set the probabilities
        _insertProbability = insertProbability;
        _deleteProbability = deleteProbability + insertProbability;
        _findProbability = 1.0;  // The rest is for the find operation

        _pointProbability = pointProbability;
        _rectangleProbability = 1.0 - pointProbability;
    }
    public Rectangle GenerateRandomRectangle()
    {
        Point bottomLeft = GenerateRandomPoint();
        Point topRight = new Point(
            _random.Next((int)bottomLeft.X, 101),
            _random.Next((int)bottomLeft.Y, 101));

        return new Rectangle(bottomLeft, topRight);
    }


    public Point GenerateRandomPoint()
    {
        return new Point(_random.Next(0, 101), _random.Next(0, 101));
    }

    public List<QuadTreeObject<Guid, string>> InsertBatch(int count)
    {
        List<QuadTreeObject<Guid, string>> objects = new List<QuadTreeObject<Guid, string>>();
        objects.Capacity = count;

        for (int i = 0; i < count; i++)
        {
            var randomObject = InsertRandomObject();
            objects.Add(randomObject);
            Console.WriteLine("InsertBatch: " + i);
        }

        return  objects;
    }

    public void RunRandomTest(int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            Console.WriteLine("RunRandomTest: " + i);
            double action = _random.NextDouble();

            if (action < _insertProbability)
                InsertRandomObject();
            else if (action < _deleteProbability)
                DeleteRandomObject();
            else
                FindRandomObject();
        }

        VerifyLists();
    }

    public QuadTreeObject<Guid, string> InsertRandomObject()
    {
        double typeChoice = _random.NextDouble();
        if (typeChoice < _pointProbability)
        {
            var point = GenerateRandomPoint();
            var obj = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Random Point", point);
            _quadTree.Insert(obj);
            _insertedObjects.Add(obj);
            Console.WriteLine($"Inserted Point: {obj.Value} at ({point.X}, {point.Y})");
            return obj;
        }
        else
        {
            var rectangle = GenerateRandomRectangle();
            var obj = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Random Rectangle", rectangle);
            _quadTree.Insert(obj);
            _insertedObjects.Add(obj);
            Console.WriteLine($"Inserted Rectangle: {obj.Value} at [({rectangle.LowerLeft.X}, {rectangle.LowerLeft.Y}), ({rectangle.UpperRight.X}, {rectangle.UpperRight.Y})]");
            return obj;
        }
    }

    public void DeleteRandomObject()
    {
        if (_insertedObjects.Count == 0)
        {
            double typeChoice = _random.NextDouble();
            if (typeChoice < _pointProbability)
            {
                var point = GenerateRandomPoint();
                var objNotContainedPoint = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Random Point", point);
                _quadTree.Delete(objNotContainedPoint);
                Console.WriteLine($"Tried to delete object which is not inside: {objNotContainedPoint.Value} at ({point.X}, {point.Y})");

            }
            else
            {
                var rectangle = GenerateRandomRectangle();
                var objNotContainedRectangle = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Random Rectangle", rectangle);
                _quadTree.Delete(objNotContainedRectangle);
                Console.WriteLine($"Tried to delete object which is not inside: {objNotContainedRectangle.Value} at [({rectangle.LowerLeft.X}, {rectangle.LowerLeft.Y}), ({rectangle.UpperRight.X}, {rectangle.UpperRight.Y})]");
            }
        }
        else
        {
            int index = _random.Next(_insertedObjects.Count);
            var obj = _insertedObjects[index];
            _quadTree.Delete(obj);
            _insertedObjects.RemoveAt(index);
            if (obj.Item is Point)
            {
                Point objPoint = obj.Item as Point;
                Console.WriteLine($"Deleted: {obj.Value} at ({objPoint.X}, {objPoint.Y})");
            }
            else
            {
                Rectangle objRectangle = obj.Item as Rectangle;
                Console.WriteLine($"Deleted: {obj.Value} at [({objRectangle.LowerLeft.X}, {objRectangle.LowerLeft.Y}), ({objRectangle.UpperRight.X}, {objRectangle.UpperRight.Y})]");
            }
        }
    }

    public void FindRandomObject()
    {
        if (_insertedObjects.Count == 0)
        {
            Console.WriteLine("No objects to find!");
            return;
        }

        int index = _random.Next(_insertedObjects.Count);
        var obj = _insertedObjects[index];
        var foundObj = _quadTree.Find(obj.Item);

        Console.WriteLine($"Find: {obj.Value} at ({obj.Item})");
    }

    public void VerifyLists()
    {
        var verificationList = _quadTree.Root.PreorderTraversal();
        var verificationItems = new List<QuadTreeObject<Guid, string>>();

        foreach (var item in verificationList)
        {
            verificationItems.AddRange(item.Data);
        }

        // Both lists should have the same count
        if (_insertedObjects.Count != verificationItems.Count)
        {
            Console.WriteLine("Verification failed! The lists have different counts.");
            return;
        }

        // Both lists should have the same items
        foreach (var item in _insertedObjects)
        {
            if (!verificationItems.Contains(item))
            {
                Console.WriteLine("Verification failed! Lists contain different items.");
                return;
            }
        }

        _insertedObjects.Clear();
        Console.WriteLine("Verification successful! Lists match.");
    }

    public void quadTreeEdgeCases()
    {

        #region traversal testing
        //Traversing the tree
        Rectangle baseRectangle = new Rectangle(new Point(0, 0), new Point(100, 100));
        QuadTree<Guid, string> quadTree = new QuadTree<Guid, string>(baseRectangle);

        // Point A will go to root
        //QuadTreeObject<Guid, string> quadTreeObjectRoot = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point Root", new Point(23, 23));
        //quadTree.Insert(quadTreeObjectRoot);

        ////////////////////////////
        //// Point C will go to NW quadrant
        //QuadTreeObject<Guid, string> quadTreeObjectC = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 0", new Point(10, 90));
        //quadTree.Insert(quadTreeObjectC);

        //// Point D will go to NE quadrant
        //QuadTreeObject<Guid, string> quadTreeObjectD = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 1", new Point(70, 90));
        //quadTree.Insert(quadTreeObjectD);

        //// Point A will go to SW quadrant
        //QuadTreeObject<Guid, string> quadTreeObjectA = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 2", new Point(10, 10));
        //quadTree.Insert(quadTreeObjectA);

        //// Point B will go to SE quadrant
        //QuadTreeObject<Guid, string> quadTreeObjectB = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 3", new Point(70, 10));
        //quadTree.Insert(quadTreeObjectB);

        ////////////////////////////
        //// Point C1 to be stored in Inner NW quadrant of NW quadrant
        //QuadTreeObject<Guid, string> quadTreeObjectC1 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 0.0", new Point(10, 85));
        //quadTree.Insert(quadTreeObjectC1);

        //// Point D1 to be stored in Inner NE quadrant of NW quadrant
        //QuadTreeObject<Guid, string> quadTreeObjectD1 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 0.1", new Point(35, 85));
        //quadTree.Insert(quadTreeObjectD1);

        //// Point A1 to be stored in Inner SW quadrant of NW quadrant
        //QuadTreeObject<Guid, string> quadTreeObjectA1 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 0.2", new Point(10, 60));
        //quadTree.Insert(quadTreeObjectA1);

        //// Point B1 to be stored in Inner SE quadrant of NW quadrant
        //QuadTreeObject<Guid, string> quadTreeObjectB1 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 0.3", new Point(35, 60));
        //quadTree.Insert(quadTreeObjectB1);

        ////////////////////////////
        //// Point C1 to be stored in Inner NW quadrant of NW quadrant
        //QuadTreeObject<Guid, string> quadTreeObjectC11 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 3.0", new Point(60, 45));
        //quadTree.Insert(quadTreeObjectC11);

        //// Point D1 to be stored in Inner NE quadrant of NW quadrant
        //QuadTreeObject<Guid, string> quadTreeObjectD11 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 3.1", new Point(85, 45));
        //quadTree.Insert(quadTreeObjectD11);

        //// Point A1 to be stored in Inner SW quadrant of NW quadrant
        //QuadTreeObject<Guid, string> quadTreeObjectA11 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 3.2", new Point(60, 20));
        //quadTree.Insert(quadTreeObjectA11);

        //// Point B1 to be stored in Inner SE quadrant of NW quadrant
        //QuadTreeObject<Guid, string> quadTreeObjectB11 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 3.3", new Point(85, 20));
        //quadTree.Insert(quadTreeObjectB11);

        //QuadTreeObject<Guid, string> quadTreeObjectB12 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point 3.3", new Point(85, 20));
        //quadTree.Insert(quadTreeObjectB12);
        // Reducing depth
        //quadTree.ChangeMaxAllowedDepth(0);
        //quadTree.ChangeMaxAllowedDepth(2);
        //var (bottomLeft,TopRight) = quadTree.Root.FindBoundaryPoints(quadTree.Root.InOrderTraversal());
        //Console.WriteLine("BottomLeft: " + bottomLeft.X + " " + bottomLeft.Y);

        //var traverse = quadTree.Root.PreorderTraversal(node => Console.WriteLine(node.Data.Count));
        //Console.WriteLine(quadTree.CalculateTreeHealth());
        //quadTree.Delete(quadTreeObjectA);
        //Console.WriteLine(quadTree.CalculateTreeHealth());
        //Console.WriteLine("Ideal depth for tree with 1 total data: " + quadTree.CalculateIdealDepth(1));
        //Console.WriteLine("Ideal depth for tree with 2 total data: " + quadTree.CalculateIdealDepth(2));
        //Console.WriteLine("Ideal depth for tree with 5 total data: " + quadTree.CalculateIdealDepth(5));
        //Console.WriteLine("Ideal depth for tree with 6 total data: " + quadTree.CalculateIdealDepth(6));
        //Console.WriteLine("Ideal depth for tree with 21 total data: " + quadTree.CalculateIdealDepth(21));
        //Console.WriteLine("Ideal depth for tree with 22 total data: " + quadTree.CalculateIdealDepth(22));
        
        //var traverse1 = quadTree.Root.InOrderTraversal();
        //var traverse2 = quadTree.Root.PostOrderTraversal();

        //Console.WriteLine("Preorder Traversal");
        //for (int i = 0; i < traverse.Count; i++)
        //{
        //    Console.WriteLine(traverse[i].Data[0].Value);
        //}

        //Console.WriteLine();
        //Console.WriteLine("Inorder Traversal");
        //for (int i = 0; i < traverse1.Count; i++)
        //{
        //    Console.WriteLine(traverse1[i].Data[0].Value);
        //}

        //Console.WriteLine();
        //Console.WriteLine("Postorder Traversal");
        //for (int i = 0; i < traverse2.Count; i++)
        //{
        //    Console.WriteLine(traverse2[i].Data[0].Value);
        //}
        Console.WriteLine();

        #endregion


        //#region edge cases
        ////// Initialize a QuadTree with a specified boundary
        QuadTree<int, string> quadTreeee = new QuadTree<int, string>(new Rectangle(new Point(0, 0), new Point(100, 100)), 5);

        ////// Inserting points with associated data
        //QuadTreeObject<Guid, string> quadTreeObjectA = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point A", (SpatialItem)new Point(10, 10));
        //QuadTreeObject<Guid, string> quadTreeObjectB = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point B", new Point(50, 50));
        //QuadTreeObject<Guid, string> quadTreeObjectC = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point C", new Point(90, 90));
        //QuadTreeObject<Guid, string> quadTreeObjectC1 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point C1", new Point(90, 90));
        //QuadTreeObject<Guid, string> quadTreeObjectD = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point D", new Point(60, 90));
        //QuadTreeObject<Guid, string> quadTreeObjectX = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Point X", new Point(60, 120));

        //quadTree.Insert(quadTreeObjectA);
        //quadTree.Insert(quadTreeObjectB);

        //quadTree.Delete(quadTreeObjectA);
        //quadTree.Delete(quadTreeObjectB);

        //quadTree.Insert(quadTreeObjectC);
        //quadTree.Insert(quadTreeObjectC1);

        //quadTree.Delete(quadTreeObjectC1);

        //quadTree.Insert(quadTreeObjectD);
        //quadTree.Insert(quadTreeObjectX);

        //// Finding points
        //var findA = quadTree.Find(quadTreeObjectA.Item);
        //var findB = quadTree.Find(quadTreeObjectB.Item);
        //var findC = quadTree.Find(quadTreeObjectC.Item);
        //var findC1 = quadTree.Find(quadTreeObjectC1.Item);
        //var findD = quadTree.Find(quadTreeObjectD.Item);
        //var findX = quadTree.Find(quadTreeObjectX.Item);

        Rectangle Rectangle1 = new Rectangle(new Point(20, 20), new Point(40, 40));
        Rectangle Rectangle2 = new Rectangle(new Point(60, 60), new Point(80, 80));
        Rectangle rectangle3 = new Rectangle(new Point(0, 0), new Point(100, 100));
        QuadTreeObject<int,string> quadTreeObject1 = new QuadTreeObject<int, string>(1, "Rectangle1", Rectangle1);
        QuadTreeObject<int, string> quadTreeObject2 = new QuadTreeObject<int, string>(1, "Rectangle2", Rectangle2);
        QuadTreeObject<int, string> quadTreeObject3 = new QuadTreeObject<int, string>(2, "Rectangle3", rectangle3);
        quadTreeee.Insert(quadTreeObject3);
        quadTreeee.Insert(quadTreeObject1);
        quadTreeee.Insert(quadTreeObject2);
        //Rectangle RectangleFull = new Rectangle(new Point(0, 0), new Point(100, 100));
        //Rectangle RectangleX = new Rectangle(new Point(60, 120), new Point(70, 130));

        //Rectangle RectangleMiddle = new Rectangle(new Point(30, 30), new Point(60, 60));
        //Rectangle UpperRight = new Rectangle(new Point(55, 55), new Point(90, 90));
        //Rectangle BigUpperRight = new Rectangle(new Point(28, 28), new Point(95, 95));
        //Rectangle LowerSmallLeft = new Rectangle(new Point(5, 5), new Point(20, 30));
        //QuadTreeObject<Guid,string> quadTreeObjectMiddle = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Middle", RectangleMiddle);
        //QuadTreeObject<Guid, string> quadTreeObjectUpperRight = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "UpperRight", UpperRight);
        //QuadTreeObject<Guid, string> quadTreeObjectBigUpperRight = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "BigUpperRight", BigUpperRight);
        //QuadTreeObject<Guid, string> quadTreeObjectLowerSmallLeft = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "LowerSmallLeft", LowerSmallLeft);
        //List<QuadTreeObject<Guid, string>> quadTreeObjects = new List<QuadTreeObject<Guid, string>>();
        //quadTreeObjects.Add(quadTreeObjectMiddle);
        //quadTreeObjects.Add(quadTreeObjectUpperRight);
        //quadTreeObjects.Add(quadTreeObjectBigUpperRight);
        //quadTreeObjects.Add(quadTreeObjectLowerSmallLeft);
        //QuadTreeOptimalization<Guid, string>.SortByLongestSide(quadTreeObjects);

        //QuadTree<Guid, string> quadTreeOfficial = new QuadTree<Guid, string>(baseRectangle);
        //quadTreeOfficial.Insert(quadTreeObjectMiddle);
        //quadTreeOfficial.Insert(quadTreeObjectUpperRight);
        //quadTreeOfficial.Insert(quadTreeObjectBigUpperRight);
        //quadTreeOfficial.Insert(quadTreeObjectLowerSmallLeft);
        //Console.WriteLine(quadTreeOfficial.CalculateTreeHealth());

        InsertBatch(10000);
        QuadTree<Guid, string> quadTreeOne = new QuadTree<Guid, string>(baseRectangle, 10000 );
        for (int i = 0; i < _insertedObjects.Count; i++)
        {
            quadTreeOne.Insert(_insertedObjects[i]);
        }
        Console.WriteLine(_quadTree.CalculateIdealDepth(10000));  
        Console.WriteLine(_quadTree.CalculateTreeHealth());  

        //QuadTree<Guid, string> quadTreeThree = new QuadTree<Guid, string>(baseRectangle, _insertedObjects, 10000,3);
        //Console.WriteLine("partions 3: " + quadTreeThree.CalculateTreeHealth());
        //QuadTree<Guid, string> quadTreeFour = new QuadTree<Guid, string>(baseRectangle, _insertedObjects, 10000,4);
        //Console.WriteLine("partions 4: " + quadTreeFour.CalculateTreeHealth());
        //QuadTree<Guid, string> quadTreeFive = new QuadTree<Guid, string>(baseRectangle, _insertedObjects, 10000,5);
        //Console.WriteLine("partions 5: " + quadTreeFive.CalculateTreeHealth());
        //QuadTree<Guid, string> quadTreeSix = new QuadTree<Guid, string>(baseRectangle, _insertedObjects, 10000,6);
        //Console.WriteLine("partions 6: " + quadTreeSix.CalculateTreeHealth());
        //QuadTree<Guid, string> quadTreeSeven = new QuadTree<Guid, string>(baseRectangle, _insertedObjects, 10000,7);
        //Console.WriteLine("partions 7: " + quadTreeSeven.CalculateTreeHealth());
        //QuadTree<Guid, string> quadTreeEight = new QuadTree<Guid, string>(baseRectangle, _insertedObjects, 10000,8);
        //Console.WriteLine("partions 8: " + quadTreeEight.CalculateTreeHealth());
        //QuadTree<Guid, string> quadTreeNine = new QuadTree<Guid, string>(baseRectangle, _insertedObjects, 10000,9);
        //Console.WriteLine("partions 9: " + quadTreeNine.CalculateTreeHealth());
        //QuadTree<Guid, string> quadTreeTen = new QuadTree<Guid, string>(baseRectangle, _insertedObjects, 10000,10);
        //Console.WriteLine("partions 10: " + quadTreeTen.CalculateTreeHealth());

        //Create 100 quadtrees but every with different portion
        List<QuadTree<Guid, string>> quadTrees = new List<QuadTree<Guid, string>>();
        for (int i = 2; i < 4; i++)
        {
            quadTrees.Add(new QuadTree<Guid, string>(baseRectangle, _insertedObjects, 10000, i));
            quadTrees[quadTrees.Count - 1].portions = i;
        }

        foreach (var quadTreee in quadTrees)
        {
            Console.WriteLine(quadTreee.CalculateTreeHealth());
        }

        Console.WriteLine("sorted by the longest side");
        QuadTreeOptimalization<Guid, string>.SortByLongestSide(_insertedObjects,false);
        quadTrees.Clear();
        for (int i = 2; i < 4; i++)
        {
            quadTrees.Add(new QuadTree<Guid, string>(baseRectangle, _insertedObjects, 10000, i));
            quadTrees[quadTrees.Count - 1].portions = i;
        }

        foreach (var quadTreee in quadTrees)
        {
            Console.WriteLine(quadTreee.CalculateTreeHealth());
        }

        Console.WriteLine("ASCEDING");
        QuadTreeOptimalization<Guid, string>.SortByLongestSide(_insertedObjects, true);
        quadTrees.Clear();
        for (int i = 2; i < 4; i++)
        {
            quadTrees.Add(new QuadTree<Guid, string>(baseRectangle, _insertedObjects, 10000, i));
            quadTrees[quadTrees.Count - 1].portions = i;
        }

        foreach (var quadTreee in quadTrees)
        {
            Console.WriteLine(quadTreee.CalculateTreeHealth());
        }

        Console.WriteLine(  );



        //Optimalization
        //QuadTree<Guid, string> quadTreeOptimalized = new QuadTree<Guid, string>(baseRectangle, quadTreeObjects, portions:4);
        //Console.WriteLine(quadTreeOptimalized.CalculateTreeHealth());
        //quadTreeOptimalized.Root.InOrderTraversal(node => Console.WriteLine(node.Data.Count));




        Console.WriteLine();

        //QuadTreeObject<Guid, string> quadTreeObjectRectangle1 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Rectangle 1", Rectangle1);
        //QuadTreeObject<Guid, string> quadTreeObjectRectangle2 = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Rectangle 2", Rectangle2);
        //QuadTreeObject<Guid, string> quadTreeObjectRectangleFull = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Rectangle Full", RectangleFull);
        //QuadTreeObject<Guid, string> quadTreeObjectRectangleX = new QuadTreeObject<Guid, string>(Guid.NewGuid(), "Rectangle X", RectangleX);

        //// Inserting a rectangle with associated data
        //quadTree.Insert(quadTreeObjectRectangle2);
        //quadTree.Insert(quadTreeObjectRectangle1);

        //quadTree.Delete(quadTreeObjectRectangle1);
        //quadTree.Delete(quadTreeObjectRectangle2);
        //quadTree.Delete(quadTreeObjectC);
        //quadTree.Delete(quadTreeObjectD);

        //var findRectangle1 = quadTree.Find(quadTreeObjectRectangle1.Item);
        //var findRectangle2 = quadTree.Find(quadTreeObjectRectangle2.Item);
        //var findRectangleFull = quadTree.Find(quadTreeObjectRectangleFull.Item);
        //var findRectangleX = quadTree.Find(quadTreeObjectRectangleX.Item);
        //#endregion
    }

}
