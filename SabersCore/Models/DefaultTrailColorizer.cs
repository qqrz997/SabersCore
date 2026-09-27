using System.Collections.Generic;
using AssetComponents.Models;
using SabersCore.Utilities.Extensions;
using UnityEngine;

namespace SabersCore.Models;

public class DefaultTrailColorizer : ITrailColorizer
{
    private const string ColorPropertyName = "_Color";

    private readonly Material material;
    private readonly SaberType saberType;
    
    public DefaultTrailColorizer(Material material, SaberType saberType)
    {
        this.material = material;
        this.saberType = saberType;
    }

    public IEnumerable<TrailColorInfo> GetPropertiesWithBoostColors(ColorScheme colorScheme, bool boost) =>
        GetPropertiesWithColors(colorScheme);

    public IEnumerable<TrailColorInfo> GetPropertiesWithColors(ColorScheme colorScheme)
    {
        var colorSchemeType = saberType is SaberType.SaberA ? ColorSchemeType.LeftSaber : ColorSchemeType.RightSaber;
        yield return new(material, ColorPropertyName, colorScheme.GetColorByType(colorSchemeType), true);
    }

    public IEnumerable<TrailColorInfo> GetDefault()
    {
        yield return new(material, ColorPropertyName, Color.white, true);
    }
}