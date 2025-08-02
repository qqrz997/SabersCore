using System;

namespace SabersCore.Models;

public interface ISaberData : IDisposable
{
    public CustomSaberMetadata Metadata { get; }
    public ISaberPrefab? Prefab { get; }
}