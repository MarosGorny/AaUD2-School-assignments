using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DynamicHashingDS.Data;

public class DummyClass : DHRecord
{
    public int Cislo { get; set; }
    public int ID { get; set; }

    [StringLength(14)]
    public string Text { get; set; }

    public override DHRecord FromByteArray(byte[] byteArray)
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

    public override BitArray GetHash()
    {
        var hashValue = Cislo.GetHashCode();
        var hashValueBytes = BitConverter.GetBytes(hashValue);
        var hash = new BitArray(hashValueBytes);
        return hash;
    }

    public override int GetSize()
    {
        int size = 0;

        size += sizeof(int); // Size for Cislo
        size += sizeof(int); // Size for ID
        size += 14 * sizeof(char); // Size for Text

        return size;
    }

    public override bool MyEquals(DHRecord other)
    {
        if (other is DummyClass otherDummy)
        {
            return this.Cislo == otherDummy.Cislo && this.ID == otherDummy.ID && this.Text == otherDummy.Text;
        }

        return false;
    }

    public override byte[] ToByteArray()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write(Cislo);
            writer.Write(ID);

            // Ensure Text is exactly 14 characters long by padding with null characters if necessary
            string paddedText = Text?.PadRight(14, '\0');
            var textBytes = Encoding.Unicode.GetBytes(paddedText);

            // Check the length of the textBytes to ensure it's 28 bytes
            if (textBytes.Length != 28)
            {
                throw new InvalidOperationException("The encoded text does not meet the expected length of 28 bytes.");
            }

            writer.Write(textBytes);

            return ms.ToArray();
        }
    }
}


public abstract class DHRecord
{
    public abstract int GetSize();

    public abstract BitArray GetHash();
    public abstract bool MyEquals(DHRecord other);

    public abstract byte[] ToByteArray();

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



    public abstract DHRecord FromByteArray(byte[] byteArray);
   
}