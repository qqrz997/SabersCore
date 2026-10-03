using AssetComponents.Components.Sabers;
using AssetComponents.Models;
using SabersCore.Utilities.Common;
using SabersCore.Utilities.Extensions;
using UnityEngine;

namespace SabersCore.Models;

internal class CustomSaberPrefab : ISaberPrefab
{
    private readonly SaberDescriptor prefab;
    private readonly ITrailData[] leftTrails = [];
    private readonly ITrailData[] rightTrails = [];

    public CustomSaberPrefab(SaberDescriptor prefab)
    {
        this.prefab = prefab;

        if (prefab.leftSaber == null)
        {
            Plugin.Log.Warn($"Invalid saber! Prefab \"{prefab.name}\" is missing a LeftSaber GameObject");
            return;
        }

        leftTrails = CustomTrailUtils.GetTrailsFromCustomSaber(prefab.leftSaber);
        rightTrails = prefab.rightSaber != null ? CustomTrailUtils.GetTrailsFromCustomSaber(prefab.rightSaber) 
            : CustomTrailUtils.GetTrailsFromCustomSaber(prefab.leftSaber, true);
    }

    public SaberInstanceSet Instantiate()
    {
        var root = Object.Instantiate(prefab);
        var leftSaber = new CustomSaber(root.leftSaber);
        var rightSaber = new CustomSaber(root.rightSaber ? root.rightSaber : MirrorSaber(root.leftSaber));
        return new(leftSaber, rightSaber, leftTrails, rightTrails);
    }

    public ITrailData[] GetTrailsForType(SaberType saberType) => 
        saberType == SaberType.SaberA ? leftTrails : rightTrails;

    public void Dispose()
    {
        if (prefab != null) prefab.gameObject.Destroy();
    }
        
    private static GameObject MirrorSaber(GameObject saber)
    {
        var mirrored = Object.Instantiate(saber, saber.transform.parent, false);
        foreach (var colorer in mirrored.GetComponentsInChildren<IColorer>(true))
        {
            colorer.MirrorColorType();
        }
        return mirrored;
    }
}