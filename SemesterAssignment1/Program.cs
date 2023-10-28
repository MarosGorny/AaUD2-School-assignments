
using QuadTree.SpatialItems;
using static QuadTree.SpatialItems.Rectangle;

namespace SemesterAssignment1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            GPSPoint gPSPoint = new GPSPoint(LatitudeDirection.N, 49, LongitudeDirection.E, 18);
            GPSPoint gPSPoint1 = new GPSPoint(LatitudeDirection.N, 40, LongitudeDirection.W, 74);

        }
    }
}