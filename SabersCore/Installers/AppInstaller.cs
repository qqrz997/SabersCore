using SabersCore.Services;
using Zenject;

namespace SabersCore.Installers;

internal class AppInstaller : Installer
{
    public override void InstallBindings()
    {
        // API
        Container.BindInterfacesTo<CustomSaberPrefabCache>().AsSingle();
        Container.BindInterfacesTo<SaberInstanceFactory>().AsSingle();
        Container.BindInterfacesTo<CustomSabersLoader>().AsSingle();
        Container.BindInterfacesTo<SaberMetadataCache>().AsSingle();
        Container.BindInterfacesTo<SaberFileManager>().AsSingle();
        Container.BindInterfacesTo<MetadataLoader>().AsSingle();
        Container.BindInterfacesTo<TrailFactory>().AsSingle();
        
        // Internals
        Container.BindInterfacesAndSelfTo<GameResourcesProvider>().AsSingle();
        Container.BindInterfacesAndSelfTo<DirectoryManager>().AsSingle();
        Container.Bind<SaberMetadataCacheMigrationManager>().AsSingle();
        Container.Bind<SaberMetadataConverter>().AsSingle();
        Container.Bind<WhackerLoader>().AsSingle();
        Container.Bind<SpriteCache>().AsSingle();
        Container.Bind<SaberLoader>().AsSingle();
    }
}