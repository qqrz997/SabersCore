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

internal class WhackerLoader
{
    private readonly SpriteCache spriteCache;

    public WhackerLoader(SpriteCache spriteCache)
    {
        this.spriteCache = spriteCache;
    }

    /// <summary>
    /// Loads a custom saber from a .whacker file
    /// </summary>
    public async Task<ISaberData> LoadWhackerAsync(SaberFileInfo saberFile)
    {
        AssetBundle? bundle = null;
        GameObject? saberPrefab = null;

        try
        {
            if (!saberFile.FileInfo.Exists)
            {
                return new NoSaberData(saberFile, SaberLoaderError.FileNotFound);
            }

            Plugin.Log.Debug($"Attempting to load whacker file - {saberFile.FileInfo.Name}");

            await using var fileStream = saberFile.FileInfo.OpenRead();
            using var archive = new ZipArchive(fileStream, ZipArchiveMode.Read);

            var jsonEntry = archive.Entries.FirstOrDefault(x => x.FullName.EndsWith(".json"));

            if (jsonEntry is null)
            {
                return new NoSaberData(saberFile, SaberLoaderError.FileNotFound);
            }

            await using var jsonStream = jsonEntry.Open();
            var whacker = jsonStream.DeserializeStream<WhackerModel>();
            
            if (whacker is null)
            {
                return new NoSaberData(saberFile, SaberLoaderError.InvalidFileType);
            }

            if (whacker.Config.IsLegacy)
            {
                return new NoSaberData(saberFile, SaberLoaderError.LegacyWhacker);
            }

            var bundleEntry = archive.GetEntry(whacker.FileName);

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

            saberPrefab = await BundleLoading.LoadAsset<GameObject>(bundle, "_Whacker");

            if (saberPrefab == null)
            {
                bundle.Unload(true);
                return new NoSaberData(saberFile, SaberLoaderError.NullAsset);
            }

            saberPrefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            saberPrefab.name += $" {whacker.Descriptor.Name}";

            var icon = await GetDownscaledIcon(archive, whacker);
            spriteCache.AddSprite(saberFile.Hash, icon);

#if SHADER_DEBUG
            await ShaderInfoDump.Instance.RegisterModelShaders(saberPrefab, whacker.descriptor.objectName ?? "Unknown Whacker");
#else
            await ShaderRepairUtils.RepairSaberShadersAsync(saberPrefab);
#endif

            var saberName = RichTextString.Create(whacker.Descriptor.Name);
            var authorName = RichTextString.Create(whacker.Descriptor.Author);
            var saberIcon = icon != null ? icon : PluginResources.NullCoverImage;
            var descriptor = new Descriptor(saberName, authorName, saberIcon);
            var hasTrails = CustomTrailUtils.GetTrailsFromWhacker(saberPrefab).Any();
            var metadata = new CustomSaberMetadata(saberFile, SaberLoaderError.None, descriptor, hasTrails);
            var whackerPrefab = new WhackerPrefab(saberPrefab);
            return new CustomSaberData(metadata, whackerPrefab);
        }
        catch (Exception ex)
        {
            if (bundle != null) bundle.Unload(true);
            Plugin.Log.Error($"Encountered a problem while trying to load file - {saberFile.FileInfo.Name}\n{ex}");
            throw;
        }
        finally
        {
            if (saberPrefab != null) saberPrefab.hideFlags &= ~HideFlags.DontUnloadUnusedAsset;
            if (bundle != null) bundle.Unload(false);
        }
    }

    private static async Task<Sprite?> GetDownscaledIcon(ZipArchive archive, WhackerModel whacker)
    {
        if (whacker.Descriptor.IconFileName is null) return null;

        var iconEntry = archive.GetEntry(whacker.Descriptor.IconFileName);

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
        var downscaledIcon = icon.texture.Downscale(128, 128).ToSprite(rename: whacker.Descriptor.Name);
        Object.Destroy(icon);
        return downscaledIcon;
    }
}
