using System.Collections.Generic;
using SabersCore.Components;
using SabersCore.Models;
using SabersCore.Utilities.Extensions;

namespace SabersCore.Utilities.Common;

public static class TrailUtils
{
    /// <summary>
    /// Legacy trail duration in frames
    /// </summary>
    public const int LegacyDuration = 30;

    /// <summary>
    /// Default duration of the saber trail in seconds
    /// </summary>
    public const float DefaultDuration = 0.4f;

    /// <summary>
    /// Converts legacy trail Length in frames to trail duration in seconds used by <see cref="SaberTrail"/>
    /// </summary>
    /// <param name="customTrailLength">Trail Length from a CustomTrail, in frames</param>
    /// <returns>Trail duration in seconds</returns>
    public static float ConvertLegacyLength(int customTrailLength) =>
        customTrailLength / (float)LegacyDuration * DefaultDuration;

    /// <summary>
    /// Configures a collection of trails. The first trail in the sequence will be enabled for overriding width, any
    /// other trails will not be able to override width.
    /// </summary>
    /// <param name="trails">The trails to configure</param>
    /// <param name="settings">The settings to use</param>
    public static void ConfigureTrails(this IList<CustomSaberTrail> trails, in TrailSettings settings)
    {
        for (int i = 0; i < trails.Count; i++)
        {
            var trail = trails[i];
            if (trail != null) trail.ConfigureTrail(settings, i == 0);
        }
    }

    private static void ConfigureTrail(this CustomSaberTrail trail, in TrailSettings settings, bool useOverrideWidth)
    {
        if (trail._trailRenderer == null)
        {
            return;
        }

        trail._whiteSectionMaxDuration = settings.DisableWhiteTrail ? 0f : 0.03f;
        trail._framesPassed = 0;
        trail._framesToScaleCheck = 0;
        trail._inited = false;

        trail.OverrideWidth = settings.TrailWidthPercent;
        trail.UseWidthOverride = settings.OverrideTrailWidth && useOverrideWidth;

        trail._trailDuration = settings.OverrideTrailDuration ? settings.TrailDurationPercent * DefaultDuration
            : trail.TrailData.LengthSeconds;

        trail.enabled = !trail._trailDuration.Approximately(0f);
    }
}