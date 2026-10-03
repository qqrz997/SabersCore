using System.Linq;
using AssetComponents.Components.Sabers;
using SabersCore.Models;
using UnityEngine;

namespace SabersCore.Utilities.Common;

public static class CustomTrailUtils
{
    /// <summary>
    /// Searches a CustomSaber's GameObject for any custom trails.
    /// </summary>
    /// <param name="saberObject">The GameObject of the custom saber</param>
    /// <param name="mirrorColors">Whether to mirror the colors to the opposite hand</param>
    public static ITrailData[] GetTrailsFromCustomSaber(GameObject saberObject, bool mirrorColors = false) => saberObject
        .GetComponentsInChildren<CustomTrail>()
        .Where(ct => ct.top != null && ct.bottom != null) // is the CustomTrail valid?
        .Select(ct => new CustomTrailData(
            trailTopOffset: ct.top.position - saberObject.transform.position,
            trailBottomOffset: ct.bottom.position - saberObject.transform.position,
            materials: ct.materials,
            lengthSeconds: ct.length,
            colorProviders: ct.GetColorProviders(mirrorColors)))
        .ToArray<ITrailData>();

    private static ITrailColorProvider[] GetColorProviders(this CustomTrail ct, bool mirrorColors) => ct
        .GetComponents<TrailColorer>()
        .Select(tc => new TrailColorProvider(tc, mirrorColors))
        .ToArray<ITrailColorProvider>();
}
