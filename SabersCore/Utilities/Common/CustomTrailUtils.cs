using System.Collections.Generic;
using System.Linq;
using SaberComponents.Components;
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
        .Select(trail => new CustomTrailData(
            material: trail.material,
            lengthSeconds: trail.length,
            colorSchemeType: trail.colorSchemeType,
            useColorBoostEvents: trail.useColorBoostEvents,
            useTrailColor: trail.useTrailColor,
            customColor: trail.trailColor,
            colorMultiplier: trail.multiplierColor,
            trailTopOffset: trail.top.position - saberObject.transform.position,
            trailBottomOffset: trail.bottom.position - saberObject.transform.position))
        .ToArray<ITrailData>();
}
