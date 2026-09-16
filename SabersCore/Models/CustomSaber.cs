using System;
using System.Linq;
using SaberComponents.Components;
using SaberComponents.Models;
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

    public bool InUse { get; set; }
    public GameObject GameObject { get; }
    public EventManager EventManager { get; }

    public CustomSaber(GameObject gameObject)
    {
        GameObject = gameObject;
        GameObject.SetLayerRecursively(12);
        EventManager = gameObject.TryGetComponentOrAdd<EventManager>();
        // colorableMaterials = CustomTrailUtils.GetColorableSaberMaterials(gameObject).ToArray();
        allColorers = gameObject.GetComponentsInChildren<MaterialColorer>() ?? [];
        saberColors = allColorers.Where(colorer => colorer.UsesSaberColors()).ToArray();
        boostColors = allColorers.Where(colorer => colorer.UsesBoostColors()).ToArray();
    }

    public void SetColor(ColorScheme colorScheme)
    {
        foreach (var colorer in allColorers)
        {
            var color = colorScheme.GetColorByType(colorer.colorSchemeType);
            colorer.materialPropertyBlock ??= new();
            colorer.materialPropertyBlock.SetColor(colorer.propertyName, color * colorer.multiplierColor);
            colorer.meshRenderer.SetPropertyBlock(colorer.materialPropertyBlock);
        }
    }

    public void UpdateBoostColors(ColorScheme colorScheme, bool isBoostOn)
    {
        foreach (var colorer in boostColors)
        {
            var color = colorScheme.GetBoostColorByType(colorer.colorSchemeType, isBoostOn);
            colorer.materialPropertyBlock ??= new();
            colorer.materialPropertyBlock.SetColor(colorer.propertyName, color * colorer.multiplierColor);
            colorer.meshRenderer.SetPropertyBlock(colorer.materialPropertyBlock);
        }
    }

    public void SetColor(Color color, SaberType saberType)
    {
        foreach (var colorer in saberColors)
        {
            colorer.materialPropertyBlock ??= new();
            if ((saberType == SaberType.SaberA && colorer.colorSchemeType == ColorSchemeType.LeftSaber)
                || (saberType == SaberType.SaberB && colorer.colorSchemeType == ColorSchemeType.RightSaber))
            {
                colorer.materialPropertyBlock.SetColor(colorer.propertyName, color * colorer.multiplierColor);
                colorer.meshRenderer.SetPropertyBlock(colorer.materialPropertyBlock);
            }
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
