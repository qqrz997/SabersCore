using System.Threading;
using System.Threading.Tasks;
using SabersCore.Models;

namespace SabersCore.Services;

public interface ISaberInstanceFactory
{
    Task<SaberInstanceSet> CreateSaberSet(string? saberHash, CancellationToken token);
    Task<SaberInstanceSet> ReplaceTrailsWithOther(SaberInstanceSet saberInstance, string? saberHash, CancellationToken token);
}