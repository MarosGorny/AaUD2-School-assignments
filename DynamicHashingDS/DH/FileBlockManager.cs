using DynamicHashingDS.Data;
using System.Text;

namespace DynamicHashingDS.DH;

/// <summary>
/// Manages the file blocks for dynamic hashing, handling operations such as retrieving, releasing, and appending blocks.
/// </summary>
/// <typeparam name="T">The type of records stored in the dynamic hashing structure.</typeparam>
public class FileBlockManager<T> where T : IDHRecord<T>, new()
{
    public string MainFilePath { get; private set;}
    public string OverflowFilePath { get; private set;}

    public int CurrentMainFileSize { get; private set;}
    public int CurrentOverflowFileSize { get; private set;}

    private int firstFreeBlockMainFile; 
    private int firstFreeBlockOverflowFile; 

    public int MainFileBlockFactor { get; private set;} 
    public int OverflowFileBlockFactor { get; private set;}

    /// <summary>
    /// Initializes a new instance of the FileBlockManager class.
    /// </summary>
    /// <param name="mainFilePath">Path to the main file.</param>
    /// <param name="overflowFilePath">Path to the overflow file.</param>
    /// <param name="mainFileBlockFactor">Block factor for the main file.</param>
    /// <param name="overflowFileBlockFactor">Block factor for the overflow file.</param>
    public FileBlockManager(string mainFilePath, string overflowFilePath, int mainFileBlockFactor, int overflowFileBlockFactor)
    {
        this.MainFilePath = mainFilePath;
        this.OverflowFilePath = overflowFilePath;
        this.firstFreeBlockMainFile = -1; 
        this.firstFreeBlockOverflowFile = -1; 
        this.CurrentMainFileSize = 0; 
        this.CurrentOverflowFileSize = 0;
        this.MainFileBlockFactor = mainFileBlockFactor;
        this.OverflowFileBlockFactor = overflowFileBlockFactor;
    }

    /// <summary>
    /// Retrieves a free block from either the main file or overflow file. If no free block is available, a new block is appended.
    /// </summary>
    /// <param name="isOverflow">Specifies whether to search in the overflow file.</param>
    /// <returns>The address of the free block.</returns>
    public int GetFreeBlock(bool isOverflow)
    {
        int firstFreeBlockAdress = isOverflow ? firstFreeBlockOverflowFile : firstFreeBlockMainFile;
        if (firstFreeBlockAdress != -1)
        {
            return HandleExistingFreeBlock(isOverflow, firstFreeBlockAdress);
        }
        else
        {
            return AppendBlock(isOverflow);
        }
    }

    /// <summary>
    /// Releases a block, adding it to the list of free blocks or shrinking the file if it's at the end.
    /// </summary>
    /// <param name="block">The block to be released.</param>
    /// <param name="isOverflow">Specifies whether the block is from the overflow file.</param>
    public void ReleaseBlock(DHBlock<T> block, bool isOverflow)
    {
        int firstFreeBlockAddress = GetFirstFreeBlockAddress(isOverflow);
        int fileSize = GetCurrentFileSize(isOverflow);
        string filePath = GetFilePath(isOverflow);
        int recordSize = GetRecordSize(isOverflow);

        // Shrink the file if the block is at the end of the file
        if (IsBlockAtEndOfFile(block, fileSize, recordSize))
        {
            if(isOverflow && block.PreviousBlockAddress != -1)
            {
                bool previousIsMain = false;
                try
                {
                    DHBlock<T> previousBlockTry = new DHBlock<T>(MainFileBlockFactor, block.PreviousBlockAddress);
                    previousBlockTry.ReadFromBinaryFile(MainFilePath, block.PreviousBlockAddress);
                    if(previousBlockTry.NextBlockAddress == block.BlockAddress)
                    {
                        previousIsMain = true;
                    }
                } 
                catch
                {
                    previousIsMain = false;
                }

                DHBlock<T> previousBlock;

                if(previousIsMain)
                {
                    previousBlock = new DHBlock<T>(MainFileBlockFactor, block.PreviousBlockAddress);
                    previousBlock.ReadFromBinaryFile(MainFilePath, block.PreviousBlockAddress);
                }
                else
                {
                    previousBlock = new DHBlock<T>(OverflowFileBlockFactor, block.PreviousBlockAddress);
                    previousBlock.ReadFromBinaryFile(OverflowFilePath, block.PreviousBlockAddress);
                }

                previousBlock.NextBlockAddress = -1;

                if(previousIsMain)
                {
                    WriteBlockToFile(previousBlock, GetFilePath(false));
                } 
                else
                {
                    WriteBlockToFile(previousBlock, GetFilePath(true));
                }

            }
            ShrinkFile(isOverflow, block.BlockAddress);
        }
        else
        {
            HandleBlockRelease(block, isOverflow, firstFreeBlockAddress, filePath);
        }
    }

