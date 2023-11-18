namespace QuadTreeDS.SpatialItems;
public static class SpatialItemExtensions
{
    public static bool ContainsStrict(this ISpatialItem item, ISpatialItem other)
    {
        return item.LowerLeft.X <= other.LowerLeft.X &&
               item.LowerLeft.Y <= other.LowerLeft.Y &&
               item.UpperRight.X >= other.UpperRight.X &&
               item.UpperRight.Y >= other.UpperRight.Y;
    }

    public static bool Intersects(this ISpatialItem item, ISpatialItem other)
    {
        return item.LowerLeft.X <= other.UpperRight.X &&
               item.UpperRight.X >= other.LowerLeft.X &&
               item.LowerLeft.Y <= other.UpperRight.Y &&
               item.UpperRight.Y >= other.LowerLeft.Y;
    }

    public static bool OverlapsBorder(this ISpatialItem item, ISpatialItem other)
    {
        // Check if the two items intersect in general.
        bool generalIntersection = item.Intersects(other);

        if (generalIntersection)
        {
            // Check if one item is fully contained within the other.
            bool thisContainsOther = item.ContainsStrict(other);
            bool otherContainsThis = other.ContainsStrict(item);

            // Return true only if neither item fully contains the other (indicating a border overlap).
            return !(thisContainsOther || otherContainsThis);
        }
        return false;
    }
}