using System.Collections.Generic;
using System.Linq;
using SaberComponents.Components;
using SabersCore.Models;
using UnityEngine;

namespace SabersCore.Utilities.Common;

public static class CustomTrailUtils
{
    /// <summary>
    /// Searches a GameObject's renderers for any materials that can be recolored.
    /// </summary>
    /// <param name="saberObject">The GameObject of the custom saber</param>
    /// <returns>A lazily evaluated sequence of found <see cref="Material"/>s.</returns>
    public static IEnumerable<Material> GetColorableSaberMaterials(GameObject saberObject)
    {
        foreach (var renderer in saberObject.GetComponentsInChildren<Renderer>(true))
        {
            if (renderer == null) continue;

            var materials = renderer.sharedMaterials;

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].IsColorable())
                {
                    materials[i] = new(materials[i]);
                    renderer.sharedMaterials = materials;
                    yield return materials[i];
                }
            }
        }
    }
    
    private static bool IsColorable(this Material material) =>
        material != null && material.HasProperty(MaterialProperties.Color) && material.HasColorableProperty();

    private static bool HasColorableProperty(this Material material) =>
        material.HasProperty(MaterialProperties.CustomColors) ? material.GetFloat(MaterialProperties.CustomColors) > 0
            : material.HasGlowOrBloom();

    private static bool HasGlowOrBloom(this Material material) =>
        material.HasProperty(MaterialProperties.Glow) && material.GetFloat(MaterialProperties.Glow) > 0
        || material.HasProperty(MaterialProperties.Bloom) && material.GetFloat(MaterialProperties.Bloom) > 0;
    
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
            useTrailColor: trail.useTrailColor,
            customColor: trail.trailColor,
            colorMultiplier: trail.multiplierColor,
            trailTopOffset: trail.top.position - saberObject.transform.position,
            trailBottomOffset: trail.bottom.position - saberObject.transform.position))
        .ToArray<ITrailData>();
}
