using QuadTreeDS.SpatialItems;
using SemesterAssignment1.RealtyObjects;
using System;
using System.Collections.Generic;

namespace SemesterAssignment1;
public class RealtyObjectsGenerator
{
    private Random _random = new Random();

    public (List<Parcel> parcels, List<Property> properties) GenerateRealtyObjects(int parcelCount, int propertyCount, GPSRectangle boundary)
    {
        var parcels = GenerateParcels(parcelCount, boundary);
        var propertisWithinParcels = GeneratePropertiesWithinParcels(propertyCount, parcels, true);
        //var properties = GenerateProperties(propertyCount, boundary);

        return (parcels, propertisWithinParcels);
    }

    // Generate parcels within a boundary
    public List<Parcel> GenerateParcels(int parcelCount, GPSRectangle boundary)
    {
        var parcels = new List<Parcel>();

        var lowerLeftGPSPoint = boundary.LowerLeft as GPSPoint;
        var upperRightGPSPoint = boundary.UpperRight as GPSPoint;

        for (int i = 0; i < parcelCount; i++)
        {
            double latStart = GenerateRandomCoordinate(lowerLeftGPSPoint.X, upperRightGPSPoint.X);
            double longStart = GenerateRandomCoordinate(lowerLeftGPSPoint.Y, upperRightGPSPoint.Y);
            double latEnd = GenerateRandomCoordinate(latStart, upperRightGPSPoint.X);
            double longEnd = GenerateRandomCoordinate(longStart, upperRightGPSPoint.Y);

            var parcelBottomLeft = new GPSPoint(latStart >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latStart), longStart >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longStart));
            var parcelTopRight = new GPSPoint(latEnd >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latEnd), longEnd >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longEnd));

            var parcel = new Parcel(i + 1, $"Parcel {i + 1}", new GPSRectangle(parcelBottomLeft, parcelTopRight));
            parcels.Add(parcel);
        }

        return parcels;
    }

    // Generate properties within a boundary
    public List<Property> GenerateProperties(int propertyCount, GPSRectangle boundary)
    {
        var properties = new List<Property>();

        var lowerLeftGPSPoint = boundary.LowerLeft as GPSPoint;
        var upperRightGPSPoint = boundary.UpperRight as GPSPoint;

        for (int i = 0; i < propertyCount; i++)
        {
            double latStart = GenerateRandomCoordinate(lowerLeftGPSPoint.X, upperRightGPSPoint.X);
            double longStart = GenerateRandomCoordinate(lowerLeftGPSPoint.Y, upperRightGPSPoint.Y);
            double latEnd = GenerateRandomCoordinate(latStart, upperRightGPSPoint.X);
            double longEnd = GenerateRandomCoordinate(longStart, upperRightGPSPoint.Y);

            var propertyBottomLeft = new GPSPoint(latStart >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latStart), longStart >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longStart));
            var propertyTopRight = new GPSPoint(latEnd >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latEnd), longEnd >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longEnd));

            var property = new Property(i + 1, $"Property {i + 1}", new GPSRectangle(propertyBottomLeft, propertyTopRight));
            properties.Add(property);
        }

        return properties;
    }

    public List<Property> GeneratePropertiesWithinParcels(int propertyCount, List<Parcel> parcels, bool linkToParcels = false)
    {
        var properties = new List<Property>();

        for (int i = 0; i < propertyCount; i++)
        {
            // Randomly select a parcel
            Parcel selectedParcel = parcels[_random.Next(parcels.Count)];
            GPSRectangle parcelBoundary = selectedParcel.Bounds;

            // Generate coordinates within the selected parcel
            double latStart = GenerateRandomCoordinate(parcelBoundary.LowerLeft.X, parcelBoundary.UpperRight.X);
            double longStart = GenerateRandomCoordinate(parcelBoundary.LowerLeft.Y, parcelBoundary.UpperRight.Y);

            // Assume properties are significantly smaller than parcels, so we limit the size
            double maxPropertySize = Math.Min(parcelBoundary.GetWidth(), parcelBoundary.GetHeight()) / 10;
            double latEnd = GenerateRandomCoordinate(latStart, Math.Min(latStart + maxPropertySize, parcelBoundary.UpperRight.X));
            double longEnd = GenerateRandomCoordinate(longStart, Math.Min(longStart + maxPropertySize, parcelBoundary.UpperRight.Y));

            // Create the GPSPoints, ensuring we handle the latitude and longitude directions properly
            var propertyBottomLeft = new GPSPoint(latStart >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latStart), longStart >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longStart));
            var propertyTopRight = new GPSPoint(latEnd >= 0 ? LatitudeDirection.N : LatitudeDirection.S, Math.Abs(latEnd), longEnd >= 0 ? LongitudeDirection.E : LongitudeDirection.W, Math.Abs(longEnd));

            // Create the property
            var property = new Property(i + 1, $"Property {i + 1}", new GPSRectangle(propertyBottomLeft, propertyTopRight));

            // Optionally link the property to the parcel
            if (linkToParcels)
            {
                selectedParcel.AddProperty(property);
            }

            properties.Add(property);
        }

        return properties;
    }

    // Utility method to generate a random coordinate between min and max
    private double GenerateRandomCoordinate(double min, double max)
    {
        double coordinate = min + (_random.NextDouble() * (max - min));
        // This allows the wrapping of coordinates to handle negative values for S and W
        return coordinate;
    }
}
