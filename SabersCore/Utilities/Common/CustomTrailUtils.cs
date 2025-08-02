using System.Collections.Generic;
using System.Linq;
using CustomSaber;
using Newtonsoft.Json;
using SabersCore.Models;
using UnityEngine;
using UnityEngine.UI;
using static SabersCore.Utilities.Common.TrailUtils;

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
        .Where(ct => ct.PointEnd != null && ct.PointStart != null) // is the CustomTrail valid?
        .Select(trail => new CustomTrailData(
            material: trail.TrailMaterial,
            lengthSeconds: ConvertLegacyLength(trail.Length),
            colorType: trail.colorType,
            customColor: trail.TrailColor,
            colorMultiplier: trail.MultiplierColor,
            trailTopOffset: trail.PointEnd.position - saberObject.transform.position,
            trailBottomOffset: trail.PointStart.position - saberObject.transform.position))
        .ToArray<ITrailData>();
    
    /// <summary>
    /// Searches a Whacker's GameObject for any custom trails.
    /// </summary>
    /// <param name="saberObject">The GameObject of the custom saber</param>
    public static ITrailData[] GetTrailsFromWhacker(GameObject saberObject)
    {
        var texts = saberObject.GetComponentsInChildren<Text>();
        var transformData = texts
            .Where(text => text.text.Contains("\"IsTop\":"))
            .Select(text => (
                Transform: text.transform, 
                Data: JsonConvert.DeserializeObject<WhackerTrailTransform>(text.text)))
            .ToList();
        var trailData = texts
            .Where(t => t.text.Contains("\"TrailColor\":"))
            .Select(text => (
                Material: text.GetComponent<MeshRenderer>().material,
                Data: JsonConvert.DeserializeObject<WhackerTrail>(text.text)))
            .Where(td => td.Data is not null);
        
        // search the transform data for each trail and find the matching transform data,
        // and take the transform from which that transform data originated from
        return trailData
            .Select(trail => new CustomTrailData(
                material: trail.Material,
                lengthSeconds: ConvertLegacyLength(trail.Data!.Length),
                colorType: trail.Data!.ColorType,
                customColor: trail.Data.TrailColor,
                colorMultiplier: trail.Data.MultiplierColor,
                saberObjectRoot: saberObject,
                trailTop: transformData.Where(transform => transform.Data.IsTop)
                    .FirstOrDefault(transform => transform.Data.TrailId == trail.Data!.TrailId)
                    .Transform,
                trailBottom: transformData.Where(transform => !transform.Data.IsTop)
                    .FirstOrDefault(transform => transform.Data.TrailId == trail.Data!.TrailId)
                    .Transform))
            .ToArray<ITrailData>();
    }
}