    /// <summary>
    /// Retrieves all records from either the main file or the overflow file.
    /// </summary>
    /// <param name="fromOverflowFile">Specifies whether to retrieve from the overflow file.</param>
    /// <returns>A list of all records retrieved.</returns>
    public List<IDHRecord<T>> GetAllRecords(bool fromOverflowFile)
    {
        string filePath = fromOverflowFile ? OverflowFilePath : MainFilePath;
        List<IDHRecord<T>> allRecords = new List<IDHRecord<T>>();
        int blockFactor = fromOverflowFile ? OverflowFileBlockFactor : MainFileBlockFactor;
        long fileSize = new FileInfo(filePath).Length;

        for (int currentAddress = 0; currentAddress < fileSize;)
        {
            DHBlock<T> block = new DHBlock<T>(blockFactor, currentAddress);
            block.ReadFromBinaryFile(filePath, currentAddress);
            allRecords.AddRange(block.RecordsList);

            currentAddress += block.GetSize();
        }

        return allRecords;
    }

    private int HandleExistingFreeBlock(bool isOverflow, int blockAddress)
    {
        // Initialize the free block
        DHBlock<T> freeBlock = InitializeBlock(isOverflow, blockAddress);
        freeBlock.ReadFromBinaryFile(GetFilePath(isOverflow), blockAddress);

        // Check if there is a next free block
        if (freeBlock.NextBlockAddress == -1)
        {
            ClearBlockAndResetFirstFreeBlock(isOverflow, freeBlock);
        }
        else
        {
            UpdateLinkToNextFreeBlock(isOverflow, freeBlock);
        }

        return blockAddress;
    }

    private DHBlock<T> InitializeBlock(bool isOverflow, int blockAddress)
    {
        int blockFactor = isOverflow ? OverflowFileBlockFactor : MainFileBlockFactor;
        return new DHBlock<T>(blockFactor, blockAddress);
    }

    private string GetFilePath(bool isOverflow)
    {
        return isOverflow ? OverflowFilePath : MainFilePath;
    }

    private int GetFirstFreeBlockAddress(bool isOverflow)
    {
        return isOverflow ? firstFreeBlockOverflowFile : firstFreeBlockMainFile;
    }

    private int GetCurrentFileSize(bool isOverflow)
    {
        return isOverflow ? CurrentOverflowFileSize : CurrentMainFileSize;
    }

    private int GetRecordSize(bool isOverflow)
    {
        int blockFactor = isOverflow ? OverflowFileBlockFactor : MainFileBlockFactor;
        return new DHBlock<T>(blockFactor).GetSize();
    }

    private bool IsBlockAtEndOfFile(DHBlock<T> block, int fileSize, int recordSize)
    {
        return block.BlockAddress == fileSize - recordSize;
    }

