using System.Text.RegularExpressions;

namespace SabersCore.Utilities.Common;

internal class RegularExpressions
{
    public static Regex RichTextRegex { get; } = new("<[^>]*>");
}
