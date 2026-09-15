using System;
using SaberComponents.Models;
using UnityEngine;

namespace SabersCore.Utilities.Extensions;

internal static class SaberColoring
{
    public static Color GetColorByType(this ColorScheme s, ColorSchemeType t) => t switch
    {
        ColorSchemeType.LeftSaber => s.saberAColor,
        ColorSchemeType.RightSaber => s.saberBColor,
        ColorSchemeType.EnvironmentColor0 => s.environmentColor0,
        ColorSchemeType.EnvironmentColor1 => s.environmentColor1,
        ColorSchemeType.EnvironmentColorW => s.environmentColorW,
        ColorSchemeType.EnvironmentColor0Boost => s.environmentColor0Boost,
        ColorSchemeType.EnvironmentColor1Boost => s.environmentColor1Boost,
        ColorSchemeType.EnvironmentColorWBoost => s.environmentColorWBoost,
        ColorSchemeType.ObstaclesColor => s.obstaclesColor,
        _ => throw new ArgumentOutOfRangeException()
    };
    
    public static void SetNewColor(this SetSaberGlowColor setSaberGlowColor, Color color)
    {
        var materialPropertyBlock = setSaberGlowColor._materialPropertyBlock ?? new MaterialPropertyBlock();
        foreach (var pair in setSaberGlowColor._propertyTintColorPairs)
        {
            materialPropertyBlock.SetColor(pair.property, color * pair.tintColor);
        }
        setSaberGlowColor._meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }

    public static void SetNewColor(this SetSaberFakeGlowColor setSaberFakeGlowColor, Color color)
    {
        setSaberFakeGlowColor._parametric3SliceSprite.color = color;
        setSaberFakeGlowColor._parametric3SliceSprite.Refresh();
    }
}