    private void ClearBlockAndResetFirstFreeBlock(bool isOverflow, DHBlock<T> block)
    {
        block.ClearNextAndValid();

        if (isOverflow)
        {
            firstFreeBlockOverflowFile = -1;
        }
        else
        {
            firstFreeBlockMainFile = -1;
        }

        WriteBlockToFile(block, GetFilePath(isOverflow));
    }

    private void UpdateLinkToNextFreeBlock(bool isOverflow, DHBlock<T> currentFreeBlock)
    {
        DHBlock<T> nextFreeBlock = InitializeBlock(isOverflow, currentFreeBlock.NextBlockAddress);
        nextFreeBlock.ReadFromBinaryFile(GetFilePath(isOverflow), currentFreeBlock.NextBlockAddress);

        nextFreeBlock.PreviousBlockAddress = -1;
        WriteBlockToFile(nextFreeBlock, GetFilePath(isOverflow));
    }

    private int AppendBlock(bool isOverflow)
    {
        
        int originalAddress = isOverflow ? CurrentOverflowFileSize : CurrentMainFileSize;

        int blockSize = isOverflow ? new DHBlock<T>(OverflowFileBlockFactor, -1).GetSize() : new DHBlock<T>(MainFileBlockFactor, -1).GetSize();
        IncreaseFileSize(isOverflow, blockSize);
        return originalAddress;
    }

    private void IncreaseFileSize(bool isOverflow, int increaseBy)
    {
        if (isOverflow)
        {
            CurrentOverflowFileSize += increaseBy;
        }
        else
        {
            CurrentMainFileSize += increaseBy;
        }
    }

    private void HandleBlockRelease(DHBlock<T> block, bool isOverflow, int firstFreeBlockAddress, string filePath)
    {
        if (firstFreeBlockAddress == -1)
        {
            if(block.PreviousBlockAddress != -1)
            {
                bool previousIsMain = false;
                try
                {
                    DHBlock<T> previousBlockTry = new DHBlock<T>(MainFileBlockFactor, block.PreviousBlockAddress);
                    previousBlockTry.ReadFromBinaryFile(MainFilePath, block.PreviousBlockAddress);
                    if (previousBlockTry.NextBlockAddress == block.BlockAddress)
                    {
                        previousIsMain = true;
                    }
                }
                catch
                {
                    previousIsMain = false;
                }

                DHBlock<T> previousBlock;

                if (previousIsMain)
                {
                    previousBlock = new DHBlock<T>(MainFileBlockFactor, block.PreviousBlockAddress);
                    previousBlock.ReadFromBinaryFile(MainFilePath, block.PreviousBlockAddress);
                }
                else
                {
                    previousBlock = new DHBlock<T>(OverflowFileBlockFactor, block.PreviousBlockAddress);
                    previousBlock.ReadFromBinaryFile(OverflowFilePath, block.PreviousBlockAddress);
                }

                previousBlock.NextBlockAddress = block.NextBlockAddress;

                if (previousIsMain)
                {
                    WriteBlockToFile(previousBlock, GetFilePath(false));
                }
                else
                {
                    WriteBlockToFile(previousBlock, GetFilePath(true));
                }

                if(block.NextBlockAddress != -1)
                {
                    var nextBlock = new DHBlock<T>(OverflowFileBlockFactor, block.NextBlockAddress);
                    nextBlock.ReadFromBinaryFile(OverflowFilePath, block.NextBlockAddress);
                    nextBlock.PreviousBlockAddress = block.PreviousBlockAddress;

                }
            }

            var oldAddress = block.BlockAddress;
            AddBlockToFreeList(block, isOverflow);
            block.WriteToBinaryFile(filePath, oldAddress);
            //WriteBlockToFile(block, filePath);
        }
        else
        {
            UpdateFreeBlockList(block, isOverflow, firstFreeBlockAddress, filePath);
        }
    }

