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
    public static ITrailData[] GetTrailsFromCustomSaber(GameObject saberObject) => saberObject
        .GetComponentsInChildren<CustomTrail>()
        .Where(ct => ct.top != null && ct.bottom != null) // is the CustomTrail valid?
        .Select(ct => new CustomTrailData(
            trailTopOffset: ct.top.position - saberObject.transform.position,
            trailBottomOffset: ct.bottom.position - saberObject.transform.position,
            materials: ct.materials,
            lengthSeconds: ct.length,
            colorizer: new TrailColorerTrailColorizer(ct.GetComponents<TrailColorer>())))
        .ToArray<ITrailData>();
}
