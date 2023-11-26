using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DynamicHashingDS.Data;

public class DummyClass : IDHRecord<DummyClass>
{
    public int Cislo { get; set; }
    public int ID { get; set; }

    [StringLength(14)]
    public string Text { get; set; } = "";

    public DummyClass FromByteArray(byte[] byteArray)
    {
        using (var ms = new MemoryStream(byteArray))
        using (var reader = new BinaryReader(ms))
        {
            this.Cislo = reader.ReadInt32();
            this.ID = reader.ReadInt32();

            // Read the Text as a UTF-16 encoded string
            var textBytes = reader.ReadBytes(14 * sizeof(char));
            this.Text = Encoding.Unicode.GetString(textBytes).TrimEnd('\0', ' '); // Trimming any padding
        }

        return this;
    }

    public BitArray GetHash()
    {
        //var hashValue = Cislo.GetHashCode();
        //var hashValueBytes = BitConverter.GetBytes(hashValue);
        //var hash = new BitArray(hashValueBytes);
        //return hash;

        var hashValue = Cislo.GetHashCode();
        var hashValueBytes = BitConverter.GetBytes(hashValue);
        var fullHash = new BitArray(hashValueBytes);

        // Create a new BitArray with a size of 3 bits
        var limitedHash = new BitArray(3);

        // Copy the first 3 bits from the full hash to the limited hash
        for (int i = 0; i < 3; i++)
        {
            limitedHash[i] = fullHash[i];
        }

        return limitedHash;
    }

    public int GetSize()
    {
        int size = 0;

        size += sizeof(int); // Size for Cislo
        size += sizeof(int); // Size for ID
        size += 14 * sizeof(char); // Size for Text

        return size;
    }

    public string GetHashString()
    {
        var hash = GetHash();
        var sb = new StringBuilder();
        int counter = 0;
        for (int i = hash.Length - 1; i >= 0; i--)
        {
            sb.Append(hash[i] ? "1" : "0");
            if(++counter % 8 == 0)
            {
                sb.Append(" ");
            }
        }

        return sb.ToString();
    }

    public override string ToString()
    {
        return $"DummyClass(Hash: {GetHashString()},Cislo: {Cislo}, ID: {ID}, Text: {Text})";
    }

    public bool MyEquals(DummyClass other)
    {
        if (other is DummyClass otherDummy)
        {
            return this.Cislo == otherDummy.Cislo && this.ID == otherDummy.ID && this.Text == otherDummy.Text;
        }

        return false;
    }

    public byte[] ToByteArray()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write(Cislo);
            writer.Write(ID);

            byte[] textBytes;
            if (Text != null)
            {
                string paddedText = Text.PadRight(14, '\0');
                textBytes = Encoding.Unicode.GetBytes(paddedText);
                if (textBytes.Length != 28)
                {
                    throw new InvalidOperationException("The encoded text does not meet the expected length of 28 bytes.");
                }
            }
            else
            {
                textBytes = new byte[28]; // Create an array of 28 null bytes
            }

            writer.Write(textBytes);

            return ms.ToArray();
        }
    }
}


/// <summary>
/// Defines the required functionalities for a record to be used in dynamic hashing.
/// </summary>
/// <typeparam name="T">The type implementing this interface.</typeparam>
public interface IDHRecord<T> where T : IDHRecord<T>
{

    /// <summary>
    /// Gets the size of the record in bytes.
    /// </summary>
    /// <returns>The size of the record.</returns>
    int GetSize();

    /// <summary>
    /// Gets the hash representation of the record.
    /// </summary>
    /// <returns>A BitArray representing the hash of the record.</returns>
    BitArray GetHash();

    /// <summary>
    /// Determines whether the current record is equal to another record of the same type.
    /// </summary>
    /// <param name="other">The record to compare with this record.</param>
    /// <returns>True if the specified record is equal to the current record; otherwise, false.</returns>
    bool MyEquals(T other);

    /// <summary>
    /// Converts the record into a byte array.
    /// </summary>
    /// <returns>A byte array representing the current record.</returns>
    byte[] ToByteArray();

    /// <summary>
    /// Creates a record from a byte array.
    /// </summary>
    /// <param name="byteArray">The byte array to create the record from.</param>
    /// <returns>A new record instance created from the byte array.</returns>
    T FromByteArray(byte[] byteArray);
}