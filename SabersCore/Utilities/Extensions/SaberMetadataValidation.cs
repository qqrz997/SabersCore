using System.Linq;
using SabersCore.Models;

namespace SabersCore.Utilities.Extensions;

internal static class SaberMetadataValidation
{
    public static CacheFileModel WithValidation(this CacheFileModel original) => original with
    {
        CachedMetadata = original.CachedMetadata.Where(meta => meta.IsValid()).ToArray(),
    };

    private static bool IsValid(this SaberMetadataModel meta) => !string.IsNullOrWhiteSpace(meta.Hash);
}