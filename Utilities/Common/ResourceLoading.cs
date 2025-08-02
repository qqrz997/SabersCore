using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace SabersCore.Utilities.Common;

internal class ResourceLoading
{
    private static Assembly? assembly;

    public static byte[] GetResource(string resourcePath)
    {
        assembly ??= Assembly.GetExecutingAssembly();
        return GetResource(assembly, resourcePath);
    }

    public static async Task<byte[]> GetResourceAsync(string resourcePath)
    {
        assembly ??= Assembly.GetExecutingAssembly();
        return await GetResourceAsync(assembly, resourcePath);
    }

    private static byte[] GetResource(Assembly assembly, string resourcePath)
    {
        using var stream = assembly.GetManifestResourceStream(resourcePath);
        return stream switch
        {
            null => [],
            MemoryStream memoryStream => memoryStream.ToArray(),
            _ => CopyStreamToMemory(stream)
        };
        static byte[] CopyStreamToMemory(Stream stream)
        {
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }

    private static async Task<byte[]> GetResourceAsync(Assembly assembly, string resourcePath)
    {
        await using var stream = assembly.GetManifestResourceStream(resourcePath);
        return stream switch
        {
            null => [],
            MemoryStream memoryStream => memoryStream.ToArray(),
            _ => await CopyStreamToMemoryAsync(stream)
        };
        static async Task<byte[]> CopyStreamToMemoryAsync(Stream stream)
        {
            await using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
