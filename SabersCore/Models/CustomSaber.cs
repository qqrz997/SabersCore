using System.Linq;
using AssetComponents.Components;
using AssetComponents.Components.Sabers;
using AssetComponents.Models;
using SabersCore.Utilities.Extensions;
using UnityEngine;

namespace SabersCore.Models;

/// <summary>
/// A wrapper around a custom saber object instance
/// </summary>
internal class CustomSaber : ISaber
{
    private readonly MaterialColorer[] allColorers;
    private readonly MaterialColorer[] saberColors;
    private readonly MaterialColorer[] boostColors;

    public GameObject GameObject { get; }
    public EventManager EventManager { get; }

    public CustomSaber(GameObject gameObject)
    {
        GameObject = gameObject;
        GameObject.SetLayerRecursively(12);
        EventManager = gameObject.TryGetComponentOrAdd<EventManager>();
        allColorers = gameObject.GetComponentsInChildren<MaterialColorer>() ?? [];
        saberColors = allColorers.Where(colorer => colorer.UsesSaberColors()).ToArray();
        boostColors = allColorers.Where(colorer => colorer.UsesBoostColors()).ToArray();
    }

    public void SetColorScheme(ColorScheme colorScheme)
    {
        foreach (var colorer in allColorers)
        {
            var color = colorScheme.GetColorByType(colorer.ColorSchemeType);
            colorer.MaterialPropertyBlock.SetColor(colorer.PropertyName, color * colorer.MultiplierColor);
            colorer.UpdateRendererProperties();
        }
    }

    public void SetBoostColors(ColorScheme colorScheme, bool isBoostOn)
    {
        foreach (var colorer in boostColors)
        {
            var color = colorScheme.GetBoostColorByType(colorer.ColorSchemeType, isBoostOn);
            colorer.MaterialPropertyBlock.SetColor(colorer.PropertyName, color * colorer.MultiplierColor);
            colorer.UpdateRendererProperties();
        }
    }

    public void SetSpecificColor(Color color)
    {
        foreach (var colorer in saberColors)
        {
            colorer.MaterialPropertyBlock.SetColor(colorer.PropertyName, color * colorer.MultiplierColor);
            colorer.UpdateRendererProperties();
        }
    }

    public void SetParent(Transform parent)
    {
        GameObject.transform.SetParent(parent, false);
        GameObject.transform.position = parent.position;
        GameObject.transform.rotation = parent.rotation;
    }

    public void SetLength(float length) =>
        GameObject.transform.localScale = GameObject.transform.localScale with { z = length };

    public void SetWidth(float width) =>
        GameObject.transform.localScale = GameObject.transform.localScale with { x = width, y = width };

    public void Destroy()
    {
        if (GameObject != null)
        {
            GameObject.Destroy();
        }
    }
}
