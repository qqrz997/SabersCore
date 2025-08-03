using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SabersCore.Models;

namespace SabersCore.Services;

/// <summary>
/// Class for loading different kinds of custom saber assets
/// </summary>
internal class CustomSabersLoader : ISabersLoader
{
    private readonly IPrefabCache prefabCache;
    private readonly SaberLoader saberLoader;
    private readonly WhackerLoader whackerLoader;

    public CustomSabersLoader(
        IPrefabCache prefabCache,
        SaberLoader saberLoader,
        WhackerLoader whackerLoader)
    {
        this.prefabCache = prefabCache;
        this.saberLoader = saberLoader;
        this.whackerLoader = whackerLoader;
    }

    private readonly Dictionary<SaberFileInfo, Task<ISaberData>> runningTasks = [];

    public async Task<ISaberData> GetSaberData(SaberFileInfo saberFile, bool keepSaberInstance, CancellationToken token)
    {
        if (prefabCache.TryGetPrefab(saberFile.Hash, out var cachedSaberData))
        {
            return cachedSaberData;
        }
        
        if (runningTasks.TryGetValue(saberFile, out var task))
        {
            return await task;
        }

        try
        {
            var loadSaberDataTask = LoadSaberDataAsync(saberFile);
            runningTasks.Add(saberFile, loadSaberDataTask);

            var saberData = await loadSaberDataTask;

            if (token.IsCancellationRequested)
            {
                saberData.Dispose();
                throw new OperationCanceledException();
            }

            if (keepSaberInstance)
            {
                prefabCache.AddPrefab(saberData);
            }

            return saberData;
        }
        catch (DirectoryNotFoundException)
        {
            return new NoSaberData(saberFile, SaberLoaderError.FileNotFound);
        }
        finally
        {
            runningTasks.Remove(saberFile);
        }
    }

    private async Task<ISaberData> LoadSaberDataAsync(SaberFileInfo saberFile) => saberFile.FileInfo.Extension switch
    {
        ".saber" => await saberLoader.LoadCustomSaberAsync(saberFile),
        ".whacker" => await whackerLoader.LoadWhackerAsync(saberFile),
        _ => new NoSaberData(saberFile, SaberLoaderError.InvalidFileType)
    };
}
