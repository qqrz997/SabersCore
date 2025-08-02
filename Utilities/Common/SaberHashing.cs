using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SabersCore.Utilities.Common;

internal class SaberHashing
{
    public static string GetSaberHash(FileInfo file)
    {
        using var fileStream = file.OpenRead();
        return MD5Checksum(fileStream, "x2");
    }

    private static string MD5Checksum(Stream stream, string format) =>
        stream.Length == 0 || !stream.CanRead || !stream.CanSeek ? string.Empty
        : GetMD5String(stream, format);

    private static string GetMD5String(Stream stream, string format)
    {
        using var md5 = MD5.Create();
        md5.ComputeHash(stream);
        return HashToString(md5.Hash, format) switch
        {
            { Length: > 0 } hashString => hashString,
            _ => string.Empty
        };
    }

    private static string HashToString(byte[] hashBytes, string format)
    {
        var sb = new StringBuilder();
        foreach (var s in hashBytes.Select(b => b.ToString(format)))
        {
            sb.Append(s);
        }
        return sb.ToString();
    }
}
