namespace SabersCore.Models;

/// <summary>
/// Provides the necessary information to color a custom SaberTrail
/// </summary>
public interface ITrailColorProvider
{
    /// <summary>
    /// Gives relevant information for coloring a trail material and a color from a color scheme.
    /// </summary>
    TrailColorInfo GetColorInfo(ColorScheme colorScheme);
    
    /// <summary>
    /// Gives relevant information for coloring a trail material and a color from a color scheme,
    /// accounting for boost colors.
    /// </summary>
    TrailColorInfo GetPropertiesWithBoostColors(ColorScheme colorScheme, bool boost);
    
    /// <summary>
    /// Gives relevant information for coloring a trail material to its default state.
    /// </summary>
    TrailColorInfo GetDefault();
}
