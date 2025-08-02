using System.IO;
using Newtonsoft.Json;

namespace SabersCore.Utilities.Common;

internal static class JsonReading
{
    public static T? DeserializeStream<T>(this Stream stream)
    {
        using var streamReader = new StreamReader(stream);
        using var jsonTextReader = new JsonTextReader(streamReader);
        return new JsonSerializer().Deserialize<T>(jsonTextReader);
    }
}
