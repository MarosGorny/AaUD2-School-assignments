using QuadTreeDS.SpatialItems;
using SemesterAssignment2.RealtyObjects;
using System.Collections;


namespace SemesterAssignment2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            ApplicationLogicDH applicationLogicDH = new ApplicationLogicDH("./ParcelMain.dat", "./ParcelOverflow.dat",
                                                                            "./PropertyMain.dat", "./PropertyOverflow.dat",
                                                                            1,1);
            Parcel parcel = new Parcel(1, "Parcel 1", new GPSRectangle(new GPSPoint(LatitudeDirection.N, 0, LongitudeDirection.E, 0), new GPSPoint(LatitudeDirection.N, 10, LongitudeDirection.E, 50)));
            Property property = new Property(2,200, "Property 1", new GPSRectangle(new GPSPoint(LatitudeDirection.N, 0, LongitudeDirection.E, 0), new GPSPoint(LatitudeDirection.N, 10, LongitudeDirection.E, 50)));

            applicationLogicDH.AddObject(parcel);
            applicationLogicDH.AddObject(property);
            var parcels = applicationLogicDH.GetAllParcels();
            var properties = applicationLogicDH.GetAllProperties();
            var propertyFound = applicationLogicDH.TryFindProperty(2);
            var parcelFound = applicationLogicDH.TryFindParcel(1);

            var propertyFound2 = applicationLogicDH.TryFindProperty(3);
            var parcelFound2 = applicationLogicDH.TryFindParcel(2);

            applicationLogicDH.DeleteParcel(1);
            applicationLogicDH.DeleteProperty(2);
            parcelFound = applicationLogicDH.TryFindParcel(1);
            propertyFound = applicationLogicDH.TryFindProperty(2);

            parcels = applicationLogicDH.GetAllParcels();
            properties = applicationLogicDH.GetAllProperties();

            // Create a GPSRectangle for the test
            //GPSPoint lowerLeft = new GPSPoint(LatitudeDirection.N, 40.7128, LongitudeDirection.W, 74.0060);
            //GPSPoint upperRight = new GPSPoint(LatitudeDirection.N, 40.9138, LongitudeDirection.W, 73.8060);
            //GPSRectangle gpsRectangle = new GPSRectangle(lowerLeft, upperRight);

            //// Create a Parcel object for testing
            //Parcel parcel = new Parcel(1, "Test Parcel", gpsRectangle);
            //parcel.TryAddProperty(101);
            //parcel.TryAddProperty(102);

            //// Serialize the Parcel object
            //byte[] serializedParcel = parcel.ToByteArray();

            //// Deserialize to get a new Parcel object
            //Parcel deserializedParcel = new Parcel(0, "", new GPSRectangle(new GPSPoint(LatitudeDirection.N, 0, LongitudeDirection.E, 0), new GPSPoint(LatitudeDirection.N, 0, LongitudeDirection.E, 0)));
            //deserializedParcel = deserializedParcel.FromByteArray(serializedParcel);

            //// Test equality and hash code
            //bool areEqual = parcel.MyEquals(deserializedParcel);
            //bool parcelsEqual = parcel.MyEquals(deserializedParcel);

            //// Output the results
            //Console.WriteLine("Are the original and deserialized Parcel equal? " + areEqual);
            //Console.WriteLine("Original Parcel Size: " + parcel.GetSize());
            //Console.WriteLine("Deserialized Parcel Size: " + deserializedParcel.GetSize());
        }
    }
}