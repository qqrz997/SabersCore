namespace SabersCore.Models;

public record CustomSaberMetadata(
    SaberFileInfo SaberFile,
    SaberLoaderError LoaderError,
    Descriptor Descriptor,
    bool HasTrails);