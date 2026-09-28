using AssetComponents.Components.Sabers;
using UnityEngine;

namespace SabersCore.Models;

public record TrailColorInfo(Material Material, int MaterialIndex, string PropertyName, Color Color, bool ApplyToVertexColor)
{
    public TrailColorInfo(TrailColorer tc, Color c) 
        : this(tc.Material, tc.MaterialIndex, tc.PropertyName, c, tc.ApplyToVertexColor) { }
    
    public TrailColorInfo(TrailColorer tc)
        : this(tc.Material, tc.MaterialIndex, tc.PropertyName, Color.white, tc.ApplyToVertexColor) { }
}