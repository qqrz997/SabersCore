using System.Collections.Generic;
using System.Linq;
using AssetComponents.Components.Sabers;
using SabersCore.Utilities.Extensions;

namespace SabersCore.Models;

public class TrailColorerTrailColorizer : ITrailColorizer
{
    private readonly TrailColorer[] trailColorers;

    public TrailColorerTrailColorizer(TrailColorer[] trailColorers)
    {
        this.trailColorers = trailColorers;
    }

    public IEnumerable<TrailColorInfo> GetPropertiesWithColors(ColorScheme colorScheme) => 
        trailColorers.Select(tc => new TrailColorInfo(
            tc, colorScheme.GetColorByType(tc.ColorSchemeType) * tc.MultiplierColor));

    public IEnumerable<TrailColorInfo> GetPropertiesWithBoostColors(ColorScheme colorScheme, bool boost) =>
        trailColorers.Select(tc => new TrailColorInfo(
            tc, colorScheme.GetBoostColorByType(tc.ColorSchemeType, boost) * tc.MultiplierColor));

    public IEnumerable<TrailColorInfo> GetDefault() => 
        trailColorers.Select(tc => new TrailColorInfo(tc));
}