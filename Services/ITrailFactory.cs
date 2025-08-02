using SabersCore.Components;
using SabersCore.Models;

namespace SabersCore.Services;

public interface ITrailFactory
{
    /// <summary>
    /// Adds trails to a custom saber.
    /// </summary>
    /// <param name="saber">The saber to add the trails to.</param>
    /// <param name="trails">The trail data to create the trails with.</param>
    /// <param name="intensity">The alpha of the trail. Only works with certain shaders.</param>
    /// <returns>An array containing the new trail instances. Returns an empty array if none are created.</returns>
    CustomSaberTrail[] AddTrailsTo(ISaber saber, ITrailData[] trails, float intensity);

    DefaultTrailData CreateDefaultTrailData();
}