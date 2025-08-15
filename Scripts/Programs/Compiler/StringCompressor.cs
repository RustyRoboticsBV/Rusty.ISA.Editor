using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Rusty.ISA.Editor;

public class StringCompressor
{
    public static string CompressString(string text)
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

    public static string DecompressString(string compressedText)
    {
        if (string.IsNullOrEmpty(compressedText))
            return "";

        byte[] bytes = Convert.FromBase64String(compressedText);

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
}