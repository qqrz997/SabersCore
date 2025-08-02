using SabersCore.Utilities.Common;
using SabersCore.Utilities.Extensions;
using UnityEngine;

namespace SabersCore.Models;

internal class WhackerPrefab : ISaberPrefab
{
    private readonly GameObject prefab;
    private readonly ITrailData[] leftTrails = [];
    private readonly ITrailData[] rightTrails = [];

    public WhackerPrefab(GameObject prefab)
    {
        this.prefab = prefab;

        var leftSaber = prefab.transform.Find("LeftSaber");
        if (leftSaber != null) leftTrails = CustomTrailUtils.GetTrailsFromWhacker(leftSaber.gameObject);
        else Plugin.Log.Warn($"Prefab \"{prefab.name}\" is missing a LeftSaber GameObject");
        
        var rightSaber = prefab.transform.Find("RightSaber");
        if (rightSaber != null) rightTrails = CustomTrailUtils.GetTrailsFromWhacker(rightSaber.gameObject); 
        else Plugin.Log.Warn($"Prefab \"{prefab.name}\" is missing a RightSaber GameObject");
    }

    public SaberInstanceSet Instantiate() => new(prefab);
    public ITrailData[] GetTrailsForType(SaberType saberType) => 
        saberType == SaberType.SaberA ? leftTrails : rightTrails;

    public void Dispose()
    {
        if (prefab != null) prefab.Destroy();
    }
}