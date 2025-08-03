using System.Threading;
using System.Threading.Tasks;
using SabersCore.Models;

namespace SabersCore.Services;

internal class SaberInstanceFactory : ISaberInstanceFactory
{
    private readonly ISaberMetadataCache saberMetadataCache;
    private readonly ISabersLoader sabersLoader;
    private readonly ITrailFactory trailFactory;
    private readonly GameResourcesProvider gameResourcesProvider;

    public SaberInstanceFactory(
        ISaberMetadataCache saberMetadataCache, 
        ISabersLoader sabersLoader,
        ITrailFactory trailFactory, 
        GameResourcesProvider gameResourcesProvider)
    {
        this.saberMetadataCache = saberMetadataCache;
        this.sabersLoader = sabersLoader;
        this.trailFactory = trailFactory;
        this.gameResourcesProvider = gameResourcesProvider;
    }

    public async Task<SaberInstanceSet> CreateSaberSet(string? saberHash, CancellationToken token)
    {
        if (!saberMetadataCache.TryGetMetadata(saberHash, out var meta))
        {
            return CreateNewDefaultSaberSet();
        }

        var saberData = await sabersLoader.GetSaberData(meta.SaberFile, true, token);
        return saberData.Prefab?.Instantiate() ?? CreateNewDefaultSaberSet();
    }

    public async Task<SaberInstanceSet> ReplaceTrailsWithOther(SaberInstanceSet saberInstance, string? saberHash, CancellationToken token)
    {
        if (!saberMetadataCache.TryGetMetadata(saberHash, out var meta))
        {
            return WithDefaultTrails(saberInstance);
        }
        
        var newSaberData = await sabersLoader.GetSaberData(meta.SaberFile, true, token);
        if (newSaberData.Prefab is null)
        {
            return WithDefaultTrails(saberInstance);
        }

        var leftTrails = newSaberData.Prefab.GetTrailsForType(SaberType.SaberA);
        var rightTrails = newSaberData.Prefab.GetTrailsForType(SaberType.SaberB);
        return saberInstance.WithTrails(leftTrails, rightTrails);
    }

    private SaberInstanceSet CreateNewDefaultSaberSet() =>
        new(new DefaultSaber(gameResourcesProvider.CreateNewDefaultSaber()),
            new DefaultSaber(gameResourcesProvider.CreateNewDefaultSaber()),
            [trailFactory.CreateDefaultTrailData()],
            [trailFactory.CreateDefaultTrailData()]);

    private SaberInstanceSet WithDefaultTrails(SaberInstanceSet saberInstance)
    {
        var defaultTrail = new ITrailData[] { trailFactory.CreateDefaultTrailData() };
        return saberInstance.WithTrails(defaultTrail, defaultTrail);
    }
}