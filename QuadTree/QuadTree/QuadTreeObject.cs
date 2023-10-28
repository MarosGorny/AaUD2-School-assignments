using QuadTreeDS.SpatialItems;

namespace QuadTreeDS.QuadTree
{
    public class QuadTreeObject<K, V> where K : IComparable<K>
    {
        public K Key { get; set; }
        public V Value { get; set; }

        public SpatialItem Item { get; set; }

        public QuadTreeObject(K key, V value, SpatialItem item)
        {
            Key = key;
            Value = value;
            Item = item;
        }

        #region Comparison based on Key
        public static bool operator ==(QuadTreeObject<K, V> left, QuadTreeObject<K, V> right)
        {
            return left.Key.CompareTo(right.Key) == 0;
        }

        public static bool operator !=(QuadTreeObject<K, V> left, QuadTreeObject<K, V> right)
        {
            return !(left == right);
        }

        public static bool operator <(QuadTreeObject<K, V> left, QuadTreeObject<K, V> right)
        {
            return left.Key.CompareTo(right.Key) < 0;
        }

        public static bool operator >(QuadTreeObject<K, V> left, QuadTreeObject<K, V> right)
        {
            return left.Key.CompareTo(right.Key) > 0;
        }

        public override bool Equals(object? obj)
        {
            if (obj is QuadTreeObject<K, V> other)
            {
                return Key.CompareTo(other.Key) == 0;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
        #endregion
    }
}
