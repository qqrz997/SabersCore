using System.Threading;
using System.Threading.Tasks;
using SabersCore.Models;

namespace SabersCore.Services;

public interface ISabersLoader
{
    Task<ISaberData> GetSaberData(SaberFileInfo saberFile, bool keepSaberInstance, CancellationToken token);
}