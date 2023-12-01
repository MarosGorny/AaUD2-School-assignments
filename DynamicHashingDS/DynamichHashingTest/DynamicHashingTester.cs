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
        InitializeDynamicHashing(mainBlockFactor, overflowBlockFactor, maxHashSize);
        InitializeProbabilities(insertProbability, deleteProbability, findProbability);
        InitializeTestEnvironment(useSelectedIds, mainBlockFactor, maxHashSize);
    }

    public void RunComplexTesting()
    {
        for (int i = 1; i < 21; i = i + 10)
        {

            for (int j = 1; j < 12; j = j + 5)
            {
                DynamicHashingTester tester = new DynamicHashingTester(j, -1, i, useSelectedIds: true);
                tester.RunRandomTest(10000);

                DynamicHashingTester tester2 = new DynamicHashingTester(j, -1, i,
                    insertProbability: 0.8,
                    deleteProbability: 0.1,
                    findProbability: 0.1,
                    useSelectedIds: true);
                tester2.RunRandomTest(10000);

                DynamicHashingTester tester3 = new DynamicHashingTester(j, -1, i,
                    insertProbability: 0.1,
                    deleteProbability: 0.8,
                    findProbability: 0.1,
                    useSelectedIds: true);
                tester3.InsertBatch(j * i);
                tester2.RunRandomTest(10000);

                DynamicHashingTester tester4 = new DynamicHashingTester(j, -1, i,
                    insertProbability: 0.1,
                    deleteProbability: 0.1,
                    findProbability: 0.8,
                    useSelectedIds: true);
                tester4.InsertBatch(j * i);
                tester2.RunRandomTest(10000);

            }
        }
    }

    private void InitializeDynamicHashing(int mainBlockFactor, int overflowBlockFactor, int maxHashSize)
    {
        _dynamicHashing = new DynamicHashing<DummyClass>(mainBlockFactor, overflowBlockFactor, "mainFilePath.dat", "overflowFilePath.dat", maxHashSize);
        _insertedRecords = new List<DummyClass>();
        _random = new Random(2);
        _mainBlockFactor = mainBlockFactor;
        _overflowBlockFactor = overflowBlockFactor;
        _maxHashSize = maxHashSize;
    }

    private void InitializeProbabilities(double insertProbability, double deleteProbability, double findProbability)
    {
        _insertProbability = insertProbability;
        _deleteProbability = deleteProbability + insertProbability;
        _findProbability = findProbability; // The rest is for the find operation
    }

    private void InitializeTestEnvironment(bool useSelectedIds, int mainBlockFactor, int maxHashSize)
    {
        _useSelectedIds = useSelectedIds;
        _idRange = mainBlockFactor * maxHashSize;
        _availableIDs = new HashSet<int>();
        for (int i = 0; i < _idRange; i++)
        {
            _availableIDs.Add(i);
        }
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

    private DummyClass GenerateRandomRecordFromIds()
    {
        if (_availableIDs.Count == 0 && _useSelectedIds)
        {
            Console.WriteLine("Trie is full !!");
            return null;
        }
        int randomIDIndex = _random.Next(_availableIDs.Count);
        int selectedID = _availableIDs.ElementAt(randomIDIndex);
        _availableIDs.Remove(selectedID);

        return new DummyClass
        {
            Cislo = selectedID,
            ID = _random.Next(0, 1000),
            Text = GenerateRandomString(14)
        };
    }

    public void RunRandomTest(int iterations)
    {
        ResetAvailableIDsAndDynamicHashing();
        PerformRandomOperations(iterations);
        VerifyStructure();
    }

    private void ResetAvailableIDsAndDynamicHashing()
    {
        _insertedRecords.Clear();
        _availableIDs.Clear();
        _dynamicHashing = new DynamicHashing<DummyClass>(_mainBlockFactor, _overflowBlockFactor, "mainFilePath.dat", "overflowFilePath.dat", _maxHashSize);
        for (int i = 0; i < _idRange; i++)
        {
            _availableIDs.Add(i);
        }
    }

    private void PerformRandomOperations(int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            //13 where it's bugged
            if(i == 12)
            {
                //12 external has wrong parent
                Console.WriteLine(  );
            }

            PerformRandomOperation(i);
        }
    }

    private void PerformRandomOperation(int iteration)
    {
        double action = _random.NextDouble();
        Console.Write($"I:{iteration + 1} ");

        var structureRecords = _dynamicHashing.FileBlockManager.GetAllRecords(false);
        if (_insertedRecords.Count != structureRecords.Count)
        {
           throw new Exception("Verification failed! Lists contain different items.");
        }

        if (action < _insertProbability)
            InsertRandomRecord();
        else if (action < _deleteProbability)
            DeleteRandomRecord();
        else
            FindRandomRecord();
    }
    public void InsertRandomRecord()
    {
        var record = _useSelectedIds ? GenerateRandomRecordFromIds() : GenerateRandomRecord();
        if(record == null)
        {
            //Console.WriteLine("Record is null!");
            return;
        }
        Console.WriteLine($"Inserted: {record}");
        _dynamicHashing.Insert(record);
        _insertedRecords.Add(record);
        
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

            Console.WriteLine($"Deleted: {record}");
            _dynamicHashing.Delete(record);
            _insertedRecords.RemoveAt(index);
            _availableIDs.Add(record.Cislo); // Add the deleted ID to the available IDs
            
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




        var dynamicHashing = new DynamicHashing<DummyClass>(2, 2, "testMain.bin", "testFlow.bin", 2);

        dynamicHashing.TryFind(dummy1, out var foundRecord);
        Console.WriteLine("Found: " + foundRecord);
        //Console.WriteLine("Deleted: " + dynamicHashing.Delete(dummy1));


        dynamicHashing.Insert(dummy1);

        dynamicHashing.TryFind(dummy1, out var foundRecord1);
        Console.WriteLine("Found: " + foundRecord1);
        //Console.WriteLine("Deleted: " + dynamicHashing.Delete(dummy1));

        dynamicHashing.Insert(dummy1);

        dynamicHashing.TryFind(dummy3, out var foundRecord3);
        Console.WriteLine(foundRecord3);

        dynamicHashing.Insert(dummy3);
        //Console.WriteLine("Deleted: " + dynamicHashing.Delete(dummy3));
        dynamicHashing.Insert(dummy3);

        dynamicHashing.TryFind(dummy3, out var foundRecord4);
        Console.WriteLine(foundRecord4);

        dynamicHashing.Insert(dummy);
        dynamicHashing.Insert(dummy2);


        var records = dynamicHashing.FileBlockManager.GetAllRecords(false);
        //Console.WriteLine("Deleted: " + dynamicHashing.Delete(dummy));


        //var block = new DHBlock<DummyClass>(3,3);
        //block.AddRecord(dummy);
        //block.AddRecord(dummy2);
        //block.AddRecord(dummy3);
        //block.DeleteRecord(dummy2);
        //block.DeleteRecord(dummy);
        //block.WriteToBinaryFile("test.bin");
        //block.ReadFromBinaryFile("test.bin");
        //Console.WriteLine("Hello World!");
    }
}
