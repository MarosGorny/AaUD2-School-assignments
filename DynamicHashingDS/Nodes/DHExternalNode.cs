using DynamicHashingDS.Data;
using DynamicHashingDS.DH;

namespace DynamicHashingDS.Nodes;

public class DHExternalNode<T> : DHNode<T> where T : IDHRecord<T>, new()
{
    public int RecordsCount { get; private set; }
    public int BlockAddress { get; private set; }

    private FileBlockManager<T> fileBlockManager;

    public DHExternalNode(DynamicHashing<T> dynamicHashing, DHNode<T> parent,int blockAdress) : base(dynamicHashing,parent)
    {
        RecordsCount = 0;
        BlockAddress = blockAdress;
        fileBlockManager = dynamicHashing.FileBlockManager;
    }

    public override bool Insert(IDHRecord<T> record)
    {
        int blockFactor = dynamicHashing.MainBlockFactor;
        // Existing logic to add record if there is space
        if (RecordsCount < blockFactor)
        {
            // Add record to block
            // ...
            this.AddRecord(record, blockFactor);
            return true;
        }
        else
        {
            // Handle the case when the block is full
            if (Depth < record.GetHash().Length) // Assuming MaxHashBits is the maximum depth based on the hash size
            {
                // Split the node and redistribute records
                SplitNode();
                return Insert(record);
            }
            else
            {
                // Handle overflow to overflow file
                HandleOverflow(record, fileBlockManager);
                return true;
            }
        }
    }

    private void SplitNode()
    {
        // Implement node splitting logic
    }

    private void HandleOverflow(IDHRecord<T> record, FileBlockManager<T> fileBlockManager)
    {
        // Implement logic to handle overflow
    }


    public void AddRecord(IDHRecord<T> record, int blockFactor)
    {
        if(RecordsCount == 0)
        {
            DHBlock<T> newBlock = new DHBlock<T>(blockFactor, BlockAddress);
            //newBlock.ReadFromBinaryFile(dynamicHashing.FileBlockManager.MainFilePath, BlockAddress);
            newBlock.AddRecord(record);
            newBlock.WriteToBinaryFile(dynamicHashing.FileBlockManager.MainFilePath,BlockAddress);

            this.RecordsCount++;
        }
    }

    public override string ToString()
    {
        string childType = "Root"; // Default to "Root" if there is no parent
        if (Parent != null)
        {
            var internalParent = Parent as DHInternalNode<T>;
            childType = internalParent != null && internalParent.LeftChild == this ? "LeftChild" : "RightChild";
        }

        return $"ExternalNode({childType}, Depth: {Depth}, Records: {RecordsCount}, BlockAddress: {BlockAddress})";
    }


}
