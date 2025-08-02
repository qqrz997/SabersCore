using System;

namespace SabersCore.Models;

public interface ISaberPrefab : IDisposable
{
    public SaberInstanceSet Instantiate();
    public ITrailData[] GetTrailsForType(SaberType saberType);
}