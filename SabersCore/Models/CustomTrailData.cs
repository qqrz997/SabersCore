using AssetComponents.Models;
using UnityEngine;

namespace SabersCore.Models;

internal class CustomTrailData : ITrailData
{
    public CustomTrailData(
        Vector3 trailTopOffset,
        Vector3 trailBottomOffset,
        Material[] materials,
        float lengthSeconds,
        ITrailColorizer colorizer)
    {
        TrailTopOffset = trailTopOffset;
        TrailBottomOffset = trailBottomOffset;
        Materials = materials;
        LengthSeconds = lengthSeconds;
        Colorizer = colorizer;
    }


    public Vector3 TrailTopOffset { get; }
    public Vector3 TrailBottomOffset { get; }
    public Material[] Materials { get; }
    
    public float LengthSeconds { get; }
    
    public ITrailColorizer Colorizer { get; }
}
