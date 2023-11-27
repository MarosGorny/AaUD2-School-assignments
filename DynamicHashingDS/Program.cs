using DynamicHashingDS.Data;
using DynamicHashingDS.DH;
using DynamicHashingDS.DynamicHashingTest;
using System.Reflection.Metadata;

namespace DynamicHashingDS;
internal class Program
{
    static void Main(string[] args)
    {
        DynamicHashingTester tester = new DynamicHashingTester(1, 1, 1, useSelectedIds:true);

        for(int i = 0; i < 3; i++)
        {
            tester.RunRandomTest(10000);
            Console.WriteLine("Test " + i + " finished");
        }

        DynamicHashingTester tester1 = new DynamicHashingTester(2, 1, 2, useSelectedIds: true);

        for (int i = 0; i < 3; i++)
        {
            tester1.RunRandomTest(10000);
            Console.WriteLine("Test " + i + " finished");
        }

        DynamicHashingTester tester2 = new DynamicHashingTester(5, 1, 2, useSelectedIds: true);

        for (int i = 0; i < 3; i++)
        {
            tester2.RunRandomTest(10000);
            Console.WriteLine("Test " + i + " finished");
        }

        DynamicHashingTester tester3 = new DynamicHashingTester(2, 1, 5, useSelectedIds: true);

        for (int i = 0; i < 3; i++)
        {
            tester3.RunRandomTest(10000);
            Console.WriteLine("Test " + i + " finished");
        }

        DummyClass dummy = new DummyClass();
        dummy.Cislo = 0;

        DummyClass dummy1 = new DummyClass();
        dummy1.Cislo = 1;
        dummy1.ID = 1;
        dummy1.Text = "ahoj";

        DummyClass dummy2 = new DummyClass();
        dummy2.Cislo = 2;
        dummy2.ID = 2;

        DummyClass dummy3 = new DummyClass();
        dummy3.Cislo = 3;
        dummy3.ID = 3;




        var dynamicHashing = new DynamicHashing<DummyClass>(2,2,"testMain.bin","testFlow.bin", 2);

        dynamicHashing.TryFind(dummy1, out var foundRecord);
        Console.WriteLine("Found: " +foundRecord);
        //Console.WriteLine("Deleted: " + dynamicHashing.Delete(dummy1));
        

        dynamicHashing.Insert(dummy1);

        dynamicHashing.TryFind(dummy1, out var foundRecord1);
        Console.WriteLine("Found: " + foundRecord1);
        //Console.WriteLine("Deleted: " + dynamicHashing.Delete(dummy1));

        dynamicHashing.Insert(dummy1);

        dynamicHashing.TryFind(dummy3, out var foundRecord3);
        Console.WriteLine(foundRecord3);

        dynamicHashing.Insert(dummy3);
        //Console.WriteLine("Deleted: " + dynamicHashing.Delete(dummy3));
        dynamicHashing.Insert(dummy3);

        dynamicHashing.TryFind(dummy3, out var foundRecord4);
        Console.WriteLine(foundRecord4);

        dynamicHashing.Insert(dummy);
        dynamicHashing.Insert(dummy2);


        var records = dynamicHashing.FileBlockManager.GetAllRecords(false);
        //Console.WriteLine("Deleted: " + dynamicHashing.Delete(dummy));


        //var block = new DHBlock<DummyClass>(3,3);
        //block.AddRecord(dummy);
        //block.AddRecord(dummy2);
        //block.AddRecord(dummy3);
        //block.DeleteRecord(dummy2);
        //block.DeleteRecord(dummy);
        //block.WriteToBinaryFile("test.bin");
        //block.ReadFromBinaryFile("test.bin");
        //Console.WriteLine("Hello World!");

    }
}
