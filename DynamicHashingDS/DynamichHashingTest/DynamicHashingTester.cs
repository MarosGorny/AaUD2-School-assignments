using DynamicHashingDS.Data;
using DynamicHashingDS.DH;
using System;
using System.Collections.Generic;

namespace DynamicHashingDS.DynamicHashingTest;
class DynamicHashingTester
{
    //private DynamicHashing<DummyClass> _dynamicHashing;
    private DynamicHashing<Osoba> _dynamicHashing;
    private List<Osoba> _insertedRecords;
    private Random _random;
    private double _insertProbability, _deleteProbability, _findProbability;

    private HashSet<string> _availableNamesSurnames;
    private bool _useSelectedIds = false;
    private int _idRange;
    private int _maxHashSize;
    private int _mainBlockFactor;
    private int _overflowBlockFactor;
    private int _blockSize;

    public DynamicHashingTester(int mainBlockFactor, int overflowBlockFactor, int maxHashSize, 
                                double insertProbability = 0.33, double deleteProbability = 0.33, double findProbability = 0.34, bool useSelectedIds = false)
    {
        InitializeDynamicHashing(mainBlockFactor, overflowBlockFactor, maxHashSize);
        InitializeProbabilities(insertProbability, deleteProbability, findProbability);
        InitializeTestEnvironment(useSelectedIds, mainBlockFactor, maxHashSize);

        //GetMainBlock size
        var mainBlock = new DHBlock<Osoba>(_mainBlockFactor, _maxHashSize);
        _blockSize = mainBlock.GetSize();
    }

    public void RunComplexTesting()
    {
        for (int i = 1; i < 21; i = i + 10)
        {

            for (int j = 1; j < 12; j = j + 5)
            {
                //DynamicHashingTester tester = new DynamicHashingTester(j, -1, i, useSelectedIds: true);
                //tester.RunRandomTest(10000, false);

                //DynamicHashingTester tester2 = new DynamicHashingTester(j, -1, i,
                //    insertProbability: 0.8,
                //    deleteProbability: 0.1,
                //    findProbability: 0.1,
                //    useSelectedIds: true);
                //tester2.RunRandomTest(10000,false);

                //DynamicHashingTester tester3 = new DynamicHashingTester(j, -1, i,
                //    insertProbability: 0.1,
                //    deleteProbability: 0.8,
                //    findProbability: 0.1,
                //    useSelectedIds: true);
                //tester3.InsertBatch(j * i);
                //tester2.RunRandomTest(10000, false);

                //DynamicHashingTester tester4 = new DynamicHashingTester(j, -1, i,
                //    insertProbability: 0.1,
                //    deleteProbability: 0.1,
                //    findProbability: 0.8,
                //    useSelectedIds: true);
                //tester4.InsertBatch(j * i);
                //tester2.RunRandomTest(10000, false);

                /////////////////////////////////
                
                for (int k = 1; k < 3; k++)
                {
                    DynamicHashingTester tester = new DynamicHashingTester(j, k, i, useSelectedIds: false);
                    tester.RunRandomTest(1000, true);

                    DynamicHashingTester tester2 = new DynamicHashingTester(j, k, i,
                        insertProbability: 0.8,
                        deleteProbability: 0.1,
                        findProbability: 0.1,
                        useSelectedIds: false);
                    tester2.RunRandomTest(1000, true);

                    DynamicHashingTester tester3 = new DynamicHashingTester(j, k, i,
                        insertProbability: 0.1,
                        deleteProbability: 0.8,
                        findProbability: 0.1,
                        useSelectedIds: false);
                    tester3.InsertBatch(j * i);
                    tester2.RunRandomTest(1000, true);

                    DynamicHashingTester tester4 = new DynamicHashingTester(j, k, i,
                        insertProbability: 0.1,
                        deleteProbability: 0.1,
                        findProbability: 0.8,
                        useSelectedIds: false);
                    tester4.InsertBatch(j * i);
                    tester2.RunRandomTest(1000, true);
                }



            }
        }
    }

    private void InitializeDynamicHashing(int mainBlockFactor, int overflowBlockFactor, int maxHashSize)
    {
        _dynamicHashing = new DynamicHashing<Osoba>(mainBlockFactor, overflowBlockFactor, "mainFilePath.dat", "overflowFilePath.dat", maxHashSize);

        _insertedRecords = new List<Osoba>();
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

        _availableNamesSurnames = new HashSet<string>();
        for(int i = 0; i < _idRange; i++)
        {
            _availableNamesSurnames.Add(GenerateRandomString(10) + GenerateRandomString(12));
        }
    }

    public Osoba GenerateRandomRecord()
    {
        return new Osoba
        {
            Meno = GenerateRandomString(10),
            Priezvisko = GenerateRandomString(12),
            Popis = GenerateRandomString(15),
        };
    }

