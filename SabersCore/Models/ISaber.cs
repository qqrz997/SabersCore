using AssetComponents.Components.Sabers;
using UnityEngine;

namespace SabersCore.Models;

public interface ISaber
{
    public GameObject GameObject { get; }
    public EventManager? EventManager { get; }

    /// <summary>
    /// Color all materials based on a full color scheme
    /// </summary>
    public void SetColorScheme(ColorScheme colorScheme);
    
    /// <summary>
    /// Update colors of the saber that want to switch between environment colors and boost colors
    /// </summary>
    public void SetBoostColors(ColorScheme colorScheme, bool isBoostOn);
    
    /// <summary>
    /// Color materials that want to use saber colors to a specific color
    /// </summary>
    public void SetSpecificColor(Color color);
    
    public void SetParent(Transform parent);
    public void SetLength(float length);
    public void SetWidth(float width);
    public void Destroy();
}
