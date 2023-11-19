using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DynamicHashingDS.Data;

public class DummyClass : IDHRecord
{
    public int Cislo { get; set; }
    public int ID { get; set; }

    [StringLength(14)]
    public string Text { get; set; }

    public IDHRecord FromByteArray(byte[] byteArray)
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

    public bool MyEquals(IDHRecord other)
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


public interface IDHRecord
{
    int GetSize();

    BitArray GetHash();
    bool MyEquals(IDHRecord other);

    byte[] ToByteArray();

    IDHRecord FromByteArray(byte[] byteArray);

    // Method to serialize the record to a byte array
    //public byte[] ToByteArray()
    //{
    //    using (var memoryStream = new MemoryStream())
    //    {
    //        using (var binaryWriter = new BinaryWriter(memoryStream))
    //        {
    //            try
    //            {
    //                binaryWriter.Write(NejakyAtributInt);
    //            }
    //            catch (IOException e)
    //            {
    //                throw new InvalidOperationException("Error during conversion to byte array.", e);
    //            }

    //            return memoryStream.ToArray();
    //        }
    //    }
    //}   
}