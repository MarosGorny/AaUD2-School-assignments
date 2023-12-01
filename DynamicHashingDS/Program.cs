using DynamicHashingDS.Data;
using DynamicHashingDS.DH;
using DynamicHashingDS.DynamicHashingTest;
using System.Reflection.Metadata;

namespace DynamicHashingDS;
internal class Program
{
    static void Main(string[] args)
    {
        DynamicHashingTester dynamicHashingTester = new DynamicHashingTester(-1, -1, -1);
        dynamicHashingTester.RunComplexTesting();

        //DynamicHashing<DummyClass> dynamicHashing = new DynamicHashing<DummyClass>(11, -1, "mainFile.txt", "overflowFile.txt", 11);
        //DummyClass dummyClass = new DummyClass();
        //dummyClass.Cislo = 10;
        //dummyClass.ID = 10;
        //dummyClass.Text = "4g";
        //dynamicHashing.Insert(dummyClass);
        //dynamicHashing.Delete(dummyClass);
        //dynamicHashing.Insert(dummyClass);

        //DummyClass dummyClass2 = new DummyClass();
        //dummyClass2.Cislo = 0;
        //dummyClass2.ID = 48;
        //dummyClass2.Text = "123123g";
        //dynamicHashing.Insert(dummyClass2);
    }
}
