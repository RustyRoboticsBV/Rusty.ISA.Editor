using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Rusty.ISA.Editor;

public class StringCompressor
{
    public static string Compress(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        byte[] bytes = Encoding.UTF8.GetBytes(text);

        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                msi.CopyTo(gs);
            }
            return Convert.ToBase64String(mso.ToArray());
        }
    }

    /// <summary>
    /// Decompress a string.
    /// </summary>
    public static string Decompress(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        byte[] bytes = Convert.FromBase64String(text);

        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            {
                gs.CopyTo(mso);
            }
            return Encoding.UTF8.GetString(mso.ToArray());
        }
    }

    /// <summary>
    /// Check if a string represents a compressed or not.
    /// </summary>
    public static bool IsCompressed(string text)
    {
        return text.StartsWith("H4s");
    }
}