namespace DynamicHashingDS.Data;
public class DHBlock<T> where T : DHRecord, new()
{
    public int MaxRecordsCount { get; set; }
    public int ValidRecordsCount { get; set; } = 0;
    public List<T> RecordsList { get; set; } = new List<T>();

    public DHBlock(int blockFactor)
    {
        MaxRecordsCount = blockFactor;
    }

    public bool AddRecord(T record)
    {
        if (ValidRecordsCount < MaxRecordsCount)
        {
            RecordsList.Add(record);
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
        foundRecord = null;
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


}

