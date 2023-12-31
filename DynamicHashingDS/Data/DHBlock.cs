﻿using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;

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

    //TODO: If it's overflowBlock or not.
    //TODO: Remember FileBlockManager? Maybe we should use it here too.

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
        if(ValidRecordsCount >= MaxRecordsCount)
        {
            throw new Exception("Too many records to add.");
            return false;
        } 
        else
        {
            foreach (var r in RecordsList)
            {
                if (r.MyEquals((T)record))
                {
                    return false;
                }
            }


            if (ValidRecordsCount < MaxRecordsCount)
            {
                RecordsList.Add(record);
                ValidRecordsCount++;
                return true;
            }
        }

        return false;
    }

    public bool AddRecords(List<IDHRecord<T>> records)
    {
        if (ValidRecordsCount + records.Count <= MaxRecordsCount)
        {
            RecordsList.AddRange(records);
            ValidRecordsCount += records.Count;
            return true;
        }
        throw new Exception("Too many records to add.");
        //return false;
    }

    /// <summary>
    /// TODO: Should rename 
    /// Clears the block, resetting its state.
    /// </summary>
    public void ClearNextAndValid()
    {
        NextBlockAddress = -1;

        ValidRecordsCount = 0;
    }

    public void ClearBlockPreviousNextAndValid()
    {
        BlockAddress = -1;

        PreviousBlockAddress = -1;
        NextBlockAddress = -1;
        ValidRecordsCount = 0;
    }

    /// <summary>
    /// Deletes a specified record from the block.
    /// </summary>
    /// <param name="record">The record to delete.</param>
    /// <returns>The deleted record if it was successfully deleted; otherwise, null.</returns>
    public IDHRecord<T>? Delete(T record)
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
            var deletedRecord = RecordsList[ValidRecordsCount - 1];
            RecordsList.RemoveAt(ValidRecordsCount - 1);
            ValidRecordsCount--;
            return deletedRecord;
        }

        //if(NextBlockAddress != GlobalConstants.InvalidAddress)
        //{
        //    // If the record is not found and there is a next block, the search should continue there.
        //    throw new NotImplementedException("Continuation to next block not implemented.");
        //}

        return null;
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

        //if (NextBlockAddress != GlobalConstants.InvalidAddress)
        //{
        //    // If the record is not found and there is a next block, the search should continue there.
        //    throw new NotImplementedException("Continuation to next block not implemented.");
        //}



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
        return sizeof(int) // ValidRecordsCount
            + sizeof(int) // PreviousBlockAddress
            + sizeof(int) // NextBlockAddress
            + (MaxRecordsCount * recordSize); // 4 bytes for ValidRecordsCount
    }

    /// <summary>
    /// Converts the block to a byte array.
    /// </summary>
    /// <returns>A byte array representing the block.</returns>
    public byte[] ToByteArray()
    {
        byte[] blockBytes = new byte[GetSize()];

        // Serialize the ValidRecordsCount, PreviousBlockAddress, and NextBlockAddress
        Buffer.BlockCopy(BitConverter.GetBytes(ValidRecordsCount), 0, blockBytes, 0, 4); //ValidRecordsCount
        Buffer.BlockCopy(BitConverter.GetBytes(PreviousBlockAddress), 0, blockBytes, 4, 4); //Previous block
        Buffer.BlockCopy(BitConverter.GetBytes(NextBlockAddress), 0, blockBytes, 8, 4); //next Block

        int recordSize = new T().GetSize();
        int offset = 12; // Start after the ValidRecordsCount, PreviousBlockAddress, and NextBlockAddress
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


        ValidRecordsCount = BitConverter.ToInt32(byteArray, 0); //ValidRecordsCount
        PreviousBlockAddress = BitConverter.ToInt32(byteArray, 4); //Previous block
        NextBlockAddress = BitConverter.ToInt32(byteArray, 8); //next Block

        int offset = 12; // Start after the ValidRecordsCount, PreviousBlockAddress, and NextBlockAddress
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



    public void WriteToBinaryFile(FileStream fileStream, int blockAddress)
    {
        byte[] blockBytes = ToByteArray();

        // Assuming fileStream is already open and set up for writing
        // Seek to the specified block address
        fileStream.Seek(blockAddress, SeekOrigin.Begin);

        // Write the block at the current position
        // Write the block at the current position
        BinaryWriter binaryWriter = new BinaryWriter(fileStream, Encoding.Default, leaveOpen: true);
        binaryWriter.Write(blockBytes);

        // Optionally, you can flush the writer to ensure data is written to the file
        //binaryWriter.Flush();
    }

    public void ReadFromBinaryFile(FileStream fileStream, int blockAddress)
    {
        // Seek to the specified block address
        fileStream.Seek(blockAddress, SeekOrigin.Begin);

        // Read the block at the current position
        BinaryReader binaryReader = new BinaryReader(fileStream, Encoding.Default, leaveOpen: true);
        int blockSize = GetSize();
        byte[] blockBytes = binaryReader.ReadBytes(blockSize);

        // Deserialize from the byte array
        FromByteArray(blockBytes);

        // Optionally, you can explicitly dispose of the binaryReader when done
        //binaryReader.Dispose();

    }

    public void ReadBlockInfoFromBinaryFile(FileStream fileStream, int blockAddress)
    {
        fileStream.Seek(blockAddress, SeekOrigin.Begin);

        // Read only the ValidRecordsCount, PreviousBlockAddress, and NextBlockAddress
        BinaryReader binaryReader = new BinaryReader(fileStream, Encoding.Default, leaveOpen: true);
        ValidRecordsCount = binaryReader.ReadInt32();
        PreviousBlockAddress = binaryReader.ReadInt32();
        NextBlockAddress = binaryReader.ReadInt32();

        // Optionally, you can explicitly dispose of the binaryReader when done
        //binaryReader.Dispose();

    }
}

