using System.Collections.Generic;

namespace SabersCore.Models;

/// <summary>
/// Provides the necessary information to color a custom SaberTrail
/// </summary>
public interface ITrailColorizer
{
    /// <summary>
    /// Gives relevant information for coloring a trail and a color from a color scheme.
    /// </summary>
    public IEnumerable<TrailColorInfo> GetPropertiesWithColors(ColorScheme colorScheme);
    
    /// <summary>
    /// Gives relevant information for coloring a trail and a color from a color scheme, accounting for boost colors.
    /// </summary>
    public IEnumerable<TrailColorInfo> GetPropertiesWithBoostColors(
        ColorScheme colorScheme, bool boost);
    
    /// <summary>
    /// Gives relevant information for coloring a trail to its default state.
    /// </summary>
    public IEnumerable<TrailColorInfo> GetDefault();
}