    private Osoba GenerateRandomRecordFromIds()
    {
        if(_availableNamesSurnames.Count == 0 && _useSelectedIds)
        {
            Console.WriteLine("Trie is full !!");
            return null;
        }
        int randomIDIndex = _random.Next(_availableNamesSurnames.Count);
        string selectedNameSurname = _availableNamesSurnames.ElementAt(randomIDIndex);
        _availableNamesSurnames.Remove(selectedNameSurname);

        return new Osoba
        {
            Meno = GenerateRandomString(10),
            Priezvisko = GenerateRandomString(12),
            Popis = GenerateRandomString(15),
        };
    }

    public void RunRandomTest(int iterations, bool withOverFlowFile)
    {
        ResetAvailableIDsAndDynamicHashing();
        PerformRandomOperations(iterations, withOverFlowFile);
        VerifyStructure(withOverFlowFile);
    }

    private void ResetAvailableIDsAndDynamicHashing()
    {
        _insertedRecords.Clear();
        _availableNamesSurnames.Clear();
        _dynamicHashing = new DynamicHashing<Osoba>(_mainBlockFactor, _overflowBlockFactor, "mainFilePath.dat", "overflowFilePath.dat", _maxHashSize);
        for (int i = 0; i < _idRange; i++)
        {
            _availableNamesSurnames.Add(GenerateRandomString(10) + GenerateRandomString(12));
        }
    }

    private void PerformRandomOperations(int iterations, bool withOverFlowFile)
    {
        for (int i = 0; i < iterations; i++)
        {
            //13 where it's bugged
            if(i == 214)
            {
                //12 external has wrong parent
                Console.WriteLine(  );
            }

            PerformRandomOperation(i,withOverFlowFile);
        }
    }

    private void PerformRandomOperation(int iteration, bool withOverflowFile)
    {
        double action = _random.NextDouble();
        Console.Write($"I:{iteration + 1} ");

        var structureRecords = _dynamicHashing.FileBlockManager.GetAllRecords(withOverflowFile);
        if (_insertedRecords.Count != structureRecords.Count)
        {
           throw new Exception("Verification failed! Lists contain different items.");
        }

        double maxFileSize = Math.Pow(2, _maxHashSize) * _blockSize;
        if (_dynamicHashing.FileBlockManager.CurrentMainFileSize > maxFileSize)
        {
            Console.WriteLine("Main file is too big!");
            throw new Exception("Main file is too big!");
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
        if (record == null)
        {
            return;
        }
        Console.WriteLine($"Inserted: {record}");
        _dynamicHashing.Insert(record);
        _insertedRecords.Add(record);
        
    }

    public void DeleteRandomRecord()
    {
        if (!_useSelectedIds || _insertedRecords.Count == 0)
        {
            var record = GenerateRandomRecord();
            var deletedRecord =  _dynamicHashing.Delete(record);

            if(deletedRecord != null)
            {
                Console.WriteLine($"Deleted record: {deletedRecord}");
                _insertedRecords.Remove((Osoba)deletedRecord);
            }
            else
            {
                Console.WriteLine($"Record deletion not found: {record}");
            }

        } 
        else
        {
            int index = _random.Next(_insertedRecords.Count);
            var record = _insertedRecords[index];

            Console.WriteLine($"Deleted: {record}");
            _dynamicHashing.Delete(record);
            _insertedRecords.RemoveAt(index);
            _availableNamesSurnames.Add(record.Meno + record.Priezvisko);
            
        }
    }

    public void FindRandomRecord()
    {
        bool found = false;
        IDHRecord<Osoba> foundRecord = null;
        Osoba record = null;

        if (!_useSelectedIds || _insertedRecords.Count == 0)
        {
            record = GenerateRandomRecord();
            found = _dynamicHashing.TryFind(record, out foundRecord);
            Console.WriteLine($"Find record {record}: {foundRecord}");
        }
        else
        {

            int index = _random.Next(_insertedRecords.Count);
            record = _insertedRecords[index];

            found = _dynamicHashing.TryFind(record, out foundRecord);


            Console.WriteLine($"Found: {foundRecord}");
        }

        if (_insertedRecords.Contains(record) && !found)
        {
            Console.WriteLine($"Record not found, even it's inside: {foundRecord}");
            throw new Exception("Record not found event it's inside!");
            return;
        }

        if (foundRecord is not null && !foundRecord.MyEquals(record))
        {
            throw new Exception("Found record is not the same as the searched record!");
        }
    }

    private string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[_random.Next(s.Length)]).ToArray());
    }


    public List<Osoba> InsertBatch(int count)
    {
        List<Osoba> batchRecords = new List<Osoba>();
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


    public void VerifyStructure(bool withOverFlowFile)
    {
        int insertedRecordsCount = _insertedRecords.Count;
        var structureRecords = _dynamicHashing.FileBlockManager.GetAllRecords(withOverFlowFile);
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
                //if (item.MyEquals((DummyClass)item2))
                if (item.MyEquals((Osoba)item2))
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
