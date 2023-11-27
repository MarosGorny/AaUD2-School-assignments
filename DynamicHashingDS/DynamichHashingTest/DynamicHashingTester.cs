using DynamicHashingDS.Data;
using DynamicHashingDS.DH;
using System;
using System.Collections.Generic;

namespace DynamicHashingDS.DynamicHashingTest;
class DynamicHashingTester
{
    private DynamicHashing<DummyClass> _dynamicHashing;
    private List<DummyClass> _insertedRecords;
    private Random _random;
    private double _insertProbability, _deleteProbability, _findProbability;

    private HashSet<int> _availableIDs;
    private bool _useSelectedIds = false;
    private int _idRange;
    private int _maxHashSize;
    private int _mainBlockFactor;
    private int _overflowBlockFactor;

    public DynamicHashingTester(int mainBlockFactor, int overflowBlockFactor, int maxHashSize, 
                                double insertProbability = 0.33, double deleteProbability = 0.33, double findProbability = 0.34, bool useSelectedIds = false)
    {
        // Initialize the dynamic hashing with appropriate parameters
        _dynamicHashing = new DynamicHashing<DummyClass>(mainBlockFactor, overflowBlockFactor, "mainFilePath.dat", "overflowFilePath.dat", maxHashSize);
        _insertedRecords = new List<DummyClass>();
        _random = new Random(-1);

        // Set the probabilities
        _insertProbability = insertProbability;
        _deleteProbability = deleteProbability + insertProbability;
        _findProbability = 1.0;  // The rest is for the find operation

        _mainBlockFactor = mainBlockFactor;
        _overflowBlockFactor = overflowBlockFactor;
        _maxHashSize = maxHashSize;


        _idRange = mainBlockFactor * maxHashSize;

        _useSelectedIds = useSelectedIds;

        //Set seed for reproducibility
        
    }

    public DummyClass GenerateRandomRecord()
    {
        return new DummyClass
        {
            Cislo = _random.Next(0, 1000),
            ID = _random.Next(0, 1000),
            Text = GenerateRandomString(14)
        };
    }

    public DummyClass GenerateRandomRecordFromIds()
    {
        // Select a random ID from the available IDs
        if(_availableIDs.Count == 0 && _useSelectedIds)
        {
            Console.WriteLine("Trie is full !!");
            return null;
        }
        int randomIDIndex = _random.Next(_availableIDs.Count);
        int selectedID = _availableIDs.ElementAt(randomIDIndex);

        // Remove the selected ID from the available IDs
        _availableIDs.Remove(selectedID);

        return new DummyClass
        {
            Cislo = selectedID,  // Use the selected unique ID
            
            ID = _random.Next(0, 1000),
            Text = GenerateRandomString(14)
        };
    }

    public void RunRandomTest(int iterations)
    {
        // Initialize the available IDs
        _availableIDs = new HashSet<int>();
        for (int i = 0; i < _idRange; i++)
        {
            _availableIDs.Add(i);
        }

        for (int i = 0; i < iterations; i++)
        {
            if(i == 125)
            {
                Console.WriteLine(  );
            }
            var structureRecords = _dynamicHashing.FileBlockManager.GetAllRecords(false);
            Console.Write(_insertedRecords.Count - structureRecords.Count);
            if(_insertedRecords.Count != structureRecords.Count)
            {
                Console.WriteLine(_dynamicHashing.FileBlockManager.SequentialFileOutput(_maxHashSize));
                
            }
            double action = _random.NextDouble();

            Console.Write($"I:{i+1 } ");

            if (action < _insertProbability)
                InsertRandomRecord();
            else if (action < _deleteProbability)
                DeleteRandomRecord();
            else
                FindRandomRecord();
        }

        VerifyStructure();
    }

    public void InsertRandomRecord()
    {
        var record = _useSelectedIds ? GenerateRandomRecordFromIds() : GenerateRandomRecord();
        if(record == null)
        {
            //Console.WriteLine("Record is null!");
            return;
        }
        _dynamicHashing.Insert(record);
        _insertedRecords.Add(record);
        Console.WriteLine($"Inserted: {record}");
    }

    public void DeleteRandomRecord()
    {
        if (_insertedRecords.Count == 0)
        {
            var record = GenerateRandomRecord();
            _dynamicHashing.Delete(record);
            Console.WriteLine($"Deleted non existing record: {record}");
        } 
        else
        {
            int index = _random.Next(_insertedRecords.Count);
            var record = _insertedRecords[index];

            _dynamicHashing.Delete(record);
            _insertedRecords.RemoveAt(index);
            _availableIDs.Add(record.Cislo); // Add the deleted ID to the available IDs
            Console.WriteLine($"Deleted: {record}");
        }


    }

    public void FindRandomRecord()
    {
        if (_insertedRecords.Count == 0)
        {
            var record = GenerateRandomRecord();
            _dynamicHashing.TryFind(record, out var foundRecord);
            Console.WriteLine($"Find non existing record: {foundRecord}");
        }
        else
        {

            int index = _random.Next(_insertedRecords.Count);
            var record = _insertedRecords[index];

            var found = _dynamicHashing.TryFind(record, out var foundRecord);
            if (!found)
            {
                Console.WriteLine($"Record not found: {record}");
                return;
            }

            Console.WriteLine($"Found: {foundRecord}");
        }


    }

    private string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[_random.Next(s.Length)]).ToArray());
    }

    public List<DummyClass> InsertBatch(int count)
    {
        List<DummyClass> batchRecords = new List<DummyClass>();
        for (int i = 0; i < count; i++)
        {
            var record = _useSelectedIds ? GenerateRandomRecordFromIds() : GenerateRandomRecord();
            _dynamicHashing.Insert(record);
            batchRecords.Add(record);
            _insertedRecords.Add(record); // Keep track of inserted records for verification
            Console.WriteLine($"Batch Insert: Record {i + 1} - {record}");
        }
        return batchRecords;
    }


    public void VerifyStructure()
    {
        int insertedRecordsCount = _insertedRecords.Count;
        var structureRecords = _dynamicHashing.FileBlockManager.GetAllRecords(false);
        int recordsInStructure = structureRecords.Count;

        if (insertedRecordsCount != recordsInStructure)
        {
            Console.WriteLine($"Structure is invalid! Inserted records: {insertedRecordsCount}, records in structure: {recordsInStructure}");
            throw new Exception("Verification failed! Lists contain different items.");
            return;
        }

        // Both lists should have the same items
        foreach (var item in _insertedRecords)
        {
            var found = false;
            foreach (var item2 in structureRecords)
            {
                if (item.MyEquals((DummyClass)item2))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Console.WriteLine("Verification failed! Lists contain different items.");
                throw new Exception("Verification failed! Lists contain different items.");
                return;
            }
        }

        Console.WriteLine("--------------------------------- Verification successful! ------------------------------------------");
        _dynamicHashing = new DynamicHashing<DummyClass>(_mainBlockFactor, _overflowBlockFactor, "mainFilePath.dat", "overflowFilePath.dat", _maxHashSize);
        _insertedRecords = new List<DummyClass>();

    }
}
