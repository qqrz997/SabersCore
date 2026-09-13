using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using SabersCore.Models;
using SabersCore.Utilities.Common;
using SabersCore.Utilities.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SabersCore.Services;

internal class Saber2Loader
{
    private readonly SpriteCache spriteCache;

    public Saber2Loader(SpriteCache spriteCache)
    {
        this.spriteCache = spriteCache;
    }

    /// <summary>
    /// Loads a custom saber from a .whacker file
    /// </summary>
    public async Task<ISaberData> LoadSaber2Async(SaberFileInfo saberFile)
    {
        AssetBundle? bundle = null;
        GameObject? saberPrefab = null;

        try
        {
            if (!saberFile.FileInfo.Exists)
            {
                return new NoSaberData(saberFile, SaberLoaderError.FileNotFound);
            }

            Plugin.Log.Debug($"Attempting to load saber2 file - {saberFile.FileInfo.Name}");

            await using var fileStream = saberFile.FileInfo.OpenRead();
            using var archive = new ZipArchive(fileStream, ZipArchiveMode.Read);

            var jsonEntry = archive.GetEntry("metadata.json");

            if (jsonEntry is null)
            {
                return new NoSaberData(saberFile, SaberLoaderError.FileNotFound);
            }

            await using var jsonStream = jsonEntry.Open();
            var saber2 = jsonStream.DeserializeStream<Saber2Model>();
            if (saber2 is null || !saber2.Assets.TryGetValue(AssetPlatform.PC, out var assetMetadata))
            {
                return new NoSaberData(saberFile, SaberLoaderError.FileNotFound);
            }

            var bundleEntry = archive.GetEntry(assetMetadata.FilePath);
            if (bundleEntry is null)
            {
                return new NoSaberData(saberFile, SaberLoaderError.FileNotFound);
            }

            await using var bundleStream = bundleEntry.Open();
            bundle = await BundleLoading.LoadBundle(bundleStream);
            if (bundle == null)
            {
                return new NoSaberData(saberFile, SaberLoaderError.NullBundle);
            }

            saberPrefab = await BundleLoading.LoadAsset<GameObject>(bundle, "_CustomSaber");
            if (saberPrefab == null)
            {
                bundle.Unload(true);
                return new NoSaberData(saberFile, SaberLoaderError.NullAsset);
            }

            saberPrefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            saberPrefab.name += $" {saber2.ModelName}";

            var icon = await GetDownscaledIcon(archive, saber2);
            spriteCache.AddSprite(saberFile.Hash, icon);

            var saberName = RichTextString.Create(saber2.ModelName);
            var authorName = RichTextString.Create(saber2.AuthorName);
            var saberIcon = PluginResources.NullCoverImage;
            var descriptor = new Descriptor(saberName, authorName, saberIcon);
            var hasTrails = CustomTrailUtils.GetTrailsFromWhacker(saberPrefab).Any();
            var metadata = new CustomSaberMetadata(saberFile, SaberLoaderError.None, descriptor, hasTrails);
            var saber2Prefab = new CustomSaberPrefab(saberPrefab);
            return new CustomSaberData(metadata, saber2Prefab);
        }
        catch (Exception ex)
        {
            if (bundle != null) bundle.Unload(true);
            Plugin.Log.Error($"Encountered a problem while trying to load file - {saberFile.FileInfo.Name}\n{ex}");
            return new NoSaberData(saberFile, SaberLoaderError.Unknown);
        }
        finally
        {
            if (saberPrefab != null) saberPrefab.hideFlags &= ~HideFlags.DontUnloadUnusedAsset;
            if (bundle != null) bundle.Unload(false);
        }
    }

    private static async Task<Sprite?> GetDownscaledIcon(ZipArchive archive, Saber2Model saber2)
    {
        if (string.IsNullOrEmpty(saber2.IconPath)) return null;
        
        var iconEntry = archive.GetEntry(saber2.IconPath);
        if (iconEntry is null) return null;

        using var memoryStream = new MemoryStream();
        await using var thumbStream = iconEntry.Open();
        
        await thumbStream.CopyToAsync(memoryStream);
        
        var icon = new Texture2D(2, 2).ToSprite(memoryStream.ToArray());
        if (icon == null)
        {
            return null;
        }
        if (icon.texture == null)
        {
            Object.Destroy(icon);
            return null;
        }
        var downscaledIcon = icon.texture.Downscale(128, 128).ToSprite(rename: saber2.ModelName);
        Object.Destroy(icon);
        return downscaledIcon;
    }
}