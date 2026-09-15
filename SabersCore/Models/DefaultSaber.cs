using SaberComponents.Components;
using SabersCore.Components;
using UnityEngine;

namespace SabersCore.Models;

public class DefaultSaber : ISaber
{
    private readonly DefaultSaberColorer defaultSaberColorer;

    public bool InUse { get; set; }
    public GameObject GameObject { get; }
    public EventManager? EventManager => null;
    
    public DefaultSaber(GameObject defaultSaberObject, SaberType saberType)
    {
        GameObject = defaultSaberObject;
        defaultSaberColorer = new(defaultSaberObject, saberType);
    }

    public void SetColor(ColorScheme colorScheme) => defaultSaberColorer.SetColor(colorScheme);
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
            Object.Destroy(GameObject);
        }
    }
}
