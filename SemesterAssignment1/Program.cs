
using QuadTreeDS.SpatialItems;
using SemesterAssignment1.RealtyObjects;
using static QuadTreeDS.SpatialItems.Rectangle;

namespace SemesterAssignment1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //GPSPoint gpsPropertyLeftBottom1 = new GPSPoint(LatitudeDirection.N, 0, LongitudeDirection.E, 0);
            //GPSPoint gpsPropertyRightTop1 = new GPSPoint(LatitudeDirection.N, 10, LongitudeDirection.E, 50);
            //GPSRectangle gpsRectangle1 = new GPSRectangle(gpsPropertyLeftBottom1, gpsPropertyRightTop1);

            //GPSPoint gpsBottomLeft = new GPSPoint(LatitudeDirection.S, 0, LongitudeDirection.W, 0);
            //GPSPoint gpsTopRight = new GPSPoint(LatitudeDirection.S, 50, LongitudeDirection.W, 50);
            //GPSRectangle gpsRectangle = new GPSRectangle(gpsBottomLeft, gpsTopRight);

            ////Need to create a parcel
            //Parcel parcel = new Parcel(1, "Parcel 1", gpsRectangle);
            //Property property1 = new Property(2, "Property 1", gpsRectangle1);

            //List<RealtyObject> realObjects = new List<RealtyObject>();
            //realObjects.Add(parcel);
            //realObjects.Add(property1);

            //RealtyObjectCSVHelper.ExportToCSV(realObjects,"file.csv");
            //var files = RealtyObjectCSVHelper.ImportFromCSV("file.csv");
            //Console.WriteLine(  );
            //RealtyObjectTester.Test();

            //OptimalizationTest optimalizationTest = new OptimalizationTest(2000, 8000);
        }
    }
}