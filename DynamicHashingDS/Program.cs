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

        DummyClass dummy2 = new DummyClass();
        dummy2.Cislo = 2;
        dummy2.ID = 3;

        DummyClass dummy3 = new DummyClass();
        dummy3.Cislo = 3;
        dummy3.ID = 4;



        var dynamicHashing = new DynamicHashing<DummyClass>(2);
        dynamicHashing.Insert(dummy);
        dynamicHashing.Insert(dummy2);
        dynamicHashing.Insert(dummy3);

    }
}
