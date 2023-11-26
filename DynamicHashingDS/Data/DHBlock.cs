using System.Reflection.Metadata.Ecma335;

namespace DynamicHashingDS.Data;

/// <summary>
/// Represents a block in a dynamic hashing data structure.
/// </summary>
/// <typeparam name="T">The type of records stored in the block.</typeparam>
public class DHBlock<T> where T : IDHRecord<T>, new()
{
    public int BlockAddress { get; private set; } //TODO: Do we need this?
    public int MaxRecordsCount { get; private set; }
    public int ValidRecordsCount { get; private set; } = 0;
    public List<IDHRecord<T>> RecordsList { get; private set; } = new List<IDHRecord<T>>();
    public int NextBlockAddress { get; set; } = -1; 
    public int PreviousBlockAddress { get; set; } = -1;


    /// <summary>
    /// Initializes a new instance of the DHBlock class with a specified block factor.
    /// </summary>
    /// <param name="blockFactor">The block factor indicating the maximum number of records.</param>
    public DHBlock(int blockFactor)
    {
        MaxRecordsCount = blockFactor;
    }

    /// <summary>
    /// Initializes a new instance of the DHBlock class with a specified block factor and block address.
    /// </summary>
    /// <param name="blockFactor">The block factor indicating the maximum number of records.</param>
    /// <param name="blockAddress">The block address within the file.</param>
    public DHBlock(int blockFactor, int blockAddress)
    {
        MaxRecordsCount = blockFactor;
        BlockAddress = blockAddress;
    }

    /// <summary>
    /// Adds a record to the block.
    /// </summary>
    /// <param name="record">The record to add.</param>
    /// <returns>True if the record was successfully added; otherwise, false.</returns>
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

    /// <summary>
    /// TODO: Should rename 
    /// Clears the block, resetting its state.
    /// </summary>
    public void Clear()
    {
        NextBlockAddress = -1;

        ValidRecordsCount = 0;
    }

    /// <summary>
    /// Deletes a specified record from the block.
    /// </summary>
    /// <param name="record">The record to delete.</param>
    /// <returns>True if the record was successfully deleted; otherwise, false.</returns>
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

    /// <summary>
    /// Attempts to find a record in the block. If the record is not found in the current block,
    /// and there is a linked next block, the search continues in the next block.
    /// </summary>
    /// <param name="record">The record to find.</param>
    /// <param name="foundRecord">When this method returns, contains the found record if it exists; otherwise null.</param>
    /// <returns>True if a record was found in the current block or any linked next block; otherwise, false.</returns>
    public bool TryFind(IDHRecord<T> record, out IDHRecord<T>? foundRecord)
    {
        foreach (var r in RecordsList)
        {
            if (r.MyEquals((T)record))
            {
                foundRecord = r;
                return true; 
            }
        }

        if (NextBlockAddress != GlobalConstants.InvalidAddress)
        {
            // If the record is not found and there is a next block, the search should continue there.
            throw new NotImplementedException("Continuation to next block not implemented.");
        }

        foundRecord = null;
        return false;
    }


    /// <summary>
    /// Gets the size of the block in bytes.
    /// </summary>
    /// <returns>The size of the block.</returns>
    public int GetSize()
    {
        int recordSize = new T().GetSize();
        return 4 + (MaxRecordsCount * recordSize); // 4 bytes for ValidRecordsCount
    }

    /// <summary>
    /// Converts the block to a byte array.
    /// </summary>
    /// <returns>A byte array representing the block.</returns>
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

    /// <summary>
    /// Initializes the block from a byte array.
    /// </summary>
    /// <param name="byteArray">The byte array to initialize from.</param>
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

    /// <summary>
    /// Writes the block to a binary file at a specified address.
    /// </summary>
    /// <param name="filePath">The file path to write to.</param>
    /// <param name="blockAddress">The address at which to write the block.</param>
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

    /// <summary>
    /// Reads a block from a binary file at a specified address.
    /// </summary>
    /// <param name="filePath">The file path to read from.</param>
    /// <param name="blockAddress">The address at which to read the block.</param>
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
}

