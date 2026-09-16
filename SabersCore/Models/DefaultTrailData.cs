using SaberComponents.Models;
using SabersCore.Utilities.Common;
using UnityEngine;

namespace SabersCore.Models;

public class DefaultTrailData : ITrailData
{
    public DefaultTrailData(Material defaultMaterial, SaberType saberType)
    {
        Material = defaultMaterial;
        ColorSchemeType = saberType == SaberType.SaberA ? ColorSchemeType.LeftSaber : ColorSchemeType.RightSaber;
    }
    
    public Material? Material { get; }
    public Vector3 TrailTopOffset => Vector3.forward;
    public Vector3 TrailBottomOffset => Vector3.zero;
    
    public float LengthSeconds => TrailUtils.DefaultDuration;

    public ColorSchemeType ColorSchemeType { get; }
    public bool UseColorBoostEvents => false;
    
    public bool UseTrailColor => false;
    public Color CustomColor => Color.white;
    public Color ColorMultiplier => Color.white; 
}