using DynamicHashingDS.Data;
using DynamicHashingDS.DH;
using DynamicHashingDS.DynamicHashingTest;
using System.Reflection.Metadata;

namespace DynamicHashingDS;
internal class Program
{
    static void Main(string[] args)
    {
        DynamicHashingTester tester = new DynamicHashingTester(2, 2, 2);
        tester.EdgeTesting();

    }
}
