using DynamicHashingDS.Data;
using System.Text;

namespace DynamicHashingDS.DH;

public class FileBlockManager<T> where T : IDHRecord<T>, new()
{
    public string MainFilePath { get; private set;}
    public string OverflowFilePath { get; private set;}

    private int firstFreeBlockMainFile; // Address of the first free block in the main file
    private int firstFreeBlockOverflowFile; // Address of the first free block in the overflow file

    private int currentMainFileSize; // Size of the main file in blocks
    private int currentOverflowFileSize; // Size of the overflow file in blocks

    private int mainFileBlockFactor; // Number of records in a block in the main file
    private int overflowFileBlockFactor; // Number of records in a block in the overflow file

    public FileBlockManager(string mainFilePath, string overflowFilePath, int mainFileBlockFactor, int overflowFileBlockFactor)
    {
        this.MainFilePath = mainFilePath;
        this.OverflowFilePath = overflowFilePath;
        this.firstFreeBlockMainFile = -1; 
        this.firstFreeBlockOverflowFile = -1; 
        this.currentMainFileSize = 0;
        this.currentOverflowFileSize = 0;
        this.mainFileBlockFactor = mainFileBlockFactor;
        this.overflowFileBlockFactor = overflowFileBlockFactor;
    }

    public int GetFreeBlock(bool isOverflow)
    {
        int firstFreeBlock = isOverflow ? firstFreeBlockOverflowFile : firstFreeBlockMainFile;
        if (firstFreeBlock != -1)
        {
            return -99;
        }
        else
        {
            return isOverflow ? AppendBlock(true) : AppendBlock(false);
        }
    }

    private int AppendBlock(bool isOverflow)
    {
        
        int originalAddress = isOverflow ? currentOverflowFileSize : currentMainFileSize;

        int blockSize = new DHBlock<T>(mainFileBlockFactor, -1).GetSize();
        if(isOverflow)
        {
            currentOverflowFileSize += blockSize;
        }
        else
        {
            currentMainFileSize += blockSize;
        }

        return originalAddress;

    }

    public void ReleaseBlock(int blockAddress, bool isOverflow)
    {
        int firstFreeBlock = isOverflow ? firstFreeBlockOverflowFile : firstFreeBlockMainFile;
        int fileSize = isOverflow ? currentOverflowFileSize : currentMainFileSize;
        string filePath = isOverflow ? OverflowFilePath : MainFilePath;
        int blockFactor = isOverflow ? overflowFileBlockFactor : mainFileBlockFactor;

        int recordSize = isOverflow ? new DHBlock<T>(overflowFileBlockFactor).GetSize() : new DHBlock<T>(mainFileBlockFactor).GetSize();

        // If the block to release is at the end of the file, shrink the file
        if (blockAddress == fileSize - recordSize)
        {
            ShrinkFile(isOverflow, blockAddress);
        }
        else
        {
            DHBlock<T> blockToRelease = new DHBlock<T>(blockFactor, blockAddress);
            blockToRelease.ReadFromBinaryFile(filePath,blockAddress);

            int nextBlockAddress = blockToRelease.NextBlockAddress;
            int previousBlockAddress = blockToRelease.PreviousBlockAddress;

            // Update the block to point to the current first free block
            blockToRelease.NextBlockAddress = firstFreeBlock;

            if (previousBlockAddress != -1)
            {
                DHBlock<T> previousBlock = new DHBlock<T>(blockFactor, previousBlockAddress);
                previousBlock.NextBlockAddress = nextBlockAddress;
                previousBlock.WriteToBinaryFile(filePath, previousBlockAddress);
            }

            if(nextBlockAddress != -1)
            {
                DHBlock<T> nextBlock = new DHBlock<T>(blockFactor, nextBlockAddress);
                nextBlock.PreviousBlockAddress = previousBlockAddress;
                nextBlock.WriteToBinaryFile(filePath, nextBlockAddress);
            }

            blockToRelease.Clear();
            blockToRelease.WriteToBinaryFile(filePath, blockAddress);
        }
    }

    private void ShrinkFile(bool isOverflow, int blockAddress)
    {
        if (isOverflow)
        {
            currentOverflowFileSize = blockAddress; 
                                                    
            if (firstFreeBlockOverflowFile == blockAddress)
            {
                firstFreeBlockOverflowFile = -1;
            }
        }
        else
        {
            currentMainFileSize = blockAddress; 
            if (firstFreeBlockMainFile == blockAddress)
            {
                firstFreeBlockMainFile = -1;
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
        int currentAddress = 0;
        long fileSize = File.Exists(filePath) ? new FileInfo(filePath).Length : 0;

        while (currentAddress < fileSize)
        {
            DHBlock<T> block = new DHBlock<T>(blockFactor, currentAddress);
            block.ReadFromBinaryFile(filePath, currentAddress);

            fileOutput.AppendLine($"Block at Address {currentAddress}:");
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
}
