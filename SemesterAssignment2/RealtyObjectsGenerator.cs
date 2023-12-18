using QuadTreeDS.SpatialItems;
using SemesterAssignment2.RealtyObjects;

namespace SemesterAssignment2;

public class RealtyObjectsGenerator
{
    private Random _random = new Random();

    public (List<Parcel> parcels, List<Property> properties) GenerateRealtyObjects(int parcelCount, int propertyCount, GPSRectangle boundary)
    {
        var parcels = GenerateParcels(parcelCount, boundary);
        var propertisWithinParcels = GeneratePropertiesWithinParcels(propertyCount, parcels, false);
        //var properties = GenerateProperties(propertyCount, boundary);

        return (parcels, propertisWithinParcels);
    }

    public List<Parcel> GenerateParcels(int parcelCount, GPSRectangle boundary)
    {
        var parcels = new List<Parcel>();

        var lowerLeftGPSPoint = boundary.LowerLeft as GPSPoint;
        var upperRightGPSPoint = boundary.UpperRight as GPSPoint;

        ////SAME PARCELS WITH SAME BOUNDARIES -- Request from prof. Jankovic
        //for (int i = 0; i < parcelCount; i++)
        //{
        //    double latStart = GenerateRandomCoordinate(lowerLeftGPSPoint.X, upperRightGPSPoint.X);
        //    double longStart = GenerateRandomCoordinate(lowerLeftGPSPoint.Y, upperRightGPSPoint.Y);
        //    double latEnd = GenerateRandomCoordinate(latStart, upperRightGPSPoint.X);
        //    double longEnd = GenerateRandomCoordinate(longStart, upperRightGPSPoint.Y);

        //    var parcelBottomLeft = new GPSPoint(latStart >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latStart), longStart >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longStart));
        //    var parcelTopRight = new GPSPoint(latEnd >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latEnd), longEnd >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longEnd));
        //    for (int j = 1; j < 11; j++)
        //    {
        //        var parcel = new Parcel((i * 10) + j, $"Parcel {(i * 10) + j}", new GPSRectangle(parcelBottomLeft, parcelTopRight));
        //        parcels.Add(parcel);
        //    }
        //}

        //OLD CODE FOR RANDOM PARCELS | Prof. Jankovic wanted to make changes in the code, here is the old code.
        //var ids = GenerateShuffledIds(parcelCount);

        for (int i = 1; i <= parcelCount; i++)
        {
            double latStart = GenerateRandomCoordinate(lowerLeftGPSPoint.X, upperRightGPSPoint.X);
            double longStart = GenerateRandomCoordinate(lowerLeftGPSPoint.Y, upperRightGPSPoint.Y);
            double latEnd = GenerateRandomCoordinate(latStart, upperRightGPSPoint.X);
            double longEnd = GenerateRandomCoordinate(longStart, upperRightGPSPoint.Y);

            var parcelBottomLeft = new GPSPoint(latStart >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latStart), longStart >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longStart));
            var parcelTopRight = new GPSPoint(latEnd >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latEnd), longEnd >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longEnd));

            var parcel = new Parcel(i, $"Parcel {i}", new GPSRectangle(parcelBottomLeft, parcelTopRight));
            parcels.Add(parcel);

        }

        return parcels;
    }

    //public List<Property> GenerateProperties(int propertyCount, GPSRectangle boundary)
    //{
    //    var properties = new List<Property>();
    //    var ids = GenerateShuffledIds(propertyCount);

    //    var lowerLeftGPSPoint = boundary.LowerLeft as GPSPoint;
    //    var upperRightGPSPoint = boundary.UpperRight as GPSPoint;


    //    double latStart = GenerateRandomCoordinate(lowerLeftGPSPoint.X, upperRightGPSPoint.X);
    //    double longStart = GenerateRandomCoordinate(lowerLeftGPSPoint.Y, upperRightGPSPoint.Y);
    //    double latEnd = GenerateRandomCoordinate(latStart, upperRightGPSPoint.X);
    //    double longEnd = GenerateRandomCoordinate(longStart, upperRightGPSPoint.Y);
    //    for (int i = 0; i < propertyCount; i++)
    //    {
    //        var propertyBottomLeft = new GPSPoint(latStart >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latStart), longStart >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longStart));
    //        var propertyTopRight = new GPSPoint(latEnd >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latEnd), longEnd >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longEnd));

    //        var property = new Property(ids[i], $"Property {ids[i]}", new GPSRectangle(propertyBottomLeft, propertyTopRight));
    //        properties.Add(property);
    //    }

    //    return properties;
    //}

    public List<Property> GeneratePropertiesWithinParcels(int propertyCount, List<Parcel> parcels, bool linkToParcels = false)
    {
        var properties = new List<Property>();
        //var ids = GenerateShuffledIds(propertyCount);

        //NEW CODE FOR SAME PROPERTIES -- Request from prof. Jankovic
        //for (int i = 0; i < propertyCount; i++)
        //{
        //    Parcel selectedParcel = parcels[_random.Next(parcels.Count)];
        //    GPSRectangle parcelBoundary = selectedParcel.Bounds;


        //    double latStart = GenerateRandomCoordinate(parcelBoundary.LowerLeft.X, parcelBoundary.UpperRight.X);
        //    double longStart = GenerateRandomCoordinate(parcelBoundary.LowerLeft.Y, parcelBoundary.UpperRight.Y);

        //    // Changing size of property based on parcel size
        //    double maxPropertySize = Math.Min(parcelBoundary.GetWidth(), parcelBoundary.GetHeight()) / 10;
        //    double latEnd = GenerateRandomCoordinate(latStart, Math.Min(latStart + maxPropertySize, parcelBoundary.UpperRight.X));
        //    double longEnd = GenerateRandomCoordinate(longStart, Math.Min(longStart + maxPropertySize, parcelBoundary.UpperRight.Y));
        //    for (int j = 1; j < 11; j++)
        //    {
        //        // Creating GPS point
        //        var propertyBottomLeft = new GPSPoint(latStart >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latStart), longStart >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longStart));
        //        var propertyTopRight = new GPSPoint(latEnd >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latEnd), longEnd >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longEnd));

        //        var property = new Property((i * 10) + j, $"Property {(i * 10) + j}", new GPSRectangle(propertyBottomLeft, propertyTopRight));


        //        if (linkToParcels)
        //        {
        //            selectedParcel.AddProperty(property);
        //            property.AddParcel(selectedParcel);
        //        }

        //        properties.Add(property);
        //    }
        //}

        //OLD CODE FOR RANDOM PROPERTIES
        for (int i = 0; i < propertyCount; i++)
        {
            Parcel selectedParcel = parcels[_random.Next(parcels.Count)];
            GPSRectangle parcelBoundary = selectedParcel.Bounds;

            double latStart = GenerateRandomCoordinate(parcelBoundary.LowerLeft.X, parcelBoundary.UpperRight.X);
            double longStart = GenerateRandomCoordinate(parcelBoundary.LowerLeft.Y, parcelBoundary.UpperRight.Y);

            // Changing size of property based on parcel size
            double maxPropertySize = Math.Min(parcelBoundary.GetWidth(), parcelBoundary.GetHeight()) / 10;
            double latEnd = GenerateRandomCoordinate(latStart, Math.Min(latStart + maxPropertySize, parcelBoundary.UpperRight.X));
            double longEnd = GenerateRandomCoordinate(longStart, Math.Min(longStart + maxPropertySize, parcelBoundary.UpperRight.Y));

            // Creating GPS point
            var propertyBottomLeft = new GPSPoint(latStart >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latStart), longStart >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longStart));
            var propertyTopRight = new GPSPoint(latEnd >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latEnd), longEnd >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longEnd));

            var property = new Property(i, _random.Next(), $"Property {i}", new GPSRectangle(propertyBottomLeft, propertyTopRight));


            if (linkToParcels)
            {
                selectedParcel.TryAddProperty(property.PropertyNumber);
                property.TryAddParcel(selectedParcel.ParcelNumber);
            }

            properties.Add(property);
        }

        return properties;
    }

    private double GenerateRandomCoordinate(double min, double max)
    {
        double coordinate = min + (_random.NextDouble() * (max - min));
        return coordinate;
    }

    private List<int> GenerateShuffledIds(int count)
    {
        var ids = new List<int>(count);
        for (int i = 1; i <= count; i++)
        {
            ids.Add(i);
        }
        Shuffle(ids);
        return ids;
    }

    private void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}
