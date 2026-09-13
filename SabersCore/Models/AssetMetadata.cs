using Newtonsoft.Json;

namespace SabersCore.Models;

internal class AssetMetadata
{
    public string FilePath;

    [JsonConstructor]
    public AssetMetadata(string filePath)
    {
        FilePath = filePath;
    }
}