    private void UpdateFreeBlockList(DHBlock<T> blockToRelease, bool isOverflow, int firstFreeBlockAddress, string filePath)
    {
        int blockFactor = isOverflow ? OverflowFileBlockFactor : MainFileBlockFactor;

        if (firstFreeBlockAddress > blockToRelease.BlockAddress)
        {
            InsertBlockBeforeFirstFreeBlock(blockToRelease, firstFreeBlockAddress, filePath, blockFactor);
        }
        else if (firstFreeBlockAddress < blockToRelease.BlockAddress)
        {
            InsertBlockAfterFirstFreeBlock(blockToRelease, firstFreeBlockAddress, filePath, blockFactor);
        }
    }

    private void InsertBlockBeforeFirstFreeBlock(DHBlock<T> blockToRelease, int firstFreeBlockAddress, string filePath, int blockFactor)
    {
        // Clear the block to release and set its next pointer to the current first free block
        blockToRelease.ClearNextAndValid();
        blockToRelease.PreviousBlockAddress = -1;
        blockToRelease.NextBlockAddress = firstFreeBlockAddress;
        WriteBlockToFile(blockToRelease, filePath);

        // Update the previous first free block to point back to the new first free block
        DHBlock<T> previousFirstFreeBlock = new DHBlock<T>(blockFactor, firstFreeBlockAddress);
        previousFirstFreeBlock.ReadFromBinaryFile(filePath, firstFreeBlockAddress);
        previousFirstFreeBlock.PreviousBlockAddress = blockToRelease.BlockAddress;
        WriteBlockToFile(previousFirstFreeBlock, filePath);
    }

    private void InsertBlockAfterFirstFreeBlock(DHBlock<T> blockToRelease, int firstFreeBlockAddress, string filePath, int blockFactor)
    {
        // Read the current first free block
        DHBlock<T> firstFreeBlock = new DHBlock<T>(blockFactor, firstFreeBlockAddress);
        firstFreeBlock.ReadFromBinaryFile(filePath, firstFreeBlockAddress);

        // Link the current first free block to the new block and update the file
        var oldNextAddress = firstFreeBlock.NextBlockAddress;
        firstFreeBlock.NextBlockAddress = blockToRelease.BlockAddress;
        WriteBlockToFile(firstFreeBlock, filePath);

        // Clear the new block and set its pointers, then update the file
        blockToRelease.ClearNextAndValid();
        blockToRelease.PreviousBlockAddress = firstFreeBlockAddress;
        blockToRelease.NextBlockAddress = oldNextAddress;
        WriteBlockToFile(blockToRelease, filePath);

        // If the new block points to another block, update that block's previous pointer
        if (oldNextAddress != -1)
        {
            DHBlock<T> oldNextBlock = new DHBlock<T>(blockFactor, oldNextAddress);
            oldNextBlock.ReadFromBinaryFile(filePath, oldNextAddress);
            oldNextBlock.PreviousBlockAddress = blockToRelease.BlockAddress;
            WriteBlockToFile(oldNextBlock, filePath);
        }
    }


    private void AddBlockToFreeList(DHBlock<T> block, bool isOverflow)
    {
        block.ClearBlockPreviousNextAndValid();

        if (isOverflow)
        {
            firstFreeBlockOverflowFile = block.BlockAddress;
        }
        else
        {
            firstFreeBlockMainFile = block.BlockAddress;
        }
    }

    private void WriteBlockToFile(DHBlock<T> block, string filePath)
    {
        block.WriteToBinaryFile(filePath, block.BlockAddress);
    }

    private void ShrinkFile(bool isOverflow, int blockAddress)
    {
        // Update the file size and reset the first free block address if needed
        UpdateFileSizeAndResetFirstFreeBlock(isOverflow, blockAddress);

        // Shrink the actual file to the new size
        ShrinkPhysicalFile(isOverflow, blockAddress);
    }

