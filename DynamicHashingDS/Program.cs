using DynamicHashingDS.Data;
using DynamicHashingDS.DH;

namespace DynamicHashingDS;
internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("a");
        DummyClass dummy = new DummyClass();
        dummy.Cislo = 1;
        dummy.ID = 2;
        dummy.Text = "ahoj";

        DummyClass dummy2 = new DummyClass();
        dummy2.Cislo = 2;
        dummy2.ID = 3;

        DummyClass dummy3 = new DummyClass();
        dummy3.Cislo = 3;
        dummy3.ID = 4;


        var dynamicHashing = new DynamicHashing<DummyClass>(5,5,"testMain.bin","testFlow.bin");
        dynamicHashing.Insert(dummy);
        dynamicHashing.Insert(dummy2);
        dynamicHashing.Insert(dummy3);

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
