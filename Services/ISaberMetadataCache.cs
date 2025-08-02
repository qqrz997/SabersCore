using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SabersCore.Models;

namespace SabersCore.Services;

public interface ISaberMetadataCache
{
    bool TryAdd(CustomSaberMetadata saberMetadata);
    void Remove(string saberHash);
    bool TryGetMetadata(string? saberHash, [NotNullWhen(true)] out CustomSaberMetadata? meta);
    CustomSaberMetadata? GetOrDefault(string? saberHash);
    void Clear();
    IEnumerable<CustomSaberMetadata> GetRefreshedMetadata();
}