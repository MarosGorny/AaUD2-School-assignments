﻿using QuadTreeDS.QuadTreeTest;

class Program
{
    static void Main()
    {
        var testerBalanced = new QuadTreeTester(0.4, 0.3, 0.3);
        testerBalanced.InsertBatch(1_000);
        testerBalanced.RunRandomTest(10_000);

        Console.WriteLine("After 1_000 inserts and 10_000 random tests, the tree is balanced.");
        Console.WriteLine();
        Console.ReadLine();

        var testerInsert = new QuadTreeTester(0.8, 0.1, 0.1);
        testerInsert.InsertBatch(1_000);
        testerInsert.RunRandomTest(10_000);

        Console.WriteLine("After 1_000 inserts and 10_000 random tests, insert tree");
        Console.WriteLine();
        Console.ReadLine();

        var testerDelete = new QuadTreeTester(0.1, 0.8, 0.1);
        testerDelete.InsertBatch(1_000);
        testerDelete.RunRandomTest(10_000);

        Console.WriteLine("After 1_000 inserts and 10_000 random tests, delete tree");
        Console.WriteLine();
        Console.ReadLine();

        var testerFind = new QuadTreeTester(0.1, 0.1, 0.8);
        testerFind.InsertBatch(1_000);
        testerFind.RunRandomTest(10_000);

        Console.WriteLine("After 1_000 inserts and 10_000 random tests, find tree");
        Console.WriteLine();
        Console.ReadLine();


        //var testerEdgeCases = new QuadTreeTester(0.4, 0.3, 0.3);
        //testerEdgeCases.quadTreeEdgeCases();



    }
}
