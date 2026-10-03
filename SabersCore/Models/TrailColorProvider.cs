using System.Collections.Generic;
using System.Linq;
using AssetComponents.Components.Sabers;
using AssetComponents.Extensions;
using AssetComponents.Models;
using SabersCore.Utilities.Extensions;
using UnityEngine;

namespace SabersCore.Models;

public class TrailColorProvider : ITrailColorProvider
{
    private readonly Material trailMaterial;
    private readonly int materialIndex;
    private readonly string propertyName;
    private readonly ColorSchemeType colorSchemeType;
    private readonly bool applyToVertexColor;

    private readonly TrailColorInfo defaultColorInfo;
    
    public TrailColorProvider(TrailColorer trailColorer, bool mirrorColor)
    {
        trailMaterial = trailColorer.Material;
        materialIndex = trailColorer.MaterialIndex;
        propertyName = trailColorer.PropertyName;
        colorSchemeType = mirrorColor ? trailColorer.ColorSchemeType.GetMirrored() : trailColorer.ColorSchemeType;
        applyToVertexColor = trailColorer.ApplyToVertexColor;

        defaultColorInfo = new(trailColorer);
    }


    public TrailColorInfo GetColorInfo(ColorScheme colorScheme) => new(
        trailMaterial,
        materialIndex, 
        propertyName,
        colorScheme.GetColorByType(colorSchemeType),
        applyToVertexColor);

    public TrailColorInfo GetPropertiesWithBoostColors(ColorScheme colorScheme, bool boost) => new(
        trailMaterial,
        materialIndex,
        propertyName,
        colorScheme.GetBoostColorByType(colorSchemeType, boost),
        applyToVertexColor);

    public TrailColorInfo GetDefault() => 
        defaultColorInfo;
}
