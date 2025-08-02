using System.Threading.Tasks;
using IPA;
using IPA.Loader;
using IPA.Logging;
using SabersCore.Installers;
using SabersCore.Utilities.Common;
using SiraUtil.Zenject;

namespace SabersCore;

[Plugin(RuntimeOptions.DynamicInit), NoEnableDisable]
internal class Plugin
{
    public static Logger Log { get; private set; } = null!;
    public static PluginMetadata Metadata { get; private set; } = null!;

    [Init]
    public Plugin(Logger logger, Zenjector zenjector, PluginMetadata metadata)
    {
        Log = logger;
        Metadata = metadata;
        Task.Run(() => InitAsync(logger, zenjector));
    }

    private static async Task InitAsync(Logger logger, Zenjector zenjector)
    {
        if (!await EmbeddedAssemblyLoading.TryLoadAssembly("CustomSaber.dll"))
        {
            return;
        }
        
        zenjector.UseLogger(logger);
        zenjector.Install<AppInstaller>(Location.App);
        zenjector.Install<PlayerInstaller>(Location.Player);
    }
}