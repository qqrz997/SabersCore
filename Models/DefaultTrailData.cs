using SabersCore.Utilities.Common;
using UnityEngine;

namespace SabersCore.Models;

public class DefaultTrailData : ITrailData
{
    public DefaultTrailData(Material defaultMaterial)
    {
        Material = defaultMaterial;
    }
    
    public Material? Material { get; }
    public float LengthSeconds => TrailUtils.DefaultDuration;
    public bool UseCustomColor => false;
    public Color CustomColor => Color.white;
    public Color ColorMultiplier => Color.white; 
    public Vector3 TrailTopOffset => Vector3.forward;
    public Vector3 TrailBottomOffset => Vector3.zero;
}