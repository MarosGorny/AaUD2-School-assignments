using DynamicHashingDS.Data;
using System.Text;

namespace DynamicHashingDS.DH;

public class FileBlockManager<T> where T : IDHRecord<T>, new()
{
    public string MainFilePath { get; private set;}
    public string OverflowFilePath { get; private set;}

    public int CurrentMainFileSize { get; private set;}
    public int CurrentOverflowFileSize { get; private set;}

    private int firstFreeBlockMainFile; // Address of the first free block in the main file
    private int firstFreeBlockOverflowFile; // Address of the first free block in the overflow file

    //private int CurrentMainFileSize; // Size of the main file in blocks
    //private int CurrentOverflowFileSize; // Size of the overflow file in blocks

    private int mainFileBlockFactor; // Number of records in a block in the main file
    private int overflowFileBlockFactor; // Number of records in a block in the overflow file

    public FileBlockManager(string mainFilePath, string overflowFilePath, int mainFileBlockFactor, int overflowFileBlockFactor)
    {
        this.MainFilePath = mainFilePath;
        this.OverflowFilePath = overflowFilePath;
        this.firstFreeBlockMainFile = -1; 
        this.firstFreeBlockOverflowFile = -1; 
        this.CurrentMainFileSize = 0;
        this.CurrentOverflowFileSize = 0;
        this.mainFileBlockFactor = mainFileBlockFactor;
        this.overflowFileBlockFactor = overflowFileBlockFactor;
    }

    public int GetFreeBlock(bool isOverflow)
    {
        int firstFreeBlock = isOverflow ? firstFreeBlockOverflowFile : firstFreeBlockMainFile;
        if (firstFreeBlock != -1)
        {
            DHBlock<T> freeBlock = new DHBlock<T>(isOverflow ? overflowFileBlockFactor : mainFileBlockFactor, firstFreeBlock);
            freeBlock.ReadFromBinaryFile(isOverflow ? OverflowFilePath : MainFilePath, firstFreeBlock);

            if (freeBlock.NextBlockAddress == -1)
            {
                freeBlock.Clear();
                if(isOverflow)
                {
                    firstFreeBlockOverflowFile = -1;
                }
                else
                {
                    firstFreeBlockMainFile = -1;
                }
                freeBlock.WriteToBinaryFile(isOverflow ? OverflowFilePath : MainFilePath, freeBlock.BlockAddress);
            }
            else
            {
                DHBlock<T> nextFreeBlock = new DHBlock<T>(isOverflow ? overflowFileBlockFactor : mainFileBlockFactor, freeBlock.NextBlockAddress);
                nextFreeBlock.ReadFromBinaryFile(isOverflow ? OverflowFilePath : MainFilePath, freeBlock.NextBlockAddress);
                nextFreeBlock.PreviousBlockAddress = -1;
                nextFreeBlock.WriteToBinaryFile(isOverflow ? OverflowFilePath : MainFilePath, freeBlock.NextBlockAddress);
            }

            return firstFreeBlock;
            //throw new NotImplementedException("GetFreeBlock() is not implemented yet.");
        }
        else
        {
            return isOverflow ? AppendBlock(true) : AppendBlock(false);
        }
    }

    private int AppendBlock(bool isOverflow)
    {
        
        int originalAddress = isOverflow ? CurrentOverflowFileSize : CurrentMainFileSize;

        int blockSize = new DHBlock<T>(mainFileBlockFactor, -1).GetSize();
        if(isOverflow)
        {
            CurrentOverflowFileSize += blockSize;
        }
        else
        {
            CurrentMainFileSize += blockSize;
        }

        return originalAddress;

    }

