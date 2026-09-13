using Newtonsoft.Json;

namespace SabersCore.Models;

internal class WhackerDescriptor
{
    public string? Name { get; }
    public string? Author { get; }
    public string? IconFileName { get; }

    [JsonConstructor]
    public WhackerDescriptor(string? objectName, string? author, string? coverImage)
    {
        Name = objectName;
        Author = author;
        IconFileName = coverImage;
    }
}
