using DynamicHashingDS.Data;
using DynamicHashingDS.Nodes;
using System.Collections;

namespace DynamicHashingDS.DH;

public class DynamicHashing<T> where T : DHRecord, new()
{
    private DHNode Root;
    public int BlockFactor { get; set; }
    public int MaxBlockDepth { get; set; }

    public List<DHBlock<T>> Blocks { get; set; }


    public DynamicHashing(int blockFactor)
    {
        Blocks = new List<DHBlock<T>>();
        Blocks.Add(new DHBlock<T>(blockFactor));
        MaxBlockDepth = new T().GetHash().Length;
        BlockFactor = blockFactor;
        Root = new DHExternalNode<T>();
    }

    public void Insert(DHRecord record)
    {
        DHNode currentNode = Root;
        var hash = record.GetHash();

        while(currentNode is DHInternalNode internalNode)
        {
            var nodeDepth = currentNode.Depth;
            bool depthBit = hash[nodeDepth];
            if (depthBit)
            {
                currentNode = internalNode.RightChild;
            }
            else
            {
                currentNode = internalNode.LeftChild;
            }
        }

        if (currentNode is DHExternalNode<T> externalNode)
        {
            if(externalNode.RecordsCount < BlockFactor)
            {
                externalNode.RecordsCount++;
                externalNode.BlockAddress = Blocks.Count - 1;
                Blocks[externalNode.BlockAddress].AddRecord(record as T);
            }
            else 
            {
                var nodeAddress = externalNode.BlockAddress;
                var parent = externalNode.Parent;
                var newInternalNode = new DHInternalNode();
                if(externalNode.Depth == 0)
                {
                    Root = newInternalNode;
                }
                newInternalNode.Depth = externalNode.Depth;
                newInternalNode.Parent = parent;

                newInternalNode.LeftChild = externalNode;
                newInternalNode.RightChild = new DHExternalNode<T>();

                newInternalNode.RightChild.Depth = newInternalNode.Depth + 1;
                newInternalNode.LeftChild.Depth = newInternalNode.Depth + 1;

                newInternalNode.RightChild.Parent = newInternalNode;
                newInternalNode.LeftChild.Parent = newInternalNode;

                var newBlock = new DHBlock<T>(BlockFactor);
                Blocks.Add(newBlock);

                var reInsertBlock = Blocks[nodeAddress];

                foreach (var reInsertRecord in reInsertBlock.RecordsList)
                {
                    var reInsertRecordHash = reInsertRecord.GetHash();
                    var reInsertRecordDepthBit = reInsertRecordHash[newInternalNode.Depth];
                    //TODO: We also need to delete that record from old one.
                    parent.Insert(reInsertRecord);
                    
                }

            }

          
        }

    }

    public void Delete(DHRecord record)
    {
        // Implement deletion logic
    }

    //public DHRecord Find(DHRecord record)
    //{
    //    var = record.GetHash();
    //}

    // Additional methods for handling resizing, splitting, etc.
}
