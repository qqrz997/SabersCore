using System.Collections.Generic;
using Newtonsoft.Json;

namespace SabersCore.Models;

internal class Saber2Model
{
    public string IconPath;
    public string ModelName;
    public string AuthorName;
    public Dictionary<AssetPlatform, AssetMetadata> Assets;

    [JsonConstructor]
    public Saber2Model(string iconPath,
        string modelName,
        string authorName,
        Dictionary<AssetPlatform, AssetMetadata> assets)
    {
        IconPath = iconPath;
        ModelName = modelName;
        AuthorName = authorName;
        Assets = assets;
    }
}