using DynamicHashingDS.Data;

namespace DynamicHashingDS.Nodes;

public class DHExternalNode<T> : DHNode<T> where T : DHRecord, new()
{
    public int RecordsCount { get; private set; }
    public int BlockAddress { get; private set; }

    public DHExternalNode()
    {
        RecordsCount = 0;
        BlockAddress = -1;
    }

    public override bool Insert(DHRecord record, List<DHBlock<T>> blocks, int blockFactor)
    {
        if (RecordsCount < blockFactor)
        {
            AddRecord(record, blocks);
            return true;
        }
        return false;
    }

    public void AddRecord(DHRecord record, List<DHBlock<T>> blocks)
    {
        RecordsCount++;
        if (BlockAddress == -1)
        {
            BlockAddress = blocks.Count - 1;
        }
        blocks[BlockAddress].AddRecord(record as T);
    }
}
