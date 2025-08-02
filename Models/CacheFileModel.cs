namespace SabersCore.Models;

internal record CacheFileModel(
    string Version,
    SaberMetadataModel[] CachedMetadata)
{
    public static CacheFileModel Empty => new(Plugin.Metadata.HVersion.ToString(), []);
}
