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
    private FileStream _mainFileStream;
    private FileStream _overflowFileStream;

    private string _mainFilePath;
    private string _overflowFilePath;

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
        FileBlockManager = new FileBlockManager<T>(_mainFileStream, _overflowFileStream, mainBlockFactor, overflowBlockFactor);
        InitializeRootNode();

        //OutputSequentialFile();
    }

    public void CloseFileStreams()
    {
        _mainFileStream?.Close();
        _overflowFileStream?.Close();

        // Set the file stream references to null after closing
        _mainFileStream = null;
        _overflowFileStream = null;
    }

    public IDHRecord<T>? Delete(IDHRecord<T> record)
    {
        IDHRecord<T>? deletedRecord = _root.Delete(record);
        ////PrintNodeHierarchy(_root);
        //Console.WriteLine(FileBlockManager.SequentialFileOutput(MaxHashSize));
        //Console.WriteLine();
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
        //PrintNodeHierarchy(_root);
        //Console.WriteLine(FileBlockManager.SequentialFileOutput(MaxHashSize));
        Console.WriteLine();
    }

    /// <summary>
    /// Initializes necessary files for dynamic hashing.
    /// </summary>
    /// <param name="mainFilePath">The path for the main file.</param>
    /// <param name="overflowFilePath">The path for the overflow file.</param>
    private void InitializeFiles(string mainFilePath, string overflowFilePath)
    {

        // Close existing file streams if they are open
        CloseFileStreams();

        DeleteFileIfExists(mainFilePath);
        //File.Create(mainFilePath).Close();

        DeleteFileIfExists(overflowFilePath);
        //File.Create(overflowFilePath).Close();

        _mainFileStream = new FileStream(mainFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        _overflowFileStream = new FileStream(overflowFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        _mainFilePath = mainFilePath;
        _overflowFilePath = overflowFilePath;

        // Clear the contents of the files
        _mainFileStream.SetLength(0);
        _overflowFileStream.SetLength(0);
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

    public List<IDHRecord<T>> GetAllRecords()
    {
        return FileBlockManager.GetAllRecords(true);
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

    public void Reset()
    {
        // Close the file streams
        CloseFileStreams();

        // Re-open the file streams
        _mainFileStream = new FileStream(_mainFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        _overflowFileStream = new FileStream(_overflowFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        // Reset other internal states as needed
        // For example, re-initialize the root node, clear records, etc.
        InitializeRootNode();
    }


    ~DynamicHashing()
    {
        CloseFileStreams();
    }
}
