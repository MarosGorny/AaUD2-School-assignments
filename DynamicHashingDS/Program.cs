using DynamicHashingDS.Data;
using DynamicHashingDS.DH;
using DynamicHashingDS.DynamicHashingTest;
using System.Reflection.Metadata;

namespace DynamicHashingDS;
internal class Program
{
    static void Main(string[] args)
    {
        DynamicHashingTester dynamicHashingTester = new DynamicHashingTester(-1,-1,-1);
        dynamicHashingTester.RunComplexTesting();
    }
}
