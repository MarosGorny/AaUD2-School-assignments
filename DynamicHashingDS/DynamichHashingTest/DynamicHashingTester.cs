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

    public DynamicHashingTester(int mainBlockFactor, int overflowBlockFactor, int maxHashSize,
                                double insertProbability = 0.33, double deleteProbability = 0.33, double findProbability = 0.34)
    {
        // Initialize the dynamic hashing with appropriate parameters
        _dynamicHashing = new DynamicHashing<DummyClass>(mainBlockFactor, overflowBlockFactor, "mainFilePath.dat", "overflowFilePath.dat", maxHashSize);
        _insertedRecords = new List<DummyClass>();
        _random = new Random();

        // Set the probabilities
        _insertProbability = insertProbability;
        _deleteProbability = deleteProbability + insertProbability;
        _findProbability = 1.0;  // The rest is for the find operation
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

    public void RunRandomTest(int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            double action = _random.NextDouble();

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
        var record = GenerateRandomRecord();
        _dynamicHashing.Insert(record);
        _insertedRecords.Add(record);
        Console.WriteLine($"Inserted: {record}");
    }

    public void DeleteRandomRecord()
    {
        if (_insertedRecords.Count == 0)
        {
            Console.WriteLine("No records to delete!");
            return;
        }

        int index = _random.Next(_insertedRecords.Count);
        var record = _insertedRecords[index];
        
        _dynamicHashing.Delete(record);
        _insertedRecords.RemoveAt(index);
        Console.WriteLine($"Deleted: {record}");
    }

    public void FindRandomRecord()
    {
        if (_insertedRecords.Count == 0)
        {
            Console.WriteLine("No records to find!");
            return;
        }

        int index = _random.Next(_insertedRecords.Count);
        var record = _insertedRecords[index];

        var found = _dynamicHashing.TryFind(record, out var foundRecord);
        if(!found)
        {
            Console.WriteLine($"Record not found: {record}");
            return;
        }

        Console.WriteLine($"Found: {foundRecord}");
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
            var record = GenerateRandomRecord();
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
                return;
            }
        }

        Console.WriteLine("--------------------------------- Verification successful! ------------------------------------------");

    }
}
