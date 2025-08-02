using System.IO;
using IPA.Utilities;
using Zenject;

namespace SabersCore.Services;

internal class DirectoryManager : IInitializable
{
    private readonly string customSabersPath = Path.Combine(UnityGame.InstallPath, "CustomSabers");
    private readonly string userDataPath = Path.Combine(UnityGame.UserDataPath, Plugin.Metadata.Id);

    public DirectoryManager()
    {
        CustomSabers = new(customSabersPath);
        UserData = new(userDataPath);
    }
    
    public DirectoryInfo CustomSabers { get; }
    public DirectoryInfo UserData { get; }

    public void Initialize()
    {
        CustomSabers.Create();
        UserData.Create();
    }
}
