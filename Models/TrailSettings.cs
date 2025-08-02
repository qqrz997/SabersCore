namespace SabersCore.Models;

public record struct TrailSettings(
    bool DisableWhiteTrail,
    bool OverrideTrailWidth,
    float TrailWidthPercent,
    bool OverrideTrailDuration,
    float TrailDurationPercent);