using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SabersCore.Models;

namespace SabersCore.Services;

internal class CustomSaberPrefabCache : IDisposable, IPrefabCache
{
    private readonly Dictionary<string, CustomSaberData> cache = [];

    public bool AddPrefab(ISaberData saberData) =>
        saberData is CustomSaberData x && cache.TryAdd(x.Metadata.SaberFile.Hash, x);

    public bool TryGetPrefab(string saberHash, [NotNullWhen(true)] out ISaberData? saberData)
    {
        if (cache.TryGetValue(saberHash, out var cached))
        {
            saberData = cached;
            return true;
        }
        saberData = null;
        return false;
    }

    public void UnloadPrefab(string saberHash)
    {
        if (!cache.TryGetValue(saberHash, out var saberData)) return;
        saberData.Dispose();
        cache.Remove(saberHash);
    }

    public void Dispose()
    {
        Clear();
    }
    
    public void Clear()
    {
        if (cache.Count == 0) return;
        foreach (var customSaberData in cache.Values) customSaberData.Dispose();
        cache.Clear();
    }
}
