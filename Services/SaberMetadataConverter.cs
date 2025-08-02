using SabersCore.Models;
using SabersCore.Utilities.Common;

namespace SabersCore.Services;

internal class SaberMetadataConverter
{
    private readonly SpriteCache spriteCache;

    public SaberMetadataConverter(SpriteCache spriteCache)
    {
        this.spriteCache = spriteCache;
    }

    public CustomSaberMetadata ConvertJson(SaberMetadataModel meta, SaberFileInfo saberFileInfo)
    {
        var saberName = RichTextString.Create(meta.SaberName);
        var authorName = RichTextString.Create(meta.AuthorName);
        var icon = spriteCache.GetSprite(meta.Hash);
        if (icon == null) icon = PluginResources.NullCoverImage;
        var descriptor = new Descriptor(saberName, authorName, icon);
        
        return new(
            saberFileInfo,
            meta.LoaderError,
            descriptor,
            meta.Trails);
    }

    public SaberMetadataModel CreateJson(CustomSaberMetadata meta) => new(
        meta.SaberFile.Hash,
        meta.SaberFile.DateAdded,
        meta.Descriptor.SaberName.FullName,
        meta.Descriptor.AuthorName.FullName,
        meta.HasTrails,
        meta.LoaderError);
}