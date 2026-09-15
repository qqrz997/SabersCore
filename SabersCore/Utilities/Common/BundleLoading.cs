using System.IO;
using System.Threading.Tasks;
using AssetBundleLoadingTools.Utilities;
using UnityEngine;

namespace SabersCore.Utilities.Common;

/// <summary>
/// Uses AssetBundleExtensions to greatly simplify async <seealso cref="AssetBundle"/> loading
/// </summary>
internal static class BundleLoading
{
    public static async Task<AssetBundle?> LoadBundle(string path) =>
        await AssetBundleExtensions.LoadFromFileAsync(path);

    public static async Task<AssetBundle?> LoadBundle(Stream stream)
    {
        if (!stream.CanRead || !stream.CanSeek)
            return await CopyStreamAndLoadBundle(stream);

        await Task.CompletedTask;
        return AssetBundle.LoadFromStream(stream);
        //return await AssetBundleExtensions.LoadFromStreamAsync(stream);
    }

    private static async Task<AssetBundle?> CopyStreamAndLoadBundle(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        await Task.CompletedTask;
        return AssetBundle.LoadFromStream(memoryStream);
        // return await AssetBundleExtensions.LoadFromStreamAsync(memoryStream);
    }

    public static async Task<T?> LoadAsset<T>(AssetBundle bundle, string assetPath) where T : Object
    {
        await Task.CompletedTask;
        return bundle.LoadAsset<T>(assetPath);
        // return await AssetBundleExtensions.LoadAssetAsync<T>(bundle, assetPath);
    }
}
