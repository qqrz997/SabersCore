using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SabersCore.Models;
using SabersCore.Utilities.Common;

namespace SabersCore.Services;

internal class SaberFileManager : ISaberFileManager
{
    private readonly DirectoryManager directoryManager;

    public SaberFileManager(DirectoryManager directoryManager)
    {
        this.directoryManager = directoryManager;
    }

    private SaberFileInfo[] loadedFiles = [];

    public SaberFileInfo[] GetLoadedSaberFiles() =>
        loadedFiles;
    
    public async Task<SaberFileInfo[]> ReloadAllSaberFiles(CancellationToken token, IProgress<int> progress) => 
        await Task.Run(() => GetDistinctSaberFiles(token, progress), token);
    
    private SaberFileInfo[] GetDistinctSaberFiles(CancellationToken token, IProgress<int> progress)
    {
        progress.Report(0);
        
        var fileInfos = directoryManager.CustomSabers.EnumerateSaberFiles(SearchOption.AllDirectories).ToList();
        int i = 0;
        int lastPercent = 0;
        var saberFileBag = new ConcurrentBag<SaberFileInfo>();
        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount / 2 - 1,
            CancellationToken = token
        };
        
        Parallel.ForEach(fileInfos, parallelOptions, file =>
        {
            if (TryCreateSaberFile(file, out var saberFileInfo)) saberFileBag.Add(saberFileInfo);
            
            int newPercent = (i + 1) * 100 / fileInfos.Count;
            if (newPercent != lastPercent)
            {
                progress.Report(newPercent);
                lastPercent = newPercent;
            }
            i++;
        });

        loadedFiles = saberFileBag.Distinct(new SaberFileInfoHashComparer()).ToArray();
        return loadedFiles;
    }

    private static bool TryCreateSaberFile(FileInfo file, [NotNullWhen(true)] out SaberFileInfo? saberFileInfo)
    {
        saberFileInfo = null;
        try
        {
            var saberHash = SaberHashing.GetSaberHash(file);
            if (string.IsNullOrEmpty(saberHash)) return false; 
            saberFileInfo = new(file, saberHash, DateTime.UtcNow);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private class SaberFileInfoHashComparer : IEqualityComparer<SaberFileInfo>
    {
        public bool Equals(SaberFileInfo? a, SaberFileInfo? b) => a != null && b != null && a.Hash == b.Hash;
        public int GetHashCode(SaberFileInfo obj) => obj.Hash.GetHashCode();
    }
}