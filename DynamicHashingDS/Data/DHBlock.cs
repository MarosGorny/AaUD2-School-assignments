namespace DynamicHashingDS.Data;
public class DHBlock<T> where T : IDHRecord, new()
{
    public int MaxRecordsCount { get; set; }
    public int ValidRecordsCount { get; set; } = 0;
    public List<T> RecordsList { get; set; } = new List<T>();

    public DHBlock(int blockFactor)
    {
        MaxRecordsCount = blockFactor;
    }

    public bool AddRecord(IDHRecord record)
    {
        if (ValidRecordsCount < MaxRecordsCount)
        {
            RecordsList.Add((T)record);
            ValidRecordsCount++;
            return true;
        }

        return false;
    }


    public bool TryFind(T record, out T? foundRecord)
    {
        foreach (var r in RecordsList)
        {
            if (r.MyEquals(record))
            {
                foundRecord = r;
                return true;
            }
        }
        foundRecord = default; //TODO: What it really does?
        return false;
    }

    public int GetSize()
    {
        return MaxRecordsCount * new T().GetSize();
    }

    public byte[] ToByteArray()
    {
        byte[] blockBytes = new byte[GetSize()];
        int offset = 0;

        foreach (var record in RecordsList)
        {
            byte[] recordBytes = record.ToByteArray();
            Buffer.BlockCopy(recordBytes, 0, blockBytes, offset, recordBytes.Length);
            offset += recordBytes.Length;
        }

        return blockBytes;
    }

    public T FromByteArray(byte[] byteArray)
    {
        T record = new T();
        record.FromByteArray(byteArray);
        return record;
    }
    //TODO: Vediet ulozit a nacitat spatne z binarneho suboru

    public void WriteToBinaryFile(string filePath)
    {
        byte[] blockBytes = ToByteArray();

        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
            {
                binaryWriter.Write(blockBytes);
            }
        }
    }

    public void ReadFromBinaryFile(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {
                RecordsList.Clear(); 
                ValidRecordsCount = 0;

                while (fileStream.Position < fileStream.Length)
                {
                    byte[] recordBytes = binaryReader.ReadBytes(new T().GetSize());
                    T record = FromByteArray(recordBytes);
                    if (record != null)
                    {
                        RecordsList.Add(record);
                        ValidRecordsCount++;
                    }
                }
            }
        }
    }

}