    private void UpdateFileSizeAndResetFirstFreeBlock(bool isOverflow, int blockAddress)
    {
        if (isOverflow)
        {
            CurrentOverflowFileSize = blockAddress;
            if (firstFreeBlockOverflowFile == blockAddress)
            {
                firstFreeBlockOverflowFile = -1;
            }
        }
        else
        {
            CurrentMainFileSize = blockAddress;
            if (firstFreeBlockMainFile == blockAddress || CurrentMainFileSize == 0)
            {
                firstFreeBlockMainFile = -1;
            }
        }
    }

    private void ShrinkPhysicalFile(bool isOverflow, int blockAddress)
    {
        string filePath = GetFilePath(isOverflow);
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write))
        {
            fileStream.SetLength(blockAddress);
        }
    }

    public DHBlock<T> GetPreviousBlockBasedOnType(int previousBlockAddress, int actualBlockAddress)
    {
        bool previousIsMain = false;
        try
        {
            DHBlock<T> previousBlockTry = new DHBlock<T>(MainFileBlockFactor, previousBlockAddress);
            previousBlockTry.ReadFromBinaryFile(MainFilePath, previousBlockAddress);
            if (previousBlockTry.NextBlockAddress == actualBlockAddress)
            {
                previousIsMain = true;
            }
        }
        catch
        {
            previousIsMain = false;
        }

        DHBlock<T> previousBlock;

        if (previousIsMain)
        {
            previousBlock = new DHBlock<T>(MainFileBlockFactor, previousBlockAddress);
            previousBlock.ReadFromBinaryFile(MainFilePath, previousBlockAddress);
        }
        else
        {
            previousBlock = new DHBlock<T>(OverflowFileBlockFactor, previousBlockAddress);
            previousBlock.ReadFromBinaryFile(OverflowFilePath, previousBlockAddress);
        }

        return previousBlock;
    }


    public string SequentialFileOutput(int maxHashSize)
    {
        StringBuilder output = new StringBuilder();

        output.AppendLine("\n---------------------------------------------------------------");
        output.AppendLine("Max Hash Size: " + maxHashSize + "\n");

        // Process the main file
        output.AppendLine("Main Block Factor: " + MainFileBlockFactor);
        output.AppendLine("First free block: " + firstFreeBlockMainFile);
        output.AppendLine("Main File Contents:");
        output.Append(ProcessFileSequentially(MainFilePath, MainFileBlockFactor));

        // Process the overflow file
        output.AppendLine("\nOverflow Block Factor: " + OverflowFileBlockFactor);
        output.AppendLine("First free block: " + firstFreeBlockOverflowFile);
        output.AppendLine("Overflow File Contents:");
        output.Append(ProcessFileSequentially(OverflowFilePath, OverflowFileBlockFactor));

        return output.ToString();
    }

    private string ProcessFileSequentially(string filePath, int blockFactor)
    {
        StringBuilder fileOutput = new StringBuilder();

        int currentAddress = 0;
        long fileSize = File.Exists(filePath) ? new FileInfo(filePath).Length : 0;

        while (currentAddress < fileSize)
        {
            DHBlock<T> block = new DHBlock<T>(blockFactor, currentAddress);
            block.ReadFromBinaryFile(filePath, currentAddress);
            
            fileOutput.AppendLine($"Block at Address {block.BlockAddress}:");
            fileOutput.AppendLine($"  Valid Records Count: {block.ValidRecordsCount}");
            fileOutput.AppendLine($"  Next Block Address: {block.NextBlockAddress}");
            fileOutput.AppendLine($"  Previous Block Address: {block.PreviousBlockAddress}");
            foreach (var record in block.RecordsList)
            {
                fileOutput.AppendLine($"    {record}");
            }

            if(block.BlockAddress != -1 && block.ValidRecordsCount == 0)
            {
                Console.WriteLine(  );
            }

            currentAddress += block.GetSize();
        }

        return fileOutput.ToString();
    }
}
