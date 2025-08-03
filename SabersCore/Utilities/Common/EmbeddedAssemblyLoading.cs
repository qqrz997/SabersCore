using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SabersCore.Utilities.Common;

internal class EmbeddedAssemblyLoading
{
    private const string ResourcesPath = "SabersCore.Resources.";
    
    public static async Task<bool> TryLoadAssembly(string assemblyName)
    {
        try
        {
            var resource = await ResourceLoading.GetResourceAsync($"{ResourcesPath}{assemblyName}");
            if (resource is []) throw new("Resource not found.");
            Assembly.Load(resource);
            return true;
        }
        catch (Exception ex)
        {
            Plugin.Log.Error($"Problem encountered when trying to load assembly '{assemblyName}'\n{ex}");
            return false;
        }
    }
}