    //public void ReleaseBlock(int blockAddress, bool isOverflow)
    public void ReleaseBlock(DHBlock<T> block, bool isOverflow)
    {
        int firstFreeBlockAddress = isOverflow ? firstFreeBlockOverflowFile : firstFreeBlockMainFile;
        int fileSize = isOverflow ? CurrentOverflowFileSize : CurrentMainFileSize;
        string filePath = isOverflow ? OverflowFilePath : MainFilePath;
        int blockFactor = isOverflow ? overflowFileBlockFactor : mainFileBlockFactor;

        int recordSize = isOverflow ? new DHBlock<T>(overflowFileBlockFactor).GetSize() : new DHBlock<T>(mainFileBlockFactor).GetSize();

        // If the block to release is at the end of the file, shrink the file
        if (block.BlockAddress == fileSize - recordSize)
        {
            ShrinkFile(isOverflow, block.BlockAddress);
        }
        else
        {
            DHBlock<T> blockToRelease = block;

            if(firstFreeBlockAddress == -1)
            {
                if (isOverflow)
                {
                    firstFreeBlockOverflowFile = block.BlockAddress;
                }
                else
                {
                    firstFreeBlockMainFile = block.BlockAddress;
                }

                firstFreeBlockAddress = block.BlockAddress;
                blockToRelease.Clear();
                blockToRelease.WriteToBinaryFile(filePath, block.BlockAddress);
            }
            else
            {
                if(firstFreeBlockAddress > block.BlockAddress)
                {
                    // The block to release is before the current first free block
                    blockToRelease.Clear();
                    blockToRelease.PreviousBlockAddress = -1;
                    blockToRelease.NextBlockAddress = firstFreeBlockAddress;
                    blockToRelease.WriteToBinaryFile(filePath, block.BlockAddress);

                    // Update the previous first free block to point to the previous block
                    DHBlock<T> previousFirstFreeBlock = new DHBlock<T>(blockFactor, firstFreeBlockAddress);
                    previousFirstFreeBlock.ReadFromBinaryFile(filePath, firstFreeBlockAddress);
                    previousFirstFreeBlock.PreviousBlockAddress = blockToRelease.BlockAddress;
                    previousFirstFreeBlock.WriteToBinaryFile(filePath, firstFreeBlockAddress);
                } 
                else if (firstFreeBlockAddress < block.BlockAddress)
                {
                    DHBlock<T> firstFreeBlock = new DHBlock<T>(blockFactor, firstFreeBlockAddress);
                    firstFreeBlock.ReadFromBinaryFile(filePath, firstFreeBlockAddress);

                    var oldNextAddress = firstFreeBlock.NextBlockAddress;

                    firstFreeBlock.NextBlockAddress = blockToRelease.BlockAddress;
                    firstFreeBlock.WriteToBinaryFile(filePath, firstFreeBlockAddress);

                    blockToRelease.Clear();
                    blockToRelease.PreviousBlockAddress = firstFreeBlockAddress;
                    blockToRelease.NextBlockAddress = oldNextAddress;
                    blockToRelease.WriteToBinaryFile(filePath, block.BlockAddress);

                    if(oldNextAddress != -1)
                    {
                        DHBlock<T> oldNextBlock = new DHBlock<T>(blockFactor, oldNextAddress);
                        oldNextBlock.ReadFromBinaryFile(filePath, oldNextAddress);
                        oldNextBlock.PreviousBlockAddress = blockToRelease.BlockAddress;
                        oldNextBlock.WriteToBinaryFile(filePath, oldNextAddress);
                    }

                }
                blockToRelease = new DHBlock<T>(blockFactor, firstFreeBlockAddress);
                blockToRelease.ReadFromBinaryFile(filePath, firstFreeBlockAddress);
            }

            //int nextBlockAddress = blockToRelease.NextBlockAddress;
            //int previousBlockAddress = blockToRelease.PreviousBlockAddress;

            //// Update the block to point to the current first free block
            //blockToRelease.NextBlockAddress = firstFreeBlockAddress;

            //if (previousBlockAddress != -1)
            //{
            //    DHBlock<T> previousBlock = new DHBlock<T>(blockFactor, previousBlockAddress);
            //    previousBlock.NextBlockAddress = nextBlockAddress;
            //    previousBlock.WriteToBinaryFile(filePath, previousBlockAddress);
            //}

            //if(nextBlockAddress != -1)
            //{
            //    DHBlock<T> nextBlock = new DHBlock<T>(blockFactor, nextBlockAddress);
            //    nextBlock.PreviousBlockAddress = previousBlockAddress;
            //    nextBlock.WriteToBinaryFile(filePath, nextBlockAddress);
            //}

            //blockToRelease.Clear();
            //blockToRelease.WriteToBinaryFile(filePath, block.BlockAddress);
        }
    }

    private void ShrinkFile(bool isOverflow, int blockAddress)
    {
        if (isOverflow)
        {
            CurrentOverflowFileSize = blockAddress; 
                                                    
            if (firstFreeBlockOverflowFile == blockAddress)
            {
                firstFreeBlockOverflowFile = -1;
            }

            using (FileStream fileStream = new FileStream(OverflowFilePath, FileMode.Open, FileAccess.Write))
            {
                fileStream.SetLength(blockAddress);
            }
        }
        else
        {
            CurrentMainFileSize = blockAddress; 
            if (firstFreeBlockMainFile == blockAddress || CurrentMainFileSize == 0)
            {
                firstFreeBlockMainFile = -1;
            }

            using (FileStream fileStream = new FileStream(MainFilePath, FileMode.Open, FileAccess.Write))
            {
                fileStream.SetLength(blockAddress);
            }
        }
    }

    public string SequentialFileOutput(int maxHashSize)
    {
        StringBuilder output = new StringBuilder();

        output.AppendLine("\n---------------------------------------------------------------");
        output.AppendLine("Max Hash Size: " + maxHashSize + "\n");

        // Process the main file
        output.AppendLine("Main Block Factor: " + mainFileBlockFactor);
        output.AppendLine("Main File Contents:");
        output.AppendLine("First free blokc: " + firstFreeBlockMainFile);
        output.Append(ProcessFileSequentially(MainFilePath, mainFileBlockFactor));

        // Process the overflow file
        output.AppendLine("\nOverflow Block Factor: " + overflowFileBlockFactor);
        output.AppendLine("Overflow File Contents:");
        output.Append(ProcessFileSequentially(OverflowFilePath, overflowFileBlockFactor));

        return output.ToString();
    }

    private string ProcessFileSequentially(string filePath, int blockFactor)
    {
        StringBuilder fileOutput = new StringBuilder();
        //int currentAdress 
        //if(filePath == MainFilePath)
        //{

        //}
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

            currentAddress += block.GetSize();
        }

        return fileOutput.ToString();
    }

    public List<IDHRecord<T>> GetAllRecords(bool fromOverflowFile)
    {
        string filePath = fromOverflowFile ? OverflowFilePath : MainFilePath;
        List<IDHRecord<T>> allRecords = new List<IDHRecord<T>>();
        int blockFactor = fromOverflowFile ? overflowFileBlockFactor : mainFileBlockFactor;
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
}
