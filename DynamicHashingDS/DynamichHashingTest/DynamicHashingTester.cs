using DynamicHashingDS.Data;
using DynamicHashingDS.DH;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

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

    public void EdgeTesting()
    {
        DummyClass dummy = new DummyClass();
        dummy.Cislo = 0;

        DummyClass dummy1 = new DummyClass();
        dummy1.Cislo = 1;
        dummy1.ID = 1;
        dummy1.Text = "ahoj";

        DummyClass dummy2 = new DummyClass();
        dummy2.Cislo = 2;
        dummy2.ID = 2;

        DummyClass dummy3 = new DummyClass();
        dummy3.Cislo = 3;
        dummy3.ID = 3;

        DummyClass dummy5 = new DummyClass();
        dummy5.Cislo = 5;
        dummy5.ID = 5;

        DummyClass dummy7 = new DummyClass();
        dummy7.Cislo = 7;
        dummy7.ID = 7;


        var dynamicHashing = new DynamicHashing<DummyClass>(1, 1, "testMain.bin", "testFlow.bin", 1,true);

        IDHRecord<DummyClass> foundRecord;

        dynamicHashing.Insert(dummy1);
        dynamicHashing.TryFind(dummy1, out foundRecord);
        dynamicHashing.TryFind(dummy3, out foundRecord);

        dynamicHashing.Insert(dummy3);
        dynamicHashing.TryFind(dummy3, out foundRecord);
        dynamicHashing.TryFind(dummy5, out foundRecord);
        dynamicHashing.Insert(dummy5);
        dynamicHashing.TryFind(dummy5, out foundRecord);
        dynamicHashing.TryFind(dummy7, out foundRecord);
        dynamicHashing.Insert(dummy7);
        dynamicHashing.TryFind(dummy7, out foundRecord);


        //dynamicHashing.TryFind(dummy1, out var foundRecord);
        //Console.WriteLine("Found: " + foundRecord);
        ////Console.WriteLine("Deleted: " + dynamicHashing.Delete(dummy1));


        //dynamicHashing.Insert(dummy1);

        //dynamicHashing.TryFind(dummy1, out var foundRecord1);
        //Console.WriteLine("Found: " + foundRecord1);
        ////Console.WriteLine("Deleted: " + dynamicHashing.Delete(dummy1));

        //dynamicHashing.Insert(dummy1);

        //dynamicHashing.TryFind(dummy3, out var foundRecord3);
        //Console.WriteLine(foundRecord3);

        //dynamicHashing.Insert(dummy3);
        ////Console.WriteLine("Deleted: " + dynamicHashing.Delete(dummy3));
        //dynamicHashing.Insert(dummy3);

        //dynamicHashing.TryFind(dummy3, out var foundRecord4);
        //Console.WriteLine(foundRecord4);

        //dynamicHashing.Insert(dummy);
        //dynamicHashing.Insert(dummy2);

    }
}
