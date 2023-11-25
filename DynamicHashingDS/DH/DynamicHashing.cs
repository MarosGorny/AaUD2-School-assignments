using DynamicHashingDS.Data;
using DynamicHashingDS.Nodes;
using System.Collections;
using System.IO;

namespace DynamicHashingDS.DH;

public class DynamicHashing<T> where T : IDHRecord<T>, new()
{
    private DHNode<T> Root;
    public int MainBlockFactor { get; private set; }
    public int OverflowBlockFactor { get; private set; }
    public int MaxBlockDepth { get; private set; }

    public int MaxHashSize { get; private set; }
    //public List<DHBlock<T>> Blocks { get; private set; }

    public FileBlockManager<T> FileBlockManager { get; private set;}

    public DynamicHashing(int mainBlockFactor, int overflowBlockFactor, string mainFilePath, string overflowFilePath, int? maxHashSize = null)
    {
        MaxHashSize = maxHashSize ?? new T().GetHash().Length;
        //TODO: DONT FORGET ABOUT THISSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
        if (File.Exists(mainFilePath))
        {
            File.Delete(mainFilePath);
        }

        using (var fsMain = new FileStream(mainFilePath, FileMode.Create))
        {
            // Empty file is created, and FileStream is closed upon exiting the using block
        }

        if (File.Exists(overflowFilePath))
        {
            File.Delete(overflowFilePath);
        }

        using (var fsOverflow = new FileStream(overflowFilePath, FileMode.Create))
        {
            // Empty file is created, and FileStream is closed upon exiting the using block
        }

        // ... Initialization ...
        FileBlockManager = new FileBlockManager<T>(mainFilePath, overflowFilePath, mainBlockFactor, overflowBlockFactor);

        MaxBlockDepth = new T().GetHash().Length;
        MainBlockFactor = mainBlockFactor;
        OverflowBlockFactor = overflowBlockFactor;

        Root = new DHInternalNode<T>(this,null);

        //var leftBlockAddress = FileBlockManager.GetFreeBlock(false);
        //var leftBlock = new DHBlock<T>(mainBlockFactor, leftBlockAddress);
        //leftBlock.WriteToBinaryFile(mainFilePath,leftBlockAddress);
        ((DHInternalNode<T>)Root).AddLeftExternalNode(-1);

        //var rightBlockAddress = FileBlockManager.GetFreeBlock(false);
        //var rightBlock = new DHBlock<T>(mainBlockFactor, rightBlockAddress);
        //rightBlock.WriteToBinaryFile(mainFilePath, rightBlockAddress);
        ((DHInternalNode<T>)Root).AddRightExternalNode(-1);

        Console.WriteLine("Root created");
        Console.WriteLine(FileBlockManager.SequentialFileOutput(MaxHashSize));

    }

    public void Insert(IDHRecord<T> record)
    {
        bool inserted = Root.Insert(record);
        Console.WriteLine(FileBlockManager.SequentialFileOutput(MaxHashSize));
    }

    /// <summary>
    /// OLD IMPLEMENTATION
    /// </summary>
    /// <param name="record"></param>

    //public void Insert(DHRecord record)
    //{
    //    DHNode currentNode = Root;
    //    var hash = record.GetHash();

    //    while(currentNode is DHInternalNode internalNode)
    //    {
    //        var nodeDepth = currentNode.Depth;
    //        bool depthBit = hash[nodeDepth];
    //        if (depthBit)
    //        {
    //            currentNode = internalNode.RightChild;
    //        }
    //        else
    //        {
    //            currentNode = internalNode.LeftChild;
    //        }
    //    }

    //    if (currentNode is DHExternalNode<T> externalNode)
    //    {
    //        if(externalNode.RecordsCount < BlockFactor)
    //        {
    //            externalNode.RecordsCount++;
    //            externalNode.BlockAddress = Blocks.Count - 1;
    //            Blocks[externalNode.BlockAddress].AddRecord(record as T);
    //        }
    //        else 
    //        {
    //            var nodeAddress = externalNode.BlockAddress;
    //            var parent = externalNode.Parent;
    //            var newInternalNode = new DHInternalNode();
    //            if(externalNode.Depth == 0)
    //            {
    //                Root = newInternalNode;
    //            }
    //            newInternalNode.Depth = externalNode.Depth;
    //            newInternalNode.Parent = parent;

    //            newInternalNode.LeftChild = externalNode;
    //            newInternalNode.RightChild = new DHExternalNode<T>();

    //            newInternalNode.RightChild.Depth = newInternalNode.Depth + 1;
    //            newInternalNode.LeftChild.Depth = newInternalNode.Depth + 1;

    //            newInternalNode.RightChild.Parent = newInternalNode;
    //            newInternalNode.LeftChild.Parent = newInternalNode;

    //            var newBlock = new DHBlock<T>(BlockFactor);
    //            Blocks.Add(newBlock);

    //            var reInsertBlock = Blocks[nodeAddress];

    //            foreach (var reInsertRecord in reInsertBlock.RecordsList)
    //            {
    //                var reInsertRecordHash = reInsertRecord.GetHash();
    //                var reInsertRecordDepthBit = reInsertRecordHash[newInternalNode.Depth];
    //                //TODO: We also need to delete that record from old one.
    //                parent.Insert(reInsertRecord);
                    
    //            }

    //        }

          
    //    }

    //}

    //public void Delete(DHRecord record)
    //{
    //    // Implement deletion logic
    //}

    //public DHRecord Find(DHRecord record)
    //{
    //    var = record.GetHash();
    //}

    // Additional methods for handling resizing, splitting, etc.
}
