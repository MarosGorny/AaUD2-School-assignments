using QuadTreeDS.SpatialItems;
using SemesterAssignment2.RealtyObjects;
using System.Collections;


namespace SemesterAssignment2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a GPSRectangle for the test
            GPSPoint lowerLeft = new GPSPoint(LatitudeDirection.N, 40.7128, LongitudeDirection.W, 74.0060);
            GPSPoint upperRight = new GPSPoint(LatitudeDirection.N, 40.9138, LongitudeDirection.W, 73.8060);
            GPSRectangle gpsRectangle = new GPSRectangle(lowerLeft, upperRight);

            // Create a Parcel object for testing
            Parcel parcel = new Parcel(1, "Test Parcel", gpsRectangle);
            parcel.TryAddProperty(101);
            parcel.TryAddProperty(102);

            // Serialize the Parcel object
            byte[] serializedParcel = parcel.ToByteArray();

            // Deserialize to get a new Parcel object
            Parcel deserializedParcel = new Parcel(0, "", new GPSRectangle(new GPSPoint(LatitudeDirection.N, 0, LongitudeDirection.E, 0), new GPSPoint(LatitudeDirection.N, 0, LongitudeDirection.E, 0)));
            deserializedParcel = deserializedParcel.FromByteArray(serializedParcel);

            // Test equality and hash code
            bool areEqual = parcel.MyEquals(deserializedParcel);
            bool parcelsEqual = parcel.MyEquals(deserializedParcel);

            // Output the results
            Console.WriteLine("Are the original and deserialized Parcel equal? " + areEqual);
            Console.WriteLine("Original Parcel Size: " + parcel.GetSize());
            Console.WriteLine("Deserialized Parcel Size: " + deserializedParcel.GetSize());
        }
    }
}