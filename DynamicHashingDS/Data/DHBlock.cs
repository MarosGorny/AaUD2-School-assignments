namespace DynamicHashingDS.Data;
public class DHBlock<T> where T : DHRecord, new()
{

    public DHBlock<T> NextBlock { get; set; }
    public DHBlock<T> PreviousBlock { get; set; }
    public int MaxRecordsCount { get; set; }
    public int ValidRecordsCount { get; set; }
    public LinkedList<T> RecordsList { get; set; }

    public DHBlock(int blockFactor)
    {
        RecordsList = new LinkedList<T>();
        ValidRecordsCount = 0;
        MaxRecordsCount = blockFactor;

        for (int i = 0; i < MaxRecordsCount; i++)
        {
            RecordsList.AddLast(new T());
        }
    }

    public bool AddRecord(T record)
    {
        if (ValidRecordsCount < MaxRecordsCount)
        {
            RecordsList.AddFirst(record);
            RecordsList.RemoveLast();
            ValidRecordsCount++;
            return true;
        }

        return false;
    }


    public bool Find(T record)
    {
        foreach (var r in RecordsList)
        {
            if (r.MyEquals(record))
            {
                return true;
            }
        }

        return false;
    }

    public int GetSize()
    {
        return RecordsList.Count * RecordsList.First.Value.GetSize();
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

