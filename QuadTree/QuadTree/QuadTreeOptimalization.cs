using QuadTreeDS.SpatialItems;
using System;
using System.Collections.Generic;

namespace QuadTreeDS.QuadTree
{
    public static class QuadTreeOptimalization<K, V> where K : IComparable<K>
    {
        public class SubdivisionResult
        {
            public Rectangle[] Quadrants { get; set; }
            public int TotalIntersections { get; set; }
        }

        private class CutData
        {
            public double CutPosition { get; set; }
            public int IntersectionsCount { get; set; }
            public List<Rectangle> Quadrants { get; set; }
        }

        public static SubdivisionResult BestSubdivision(Rectangle quadrant, List<QuadTreeObject<K, V>> items, int portions)
        {
            if (portions < 2 ) throw new ArgumentException("cuts should be greater than 1", nameof(portions));

            List<double> horizontalCuts = GetCutPositions(quadrant, portions, true);
            List<double> verticalCuts = GetCutPositions(quadrant, portions, false);

            CutData bestHorizontalCutData = CalculateBestSubdivision(horizontalCuts, quadrant, items, isHorizontal: true);
            CutData bestVerticalCutData = CalculateBestSubdivision(verticalCuts, quadrant, items, isHorizontal: false);

            // Combine the best horizontal and vertical cuts to generate four quadrants
            List<Rectangle> combinedQuadrants = new List<Rectangle>
            {
                new Rectangle(new Point(quadrant.LowerLeft.X, bestVerticalCutData.CutPosition), new Point(bestHorizontalCutData.CutPosition, quadrant.UpperRight.Y)), // Upper left
                new Rectangle(new Point(bestHorizontalCutData.CutPosition, bestVerticalCutData.CutPosition), quadrant.UpperRight), // Upper right
                new Rectangle(new Point(bestHorizontalCutData.CutPosition, quadrant.LowerLeft.Y), new Point(quadrant.UpperRight.X, bestVerticalCutData.CutPosition)), // Lower right
                new Rectangle(quadrant.LowerLeft, new Point(bestHorizontalCutData.CutPosition, bestVerticalCutData.CutPosition)) // Lower left
            };

            //Just for testing now
            int totalIntersections = 0;
            foreach (var item in items)
            {
                foreach (var subdiv in combinedQuadrants)
                {
                    if (subdiv.OverlapsBorder(item.Item))
                    {
                        totalIntersections++;
                    }
                }
            }

            return new SubdivisionResult { Quadrants = combinedQuadrants.ToArray(), TotalIntersections = totalIntersections };
        }

        private static List<double> GetCutPositions(Rectangle quadrant, int portions, bool isHorizontal)
        {
            double length = isHorizontal ? quadrant.UpperRight.X - quadrant.LowerLeft.X : quadrant.UpperRight.Y - quadrant.LowerLeft.Y;
            double offset = isHorizontal ? quadrant.LowerLeft.X : quadrant.LowerLeft.Y;
            double step = length / portions;

            List<double> positions = new List<double>();

            for (int i = 1; i < portions; i++)
            {
                positions.Add(offset + step * i);
            }

            return positions;
        }


        private static List<double> GetCutPositions(double length, int portions)
        {
            double step = length / portions;
            List<double> positions = new List<double>();

            for (int i = 1; i < portions; i++)
            {
                positions.Add(step * i);
            }

            return positions;
        }

        private static CutData CalculateBestSubdivision(List<double> cutPositions, Rectangle quadrant, List<QuadTreeObject<K, V>> items, bool isHorizontal)
        {
            List<CutData> bestCuts = new List<CutData>();
            int bestIntersectionsCount = int.MaxValue;



            foreach (var cut in cutPositions)
            {
                List<Rectangle> subdivisions = GetSubdivisionsForCut(quadrant, cut, isHorizontal);
                int totalIntersections = 0;

                foreach (var item in items)
                {
                    if (subdivisions[0].OverlapsBorder(item.Item))
                    {
                        totalIntersections++;
                    }
                }

                if (totalIntersections < bestIntersectionsCount)
                {
                    bestIntersectionsCount = totalIntersections;
                    bestCuts.Clear();
                    bestCuts.Add(new CutData
                    {
                        CutPosition = cut,
                        IntersectionsCount = totalIntersections,
                        Quadrants = subdivisions
                    });
                }
                else if (totalIntersections == bestIntersectionsCount)
                {
                    bestCuts.Add(new CutData
                    {
                        CutPosition = cut,
                        IntersectionsCount = totalIntersections,
                        Quadrants = subdivisions
                    });
                }
            }

            // If odd number of best cuts, take the middle one.
            // If even, take one of the two middle values randomly.
            int indexToReturn = bestCuts.Count / 2;
            if (bestCuts.Count % 2 == 0 && new Random().Next(2) == 1)
            {
                indexToReturn--;
            }

            return bestCuts[indexToReturn];
        }


        private static List<Rectangle> GetSubdivisionsForCut(Rectangle quadrant, double cut, bool isHorizontal)
        {
            List<Rectangle> subdivisions = new List<Rectangle>();
            if (isHorizontal)
            {
                // Divide based on X-axis (horizontal)
                subdivisions.Add(new Rectangle(quadrant.LowerLeft, new Point(cut, quadrant.UpperRight.Y)));
                subdivisions.Add(new Rectangle(new Point(cut, quadrant.LowerLeft.Y), quadrant.UpperRight));
            }
            else
            {
                // Divide based on Y-axis (vertical)
                subdivisions.Add(new Rectangle(quadrant.LowerLeft, new Point(quadrant.UpperRight.X, cut)));
                subdivisions.Add(new Rectangle(new Point(quadrant.LowerLeft.X, cut), quadrant.UpperRight));
            }

            return subdivisions;
        }
    }
}
