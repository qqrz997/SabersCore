using AssetComponents.Components;
using UnityEngine;

namespace SabersCore.Models;

/// <summary>
/// The necessary information to create a custom trail
/// </summary>
public interface ITrailData
{
    public Vector3 TrailTopOffset { get; }
    public Vector3 TrailBottomOffset { get; }
    public Material[] Materials { get; }
    
    public float LengthSeconds { get; }

    public ITrailColorizer Colorizer { get; }
}