
using QuadTreeDS.SpatialItems;
using SemesterAssignment1.RealtyObjects;
using static QuadTreeDS.SpatialItems.Rectangle;

namespace SemesterAssignment1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            #region 5 rectangles next to each other
            GPSPoint gpsPropertyLeftBottom1 = new GPSPoint(LatitudeDirection.N, 0, LongitudeDirection.E, 0);
            GPSPoint gpsPropertyRightTop1 = new GPSPoint(LatitudeDirection.N, 10, LongitudeDirection.E, 50);
            GPSRectangle gpsRectangle1 = new GPSRectangle(gpsPropertyLeftBottom1, gpsPropertyRightTop1);

            GPSPoint gpsPropertyLeftBottom2 = new GPSPoint(LatitudeDirection.N, 10, LongitudeDirection.E, 0);
            GPSPoint gpsPropertyRightTop2 = new GPSPoint(LatitudeDirection.N, 20, LongitudeDirection.E, 50);
            GPSRectangle gpsRectangle2 = new GPSRectangle(gpsPropertyLeftBottom2, gpsPropertyRightTop2);

            GPSPoint gpsPropertyLeftBottom3 = new GPSPoint(LatitudeDirection.N, 20, LongitudeDirection.E, 0);
            GPSPoint gpsPropertyRightTop3 = new GPSPoint(LatitudeDirection.N, 30, LongitudeDirection.E, 50);
            GPSRectangle gpsRectangle3 = new GPSRectangle(gpsPropertyLeftBottom3, gpsPropertyRightTop3);

            GPSPoint gpsPropertyLeftBottom4 = new GPSPoint(LatitudeDirection.N, 30, LongitudeDirection.E, 0);
            GPSPoint gpsPropertyRightTop4 = new GPSPoint(LatitudeDirection.N, 40, LongitudeDirection.E, 50);
            GPSRectangle gpsRectangle4 = new GPSRectangle(gpsPropertyLeftBottom4, gpsPropertyRightTop4);

            GPSPoint gpsPropertyLeftBottom5 = new GPSPoint(LatitudeDirection.N, 40, LongitudeDirection.E, 0);
            GPSPoint gpsPropertyRightTop5 = new GPSPoint(LatitudeDirection.N, 50, LongitudeDirection.E, 50);
            GPSRectangle gpsRectangle5 = new GPSRectangle(gpsPropertyLeftBottom5, gpsPropertyRightTop5);
            #endregion

            //Big parcel
            GPSPoint gpsBottomLeft = new GPSPoint(LatitudeDirection.N, 0, LongitudeDirection.E, 0);   
            GPSPoint gpsTopRight = new GPSPoint(LatitudeDirection.N, 50, LongitudeDirection.E, 50);   
            GPSRectangle gpsRectangle = new GPSRectangle(gpsBottomLeft, gpsTopRight);



            //Need to create a parcel
            Parcel parcel = new Parcel(1, "Parcel 1", gpsRectangle);
            Property property1 = new Property(1, "Property 1",gpsRectangle1);
            Property property2 = new Property(2, "Property 2",gpsRectangle2);
            Property property3 = new Property(3, "Property 3",gpsRectangle3);
            Property property4 = new Property(4, "Property 4",gpsRectangle4);
            Property property5 = new Property(5, "Property 5",gpsRectangle5);

            ApplicationLogic applicationLogic = new ApplicationLogic();
            applicationLogic.AddObject(property1);

        }
    }
}