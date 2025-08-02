using SabersCore.Services;
using Zenject;

namespace SabersCore.Installers;

public class PlayerInstaller : Installer
{
    public override void InstallBindings()
    {
        // API
        Container.BindInterfacesTo<CustomSaberEventManagerHandler>().AsTransient();
    }
}