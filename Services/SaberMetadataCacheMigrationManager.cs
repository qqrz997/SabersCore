using System;
using System.Threading.Tasks;

namespace SabersCore.Services;

internal class SaberMetadataCacheMigrationManager
{
    // private readonly DirectoryManager directoryManager;
    // private readonly FileInfo cacheFile;

    public SaberMetadataCacheMigrationManager()
    {
        // this.directoryManager = directoryManager;
        // cacheFile = new(Path.Combine(directoryManager.UserData.FullName, "SaberMetadataCache"));

        MigrationTask = Task.Run(Migrate);
    }
    
    public Task<bool> MigrationTask { get; }

    private bool Migrate()
    {
        try
        {
            // There are currently no migrations necessary!
            // If the metadata requirements of the cache change, it is necessary to make sure the mod knows when
            // to delete the cache file so that the loader can refresh and replace it with the new one
            
            // var cacheVersion = GetCacheVersion();
            // Plugin.Log.Notice("Old cache version detected, deleting old cache");
            
            return true;
        }
        catch (Exception ex)
        {
            Plugin.Log.Critical($"A problem occurred during saber metadata cache migration\n{ex}");
            return false;
        }
    }

    // private Version GetCacheVersion()
    // {
    //     if (!cacheFile.Exists) return new(0, 0, 0);
    //     
    //     using var zipArchive = ZipFile.OpenRead(cacheFile.FullName);
    //     using var metadataStream = zipArchive.GetEntry("metadata.json")?.Open();
    //     if (metadataStream == null) return new(0, 0, 0);
    //
    //     var versionString = metadataStream.DeserializeStream<CacheFileModel>()?.Version;
    //     
    //     return versionString is null ? new(0, 0, 0) : new Version(versionString);
    // }
}
