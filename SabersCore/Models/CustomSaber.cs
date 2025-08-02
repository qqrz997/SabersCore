using System.Linq;
using CustomSaber;
using SabersCore.Utilities.Common;
using SabersCore.Utilities.Extensions;
using UnityEngine;

namespace SabersCore.Models;

/// <summary>
/// A wrapper around a custom saber object instance
/// </summary>
internal class CustomSaber : ISaber
{
    private readonly Material[] colorableMaterials;

    public bool InUse { get; set; }
    public GameObject GameObject { get; }
    public EventManager EventManager { get; }

    public CustomSaber(GameObject gameObject)
    {
        GameObject = gameObject;
        GameObject.SetLayerRecursively(12);
        EventManager = gameObject.TryGetComponentOrAdd<EventManager>();
        colorableMaterials = CustomTrailUtils.GetColorableSaberMaterials(gameObject).ToArray();
    }

    public void SetColor(Color color)
    {
        foreach (var colorableMaterial in colorableMaterials)
        {
            colorableMaterial.SetColor(MaterialProperties.Color, color);
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
