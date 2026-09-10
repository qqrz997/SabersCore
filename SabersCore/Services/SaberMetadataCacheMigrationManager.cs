using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using SabersCore.Models;
using SabersCore.Utilities.Common;

namespace SabersCore.Services;

internal class SaberMetadataCacheMigrationManager
{
    private readonly FileInfo cacheFile;

    public SaberMetadataCacheMigrationManager(DirectoryManager directoryManager)
    {
        cacheFile = new(Path.Combine(directoryManager.UserData.FullName, "cache"));
        MigrationTask = Task.Run(Migrate);
    }
    
    /// <summary>
    /// Task representing the migration of the cache file. Returns false if an error occurs during the task.
    /// The cache file may not exist after migration, make sure to check it exists after migration.
    /// </summary>
    public Task<bool> MigrationTask { get; }

    private bool Migrate()
    {
        try
        {
            if (!cacheFile.Exists)
            {
                return true;
            }
            
            // There are currently no migrations necessary!
            // If the metadata requirements of the cache change, it is necessary to make sure the mod knows when
            // to delete the cache file so that the loader can refresh and replace it with the new one
            
            var cacheVersion = GetCacheVersion();
            Plugin.Log.Notice($"Cache version: {cacheVersion}");

            if (cacheVersion < new Version(1, 1, 0))
            {
                Plugin.Log.Notice("Old cache version detected, deleting old cache");
                DeleteCacheMigration();
            }

            return true;
        }
        catch (Exception ex)
        {
            Plugin.Log.Critical($"A problem occurred during saber metadata cache migration\n{ex}");
            return false;
        }
    }

    private Version GetCacheVersion()
    {
        using var zipArchive = ZipFile.OpenRead(cacheFile.FullName);
        using var metadataStream = zipArchive.GetEntry("metadata.json")?.Open();
        if (metadataStream == null) return new(0, 0, 0);
    
        var versionString = metadataStream.DeserializeStream<CacheFileModel>()?.Version;
        
        return versionString is null ? new(0, 0, 0) : new Version(versionString);
    }

    private void DeleteCacheMigration()
    {
        // Proper migration isn't possible so we are forced to refresh the cache
        cacheFile.Delete();
    }
}
