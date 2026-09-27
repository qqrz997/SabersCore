using System.Collections.Generic;

namespace SabersCore.Models;

/// <summary>
/// Provides the necessary information to color a custom SaberTrail
/// </summary>
public interface ITrailColorizer
{
    /// <summary>
    /// Gives each of the material's property names and the appropriate color to set.
    /// </summary>
    public IEnumerable<TrailColorInfo> GetPropertiesWithColors(ColorScheme colorScheme);
    
    /// <summary>
    /// Gives each of the material's property names and the appropriate color to set. Uses boost colors if necessary.
    /// </summary>
    public IEnumerable<TrailColorInfo> GetPropertiesWithBoostColors(
        ColorScheme colorScheme, bool boost);
    
    /// <summary>
    /// Gives only the property names of the trail's materials 
    /// </summary>
    public IEnumerable<TrailColorInfo> GetDefault();
}