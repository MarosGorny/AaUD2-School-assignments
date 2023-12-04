using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DynamicHashingDS.Data;

public class Osoba : IDHRecord<Osoba>
{
    [StringLength(10)]
    public string Meno { get; set; } = "";
    [StringLength(12)]
    public string Priezvisko { get; set; } = "";
    [StringLength(15)]
    public string Popis { get; set; } = "";

    public Osoba FromByteArray(byte[] byteArray)
    {
        using (var ms = new MemoryStream(byteArray))
        using (var reader = new BinaryReader(ms))
        {
            // Read the Text as a UTF-16 encoded string
            var menoTextBytes = reader.ReadBytes(10 * sizeof(char));
            this.Meno = Encoding.Unicode.GetString(menoTextBytes).TrimEnd('\0', ' '); // Trimming any padding

            var priezviskoTextBytes = reader.ReadBytes(12 * sizeof(char));
            this.Priezvisko = Encoding.Unicode.GetString(priezviskoTextBytes).TrimEnd('\0', ' '); // Trimming any padding

            var popisTextBytes = reader.ReadBytes(15 * sizeof(char));
            this.Popis = Encoding.Unicode.GetString(popisTextBytes).TrimEnd('\0', ' '); // Trimming any padding
        }

        return this;
    }

    public BitArray GetHash()
    {
        var MenoPriezvisko = Meno + Priezvisko;

        var hashValue = MenoPriezvisko.GetHashCode();
        var hashValueBytes = BitConverter.GetBytes(hashValue);
        var hash = new BitArray(hashValueBytes);
        return hash;
    }

    public int GetSize()
    {
        int size = 0;

        size += 10 * sizeof(char); // Size for Meno
        size += 12 * sizeof(char); // Size for Priezvisko
        size += 15 * sizeof(char); // Size for Popis

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
            if (++counter % 8 == 0)
            {
                sb.Append(" ");
            }
        }

        return sb.ToString();
    }

    public override string ToString()
    {
        return $"Osoba(Hash: {GetHashString()},Meno: {Meno}, Priezvisko: {Priezvisko}, Popis: {Popis})";
    }

    public bool MyEquals(Osoba other)
    {
        if (other is Osoba otherOsoba)
        {
            return this.Meno == otherOsoba.Meno && this.Priezvisko == otherOsoba.Priezvisko;
        }

        return false;
    }

    public byte[] ToByteArray()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            byte[] textBytesMeno;
            if (Meno != null)
            {
                string paddedText = Meno.PadRight(10, '\0');
                textBytesMeno = Encoding.Unicode.GetBytes(paddedText);
                if (textBytesMeno.Length != 20)
                {
                    throw new InvalidOperationException("The encoded text does not meet the expected length of 20 bytes.");
                }
            }
            else
            {
                textBytesMeno = new byte[20]; // Create an array of 20 null bytes
            }
            writer.Write(textBytesMeno);

            byte[] textBytesPriezvisko;
            if (Priezvisko != null)
            {
                string paddedText = Priezvisko.PadRight(12, '\0');
                textBytesPriezvisko = Encoding.Unicode.GetBytes(paddedText);
                if (textBytesPriezvisko.Length != 24)
                {
                    throw new InvalidOperationException("The encoded text does not meet the expected length of 24 bytes.");
                }
            }
            else
            {
                textBytesPriezvisko = new byte[24]; // Create an array of 24 null bytes
            }
            writer.Write(textBytesPriezvisko);

            byte[] textBytesPopis;
            if (Popis != null)
            {
                string paddedText = Popis.PadRight(15, '\0');
                textBytesPopis = Encoding.Unicode.GetBytes(paddedText);
                if (textBytesPopis.Length != 30)
                {
                    throw new InvalidOperationException("The encoded text does not meet the expected length of 30 bytes.");
                }
            }
            else
            {
                textBytesPopis = new byte[30]; // Create an array of 30 null bytes
            }
            writer.Write(textBytesPopis);
            return ms.ToArray();
        }
    }
}

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
        var hashValue = Cislo.GetHashCode();
        var hashValueBytes = BitConverter.GetBytes(hashValue);
        var hash = new BitArray(hashValueBytes);
        return hash;
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