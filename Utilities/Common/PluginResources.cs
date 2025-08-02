using System;
using System.IO;
using SabersCore.Utilities.Extensions;
using UnityEngine;

namespace SabersCore.Utilities.Common;

internal class PluginResources
{
    private const string ResourcesPath = $"{nameof(SabersCore)}.Resources.";
    
    public static Sprite NullCoverImage { get; } = LoadSpriteResource("null-image.png");
    public static Sprite DefaultCoverImage { get; } = LoadSpriteResource("defaultsabers-image.jpg");

    private static Sprite LoadSpriteResource(string resourceName)
    {
        var imageData = ResourceLoading.GetResource(ResourcesPath + resourceName);
        var sprite = new Texture2D(2, 2).ToSprite(imageData, rename: Path.GetFileNameWithoutExtension(resourceName))
               ?? throw new InvalidOperationException("Failed to create a sprite from an internal image");
        return sprite;
    }
}
