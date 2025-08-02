namespace SabersCore.Services;

public record MetadataLoaderProgress(
    string Stage,
    bool Completed = false,
    int? StagePercent = null);