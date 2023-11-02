using QuadTreeDS.SpatialItems;

namespace QuadTreeDS.QuadTree;
public class QuadTreeOptimalization<K, V> where K : IComparable<K>
{

    // Somewhere in your class, define the scaling factor
    public static readonly double ScalingFactor = 0.1; // 1 = quadratic, 2 = 4 times bigger, 0.5 = 4 times smaller

    public class SubdivisionResult
    {
        public Rectangle[] Quadrants { get; set; }
        public List<QuadTreeObject<K, V>>[] SortedItems { get; set; }
        public double HorizontalCut { get; set; }
        public double VerticalCut { get; set; }
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

        // Lists to sort items based on which quadrant they fall into
        List<QuadTreeObject<K, V>>[] sortedItems = new List<QuadTreeObject<K, V>>[5]
        {
            new List<QuadTreeObject<K, V>>(),
            new List<QuadTreeObject<K, V>>(),
            new List<QuadTreeObject<K, V>>(),
            new List<QuadTreeObject<K, V>>(),
            new List<QuadTreeObject<K, V>>()  // Items that don't fit into any quadrant
        };

        foreach (var item in items)
        {
            bool itemAdded = false;

            for (int i = 0; i < combinedQuadrants.Count; i++)
            {
                if (combinedQuadrants[i].ContainsStrict(item.Item))
                {
                    sortedItems[i].Add(item);
                    itemAdded = true;
                    break;
                }
            }

            if (!itemAdded)
            {
                // The item doesn't fit into any quadrant
                sortedItems[4].Add(item);
            }
        }

        return new SubdivisionResult { 
            Quadrants = combinedQuadrants.ToArray(), 
            HorizontalCut = bestHorizontalCutData.CutPosition,
            VerticalCut = bestVerticalCutData.CutPosition,
            SortedItems = sortedItems
        };
    }

    //make method for sorting quadTreeObject by their key



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

    // Gets subdivisions for a cut, divided based on the specified orientation.
    private static List<Rectangle> GetSubdivisionsForCut(Rectangle quadrant, double cut, bool isHorizontal)
    {
        if (isHorizontal)
        {
            return new List<Rectangle>
            {
                new Rectangle(quadrant.LowerLeft, new Point(cut, quadrant.UpperRight.Y)),
                new Rectangle(new Point(cut, quadrant.LowerLeft.Y), quadrant.UpperRight)
            };
        }
        else
        {
            return new List<Rectangle>
            {
                new Rectangle(quadrant.LowerLeft, new Point(quadrant.UpperRight.X, cut)),
                new Rectangle(new Point(quadrant.LowerLeft.X, cut), quadrant.UpperRight)
            };
        }
    }

    public static void SortByLongestSide<K, V>(List<QuadTreeObject<K, V>> items, bool descending = false)
        where K : IComparable<K>
    {
        items.Sort((a, b) =>
        {
            // Directly call GetLongestSide, no need to check the type.
            double longestSideA = a.Item.GetLongestSide();
            double longestSideB = b.Item.GetLongestSide();

            // If descending is true, multiply by -1 to invert the comparison
            return descending ? longestSideB.CompareTo(longestSideA) : longestSideA.CompareTo(longestSideB);
        });
    }
}