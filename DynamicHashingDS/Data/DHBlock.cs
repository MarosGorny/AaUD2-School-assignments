namespace DynamicHashingDS.Data;
public class DHBlock<T> where T : IDHRecord<T>, new()
{
    public int BlockAddress { get; private set; } // Address of the block in the file
    public int MaxRecordsCount { get; private set; }
    public int ValidRecordsCount { get; private set; } = 0;
    public List<IDHRecord<T>> RecordsList { get; private set; } = new List<IDHRecord<T>>();

    public int NextBlockAddress { get; set; } = -1; 
    public int PreviousBlockAddress { get; set; } = -1; 

    public DHBlock(int blockFactor)
    {
        MaxRecordsCount = blockFactor;
    }

    public DHBlock(int blockFactor, int blockAddress)
    {
        MaxRecordsCount = blockFactor;
        BlockAddress = blockAddress;
    }

    public bool AddRecord(IDHRecord<T> record)
    {
        if (ValidRecordsCount < MaxRecordsCount)
        {
            RecordsList.Add(record);
            ValidRecordsCount++;
            return true;
        }
        return false;
    }

    public void Clear()
    {
        NextBlockAddress = -1;

        ValidRecordsCount = 0;
    }

    public bool DeleteRecord(T record)
    {
        var foundIndex = RecordsList.FindIndex(r => r.MyEquals(record));
        if (foundIndex != -1)
        {
            // Swap with the last valid record if it's not already the last one
            if (foundIndex != ValidRecordsCount - 1)
            {
                var temp = RecordsList[foundIndex];
                RecordsList[foundIndex] = RecordsList[ValidRecordsCount - 1];
                RecordsList[ValidRecordsCount - 1] = temp;
            }

            // Remove the last record (which is the record to be deleted)
            RecordsList.RemoveAt(ValidRecordsCount - 1);
            ValidRecordsCount--;
            return true;
        }
        return false;
    }

    public bool TryFind(T record, out IDHRecord<T>? foundRecord)
    {
        foreach (var r in RecordsList)
        {
            if (r.MyEquals(record))
            {
                foundRecord = r;
                return true;
            }
        }
        foundRecord = default; 
        return false;
    }

    public int GetSize()
    {
        int recordSize = new T().GetSize();
        return 4 + (MaxRecordsCount * recordSize); // 4 bytes for ValidRecordsCount
    }

    public byte[] ToByteArray()
    {
        byte[] blockBytes = new byte[GetSize()];

        // Serialize the ValidRecordsCount
        Buffer.BlockCopy(BitConverter.GetBytes(ValidRecordsCount), 0, blockBytes, 0, 4);

        int recordSize = new T().GetSize();
        // Serialize the valid records
        int offset = 4; // Start after the ValidRecordsCount
        for (int i = 0; i < ValidRecordsCount; i++)
        {
            byte[] recordBytes = RecordsList[i].ToByteArray();
            Buffer.BlockCopy(recordBytes, 0, blockBytes, offset, recordSize);
            offset += recordSize;
        }

        return blockBytes;
    }

    public void FromByteArray(byte[] byteArray)
    {
        int recordSize = new T().GetSize();

        // Deserialize the ValidRecordsCount
        ValidRecordsCount = BitConverter.ToInt32(byteArray, 0);

        // Deserialize the valid records
        int offset = 4; // Start after the ValidRecordsCount
        RecordsList.Clear();
        for (int i = 0; i < ValidRecordsCount; i++)
        {
            byte[] recordBytes = new byte[recordSize];
            Buffer.BlockCopy(byteArray, offset, recordBytes, 0, recordSize);
            T record = new T().FromByteArray(recordBytes);
            RecordsList.Add(record);
            offset += recordSize;
        }
    }

    public void WriteToBinaryFile(string filePath, int blockAddress)
    {
        byte[] blockBytes = ToByteArray();

        using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
            {
                // Seek to the specified block address
                fileStream.Seek(blockAddress, SeekOrigin.Begin);

                // Write the block at the current position
                binaryWriter.Write(blockBytes);
            }
        }
    }

    public void ReadFromBinaryFile(string filePath, int blockAddress)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {
                // Seek to the specified block address
                fileStream.Seek(blockAddress, SeekOrigin.Begin);

                // Read the block at the current position
                int blockSize = GetSize(); 
                byte[] blockBytes = binaryReader.ReadBytes(blockSize);

                // Deserialize from the byte array
                FromByteArray(blockBytes);
            }
        }
    }


    //public void ReadFromBinaryFile(string filePath)
    //{
    //    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
    //    {
    //        using (BinaryReader binaryReader = new BinaryReader(fileStream))
    //        {
    //            // Read the entire block into a byte array
    //            byte[] blockBytes = binaryReader.ReadBytes((int)fileStream.Length);

    //            // Deserialize from the byte array
    //            FromByteArray(blockBytes);
    //        }
    //    }
    //}
}

