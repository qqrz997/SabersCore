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

internal class SaberFileManager
{
    private readonly DirectoryManager directoryManager;

    public SaberFileManager(DirectoryManager directoryManager)
    {
        this.directoryManager = directoryManager;
    }

    /// <summary>
    /// Search the sabers directory for all saber files, getting their file info and computing their checksum. This will
    /// ignore duplicate saber files with the same hash.
    /// </summary>
    /// <returns>An array containing each saber file info</returns>
    public async Task<SaberFileInfo[]> GetSaberFilesAsync(CancellationToken token, IProgress<int> progress) => 
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

        return saberFileBag.Distinct(new SaberFileInfoHashComparer()).ToArray();
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