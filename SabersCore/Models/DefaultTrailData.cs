using AssetComponents.Models;
using SabersCore.Utilities.Common;
using UnityEngine;

namespace SabersCore.Models;

public class DefaultTrailData : ITrailData
{
    public DefaultTrailData(Material defaultMaterial, SaberType saberType)
    {
        Materials = [defaultMaterial];
        ColorProviders = [new DefaultTrailColorProvider(defaultMaterial, saberType)];
    }
    
    public Vector3 TrailTopOffset => Vector3.forward;
    public Vector3 TrailBottomOffset => Vector3.zero;
    public Material[] Materials { get; }

    public float LengthSeconds => TrailUtils.DefaultDuration;
    
    public ITrailColorProvider[] ColorProviders { get; }
}