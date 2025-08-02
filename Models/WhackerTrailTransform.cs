using Newtonsoft.Json;

namespace SabersCore.Models;

internal struct WhackerTrailTransform
{
    public int TrailId { get; }
    public bool IsTop { get; }

    [JsonConstructor]
    public WhackerTrailTransform(
        int trailId,
        bool isTop)
    {
        TrailId = trailId;
        IsTop = isTop;
    }
}
