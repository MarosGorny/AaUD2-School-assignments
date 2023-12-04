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

        //DynamicHashing<DummyClass> dynamicHashing = new DynamicHashing<DummyClass>(1, 1, "mainFile.bin", "overflowFile.bin", 2);
        //DummyClass dummyClass = new DummyClass();
        //dummyClass.Cislo = 0;
        //dummyClass.ID = 0;
        //dummyClass.Text = "0";
        //dynamicHashing.Insert(dummyClass);

        //DummyClass dummyClass1 = new DummyClass();
        //dummyClass1.Cislo = 8;
        //dummyClass1.ID = 1;
        //dummyClass1.Text = "1";
        //dynamicHashing.Insert(dummyClass1);

        //dynamicHashing.Delete(dummyClass);

        //DummyClass dummyClass2 = new DummyClass();
        //dummyClass2.Cislo = 2;
        //dummyClass2.ID = 2;
        //dummyClass2.Text = "2";
        //dynamicHashing.Insert(dummyClass2);

        //DummyClass dummyClass3 = new DummyClass();
        //dummyClass3.Cislo = 3;
        //dummyClass3.ID = 3;
        //dummyClass3.Text = "3";
        //dynamicHashing.Insert(dummyClass3);


        //DummyClass dummyClass4 = new DummyClass();
        //dummyClass4.Cislo = 4;
        //dummyClass4.ID = 4;
        //dummyClass4.Text = "4";
        //dynamicHashing.Insert(dummyClass4);

        //DummyClass dummyClass5 = new DummyClass();
        //dummyClass5.Cislo = 5;
        //dummyClass5.ID = 5;
        //dummyClass5.Text = "5";
        //dynamicHashing.Insert(dummyClass5);

        //DummyClass dummyClass6 = new DummyClass();
        //dummyClass6.Cislo = 6;
        //dummyClass6.ID = 6;
        //dummyClass6.Text = "6";
        //dynamicHashing.Insert(dummyClass6);

        //DummyClass dummyClass7 = new DummyClass();
        //dummyClass7.Cislo = 7;
        //dummyClass7.ID = 7;
        //dummyClass7.Text = "7";
        //dynamicHashing.Insert(dummyClass7);

        //DummyClass dummyClass8 = new DummyClass();
        //dummyClass8.Cislo = 8;
        //dummyClass8.ID = 8;
        //dummyClass8.Text = "8";
        //dynamicHashing.Insert(dummyClass8);

        //DummyClass dummyClass9 = new DummyClass();
        //dummyClass9.Cislo = 9;
        //dummyClass9.ID = 9;
        //dummyClass9.Text = "9";
        //dynamicHashing.Insert(dummyClass9);

        //dynamicHashing.Delete(dummyClass);
        //dynamicHashing.Delete(dummyClass2);
        //dynamicHashing.Delete(dummyClass4);
        //dynamicHashing.Delete(dummyClass6);
        //dynamicHashing.Delete(dummyClass8);

        //dynamicHashing.Delete(dummyClass2);
        //dynamicHashing.Delete(dummyClass4);

        //dynamicHashing.Delete(dummyClass6);
        //dynamicHashing.Delete(dummyClass8);

        //dynamicHashing.Delete(dummyClass9);
        //dynamicHashing.Delete(dummyClass7);

        //dynamicHashing.Delete(dummyClass5);
        //dynamicHashing.Delete(dummyClass3);



        //dynamicHashing.Delete(dummyClass1);
        //dynamicHashing.Delete(dummyClass);

        //dynamicHashing.Insert(dummyClass9);
        //dynamicHashing.Insert(dummyClass7);
        //dynamicHashing.Insert(dummyClass5);
        //dynamicHashing.Insert(dummyClass8);
        //dynamicHashing.Insert(dummyClass6);



        Console.ReadLine();
    }
}
