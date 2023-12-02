using DynamicHashingDS.Data;
using DynamicHashingDS.Nodes;

namespace DynamicHashingDS.DH;

/// <summary>
/// Represents a dynamic hashing mechanism for storing and retrieving records.
/// </summary>
/// <typeparam name="T">The type of records to be stored, which must implement IDHRecord.</typeparam>
public class DynamicHashing<T> where T : IDHRecord<T>, new()
{
    private DHNode<T> _root;
    public int MainBlockFactor { get; private set; }
    public int OverflowBlockFactor { get; private set; }
    public int MaxHashSize { get; private set; }
    public FileBlockManager<T> FileBlockManager { get; private set;}

    /// <summary>
    /// Initializes a new instance of the DynamicHashing class.
    /// </summary>
    /// <param name="mainBlockFactor">The block factor for the main file.</param>
    /// <param name="overflowBlockFactor">The block factor for the overflow file.</param>
    /// <param name="mainFilePath">The file path for the main file.</param>
    /// <param name="overflowFilePath">The file path for the overflow file.</param>
    /// <param name="maxHashSize">The maximum size of the hash. If null, it will be calculated based on the type T.</param>
    public DynamicHashing(int mainBlockFactor, int overflowBlockFactor, string mainFilePath, string overflowFilePath, int? maxHashSize = null)
    {
        InitializeFiles(mainFilePath, overflowFilePath);
        MaxHashSize = maxHashSize ?? new T().GetHash().Length;
        MainBlockFactor = mainBlockFactor;
        OverflowBlockFactor = overflowBlockFactor;
        FileBlockManager = new FileBlockManager<T>(mainFilePath, overflowFilePath, mainBlockFactor, overflowBlockFactor);
        InitializeRootNode();

        //OutputSequentialFile();
    }

    public IDHRecord<T>? Delete(IDHRecord<T> record)
    {
        IDHRecord<T>? deletedRecord = _root.Delete(record);
        PrintNodeHierarchy(_root);
        Console.WriteLine(FileBlockManager.SequentialFileOutput(MaxHashSize));
        Console.WriteLine();
        return deletedRecord;
    }

    public bool TryFind(IDHRecord<T> record, out IDHRecord<T>? foundRecord)
    {  
        return _root.TryFind(record,out foundRecord);
    }

    /// <summary>
    /// Inserts a record into the dynamic hashing structure.
    /// </summary>
    /// <param name="record">The record to insert.</param>
    public void Insert(IDHRecord<T> record)
    {
        bool inserted = _root.Insert(record);
        PrintNodeHierarchy(_root);
        Console.WriteLine(FileBlockManager.SequentialFileOutput(MaxHashSize));
        Console.WriteLine();
    }

    /// <summary>
    /// Initializes necessary files for dynamic hashing.
    /// </summary>
    /// <param name="mainFilePath">The path for the main file.</param>
    /// <param name="overflowFilePath">The path for the overflow file.</param>
    private void InitializeFiles(string mainFilePath, string overflowFilePath)
    {
        DeleteFileIfExists(mainFilePath);
        File.Create(mainFilePath).Close();

        DeleteFileIfExists(overflowFilePath);
        File.Create(overflowFilePath).Close();
    }

    /// <summary>
    /// Deletes a file if it exists.
    /// </summary>
    /// <param name="filePath">The path of the file to delete.</param>
    private void DeleteFileIfExists(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    /// <summary>
    /// Initializes the root node of the dynamic hashing structure.
    /// </summary>
    private void InitializeRootNode()
    {
        _root = new DHInternalNode<T>(this, null);
        ((DHInternalNode<T>)_root).ChangeLeftExternalNodeAddress(-1,0);
        ((DHInternalNode<T>)_root).ChangeRightExternalNodeAddress(-1,0);
    }

    /// <summary>
    /// Outputs the current state of the dynamic hashing structure to the console.
    /// </summary>
    private void OutputSequentialFile()
    {
        Console.WriteLine(FileBlockManager.SequentialFileOutput(MaxHashSize));
    }

    private void PrintNodeHierarchy(DHNode<T> node, int depth = 0)
    {
        if (node is DHInternalNode<T> internalNode)
        {
            Console.WriteLine($"{new string(' ', depth * 2)}Internal Node at Depth {depth}");
            PrintNodeHierarchy(internalNode.LeftChild, depth + 1);
            PrintNodeHierarchy(internalNode.RightChild, depth + 1);
        }
        else if (node is DHExternalNode<T> externalNode)
        {
            Console.WriteLine($"{new string(' ', depth * 2)}External Node at Depth {depth} with {externalNode._recordsCount} records");
        }
    }
}
