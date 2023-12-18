using DynamicHashingDS.Data;
using DynamicHashingDS.Nodes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicHashingDS.DH;

public class DynamicHashingExport<T> where T : IDHRecord<T>, new()
{
    public int MainBlockFactor { get; set; }
    public int OverflowBlockFactor { get; set; }
    public string MainFilePath { get; set; }
    public string OverflowFilePath { get; set; }
    public int MaxHashSize { get; set; }
    public DHNode<T> RootNode { get; set; }

    public void ExportToFile(DynamicHashing<T> dynamicHashing, string filePath)
    {
        MainBlockFactor = dynamicHashing.MainBlockFactor;
        OverflowBlockFactor = dynamicHashing.OverflowBlockFactor;
        MainFilePath = dynamicHashing.MainFilePath;
        OverflowFilePath = dynamicHashing.OverflowFilePath;
        MaxHashSize = dynamicHashing.MaxHashSize;
        RootNode = dynamicHashing.Root;

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Objects
        };

        string json = JsonConvert.SerializeObject(this, settings);
        File.WriteAllText(filePath, json);
    }

    public static DynamicHashing<T> ImportFromFile(string filePath)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects // Use type information during deserialization
        };

        string json = File.ReadAllText(filePath);
        var export = JsonConvert.DeserializeObject<DynamicHashingExport<T>>(json, settings);

        var dynamicHashing = new DynamicHashing<T>(
            export.MainBlockFactor,
            export.OverflowBlockFactor,
            export.MainFilePath,
            export.OverflowFilePath,
            export.MaxHashSize);

        dynamicHashing.Root = export.RootNode;
        return dynamicHashing;
    }
}
