using SaberComponents.Components;
using SabersCore.Utilities.Extensions;
using UnityEngine;

namespace SabersCore.Models;

/// <summary>
/// A wrapper around a custom saber object instance
/// </summary>
internal class CustomSaber : ISaber
{
    // private readonly Material[] colorableMaterials;
    private readonly MaterialColorer[] colorers;

    public bool InUse { get; set; }
    public GameObject GameObject { get; }
    public EventManager EventManager { get; }

    public CustomSaber(GameObject gameObject)
    {
        GameObject = gameObject;
        GameObject.SetLayerRecursively(12);
        EventManager = gameObject.TryGetComponentOrAdd<EventManager>();
        // colorableMaterials = CustomTrailUtils.GetColorableSaberMaterials(gameObject).ToArray();
        colorers = gameObject.GetComponentsInChildren<MaterialColorer>() ?? [];
    }

    public void SetColor(ColorScheme colorScheme)
    {
        foreach (var colorer in colorers)
        {
            colorer.materialPropertyBlock ??= new();
            colorer.materialPropertyBlock.SetColor(colorer.propertyName, colorScheme.GetColorByType(colorer.colorSchemeType) * colorer.multiplierColor);
            colorer.meshRenderer.SetPropertyBlock(colorer.materialPropertyBlock);
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
