using System;
using System.Threading;
using System.Threading.Tasks;
using SabersCore.Models;

namespace SabersCore.Services;

public interface ISaberFileManager
{
    SaberFileInfo[] GetLoadedSaberFiles();
    Task<SaberFileInfo[]> ReloadAllSaberFiles(CancellationToken token, IProgress<int> progress);
}