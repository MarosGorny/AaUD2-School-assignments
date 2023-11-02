using QuadTreeDS.SpatialItems;
using System.Globalization;

namespace SemesterAssignment1.RealtyObjects;
public class RealtyObjectCSVHelper
{
    public static void ExportToCSV(List<RealtyObject> realtyObjects, string filePath)
    {
        using var writer = new StreamWriter(filePath);
        /*using var writer = new StreamWriter("file.csv")*/
      

        // Write the header
        writer.WriteLine("Type,Number,Description,LowerLeftLat,LowerLeftLong,UpperRightLat,UpperRightLong,RelatedNumbers");

        foreach (var obj in realtyObjects)
        {
            string type = obj is Property ? "Property" : "Parcel";
            string number = obj is Property p ? p.ConscriptionNumber.ToString() : ((Parcel)obj).ParcelNumber.ToString();
            string relatedNumbers = obj is Property prop
                ? string.Join(";", prop.PositionedOnParcels.Select(parcel => parcel.ParcelNumber))
                : string.Join(";", ((Parcel)obj).OccupiedByProperties.Select(property => property.ConscriptionNumber));

            var lowerLeft = (GPSPoint)obj.LowerLeft;
            var upperRight = (GPSPoint)obj.UpperRight;

            //// Inside the loop, create these additional strings to include directions
            //string lowerLeftDirection = $"{lowerLeft.LatitudeDirection}/{lowerLeft.LongitudeDirection}";
            //string upperRightDirection = $"{upperRight.LatitudeDirection}/{upperRight.LongitudeDirection}";

            writer.WriteLine(
                $"{type},{number},\"{obj.Description}\"," +
                $"{lowerLeft.X.ToString(CultureInfo.InvariantCulture)}," +
                $"{lowerLeft.Y.ToString(CultureInfo.InvariantCulture)}," +
                $"{upperRight.X.ToString(CultureInfo.InvariantCulture)}," +
                $"{upperRight.Y.ToString(CultureInfo.InvariantCulture)},"
                // + $"{relatedNumbers}"
            );
        }
    }

    public static List<RealtyObject> ImportFromCSV(string filePath)
    {
        var realtyObjects = new List<RealtyObject>();
        var lines = File.ReadAllLines(filePath);

        // Skip the header line
        foreach (var line in lines.Skip(1))
        {
            var fields = line.Split(',');

            double ParseLatitude(string s, out LatitudeDirection direction)
            {
                direction = s.StartsWith('-') ? LatitudeDirection.S : LatitudeDirection.N;
                return Math.Abs(double.Parse(s, CultureInfo.InvariantCulture));
            }

            double ParseLongitude(string s, out LongitudeDirection direction)
            {
                direction = s.StartsWith('-') ? LongitudeDirection.W : LongitudeDirection.E;
                return Math.Abs(double.Parse(s, CultureInfo.InvariantCulture));
            }

            // Parse the latitude and longitude fields along with their directions
            LatitudeDirection lowerLeftLatDirection, upperRightLatDirection;
            LongitudeDirection lowerLeftLongDirection, upperRightLongDirection;

            double lowerLeftLat = ParseLatitude(fields[3], out lowerLeftLatDirection);
            double lowerLeftLong = ParseLongitude(fields[4], out lowerLeftLongDirection);
            double upperRightLat = ParseLatitude(fields[5], out upperRightLatDirection);
            double upperRightLong = ParseLongitude(fields[6], out upperRightLongDirection);


            if (fields[0] == "Property")
            {
                var property = new Property(
                    int.Parse(fields[1]),
                    fields[2].Trim('"'), // Assuming description is enclosed in quotes
                    new GPSRectangle(
                        new GPSPoint(lowerLeftLatDirection,
                                     lowerLeftLat,
                                     lowerLeftLongDirection,
                                     lowerLeftLong),
                        new GPSPoint(upperRightLatDirection,
                                     upperRightLat,
                                     upperRightLongDirection,
                                     upperRightLong)
                    )
                );
                // Logic to handle related Parcels will go here
                realtyObjects.Add(property);
            }
            else if (fields[0] == "Parcel")
            {
                var parcel = new Parcel(
                    int.Parse(fields[1]),
                    fields[2].Trim('"'), // Assuming description is enclosed in quotes
                    new GPSRectangle(
                        new GPSPoint(lowerLeftLatDirection,
                                     lowerLeftLat,
                                     lowerLeftLongDirection,
                                     lowerLeftLong),
                        new GPSPoint(upperRightLatDirection,
                                     upperRightLat,
                                     upperRightLongDirection,
                                     upperRightLong)
                    )
                );
                // Logic to handle related Properties will go here
                realtyObjects.Add(parcel);
            }
        }

        return realtyObjects;
    }
}

