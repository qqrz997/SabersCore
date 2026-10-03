using System.Collections.Generic;
using AssetComponents.Models;
using SabersCore.Utilities.Extensions;
using UnityEngine;

namespace SabersCore.Models;

public class DefaultTrailColorProvider : ITrailColorProvider
{
    private const string ColorPropertyName = "_Color";

    private readonly Material material;
    private readonly SaberType saberType;
    
    private readonly TrailColorInfo defaultColorInfo;
    
    public DefaultTrailColorProvider(Material material, SaberType saberType)
    {
        this.material = material;
        this.saberType = saberType;
        
        defaultColorInfo = new(material, 0, ColorPropertyName, Color.white, true);
    }

    public TrailColorInfo GetColorInfo(ColorScheme colorScheme)
    {
        var colorSchemeType = saberType is SaberType.SaberA ? ColorSchemeType.LeftSaber : ColorSchemeType.RightSaber;
        return new(material, 0, ColorPropertyName, colorScheme.GetColorByType(colorSchemeType), true);
    }

    public TrailColorInfo GetPropertiesWithBoostColors(ColorScheme colorScheme, bool boost) => 
        GetColorInfo(colorScheme);

    public TrailColorInfo GetDefault() =>
        defaultColorInfo;
}