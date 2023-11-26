using DynamicHashingDS.Data;
using DynamicHashingDS.DH;

namespace DynamicHashingDS;
internal class Program
{
    static void Main(string[] args)
    {
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
        dynamicHashing.Insert(dummy1);
        dynamicHashing.Insert(dummy1);
        dynamicHashing.Insert(dummy3);
        dynamicHashing.Insert(dummy);
        dynamicHashing.Insert(dummy2);


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
