using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SabersCore.Models;

namespace SabersCore.Services;

internal class SaberMetadataCache : ISaberMetadataCache
{
    private readonly Dictionary<string, CustomSaberMetadata> cache = [];

    public bool TryAdd(CustomSaberMetadata saberMetadata) =>
        cache.TryAdd(saberMetadata.SaberFile.Hash, saberMetadata);

    public void Remove(string saberHash) =>
        cache.Remove(saberHash);

    public bool TryGetMetadata(string? saberHash, [NotNullWhen(true)] out CustomSaberMetadata? meta) => 
        (meta = GetOrDefault(saberHash)) != null;

    public CustomSaberMetadata? GetOrDefault(string? saberHash)
    {
        if (saberHash is null || !cache.TryGetValue(saberHash, out var meta))
        {
            return null;
        }
        
        meta.SaberFile.FileInfo.Refresh();
        
        if (!meta.SaberFile.FileInfo.Exists)
        {
            Remove(saberHash);
            return null;
        }
        
        return meta;
    }

    public void Clear() => 
        cache.Clear();

    public IEnumerable<CustomSaberMetadata> GetRefreshedMetadata()
    {
        foreach (var meta in cache.Values)
        {
            meta.SaberFile.FileInfo.Refresh();
            yield return meta;
        }
    }
}