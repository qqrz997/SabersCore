﻿using UnityEngine;

namespace SabersCore.Utilities.Extensions;

internal static class GameObjectExtensions
{
    public static T TryGetComponentOrAdd<T>(this GameObject obj) where T : MonoBehaviour =>
        obj.GetComponent<T>() ?? obj.AddComponent<T>();

    public static void Destroy(this GameObject? obj)
    {
        if (obj != null) Object.Destroy(obj);
    }

    public static void SetLayerRecursively(this GameObject obj, int layer) => SetLayer(obj, layer);
    private static void SetLayer(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            SetLayer(gameObject.transform.GetChild(i).gameObject, layer);
        }
    }
}
