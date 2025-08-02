using SabersCore.Services;
using Zenject;

namespace SabersCore.Installers;

internal class AppInstaller : Installer
{
    public override void InstallBindings()
    {
        // API
        Container.BindInterfacesTo<CustomSaberPrefabCache>().AsSingle();
        Container.BindInterfacesTo<CustomSabersLoader>().AsSingle();
        Container.BindInterfacesTo<MetadataLoader>().AsSingle();
        Container.BindInterfacesTo<SaberMetadataCache>().AsSingle();
        Container.BindInterfacesTo<TrailFactory>().AsSingle();
        
        // Internals
        Container.BindInterfacesAndSelfTo<GameResourcesProvider>().AsSingle();
        Container.BindInterfacesAndSelfTo<DirectoryManager>().AsSingle();
        Container.Bind<SaberFileManager>().AsSingle();
        Container.Bind<SaberMetadataConverter>().AsSingle();
        Container.Bind<SaberMetadataCacheMigrationManager>().AsSingle();
        
        Container.Bind<SpriteCache>().AsSingle();
        
        Container.Bind<WhackerLoader>().AsSingle();
        Container.Bind<SaberLoader>().AsSingle();
    }
}