using SaberComponents.Models;
using UnityEngine;

namespace SabersCore.Models;

/// <summary>
/// The necessary information to create a custom trail
/// </summary>
public interface ITrailData
{
    public Vector3 TrailTopOffset { get; }
    public Vector3 TrailBottomOffset { get; }
    public Material? Material { get; }
    
    public float LengthSeconds { get; }
    
    public ColorSchemeType ColorSchemeType { get; }
    public bool UseColorBoostEvents { get; }
    
    public bool UseTrailColor { get; }
    public Color CustomColor { get; }
    public Color ColorMultiplier { get; }
}