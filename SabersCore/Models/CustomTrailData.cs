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
        ITrailColorProvider[] colorProviders)
    {
        TrailTopOffset = trailTopOffset;
        TrailBottomOffset = trailBottomOffset;
        Materials = materials;
        LengthSeconds = lengthSeconds;
        ColorProviders = colorProviders;
    }


    public Vector3 TrailTopOffset { get; }
    public Vector3 TrailBottomOffset { get; }
    public Material[] Materials { get; }
    
    public float LengthSeconds { get; }
    
    public ITrailColorProvider[] ColorProviders { get; }
}
