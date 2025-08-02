using System;
using System.Threading.Tasks;

namespace SabersCore.Services;

public interface ISaberMetadataLoader
{
    MetadataLoaderProgress CurrentProgress { get; }
    event Action<MetadataLoaderProgress>? LoadingProgressChanged;
    
    Task ReloadAsync();
}