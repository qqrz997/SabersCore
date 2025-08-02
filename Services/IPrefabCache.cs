using System.Diagnostics.CodeAnalysis;
using SabersCore.Models;

namespace SabersCore.Services;

public interface IPrefabCache
{
    bool AddPrefab(ISaberData saberData);
    bool TryGetPrefab(string saberHash, [NotNullWhen(true)] out ISaberData? saberData);
    void UnloadPrefab(string saberHash);
    void Clear();
}