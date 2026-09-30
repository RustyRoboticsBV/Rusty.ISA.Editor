using System;
using System.Security.Cryptography;
using System.Text;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for computing the hashcode of a codec.
/// </summary>
internal static class Hasher
{
    /* Public methods. */
    /// <summary>
    /// Compute the hashcode of a codec and its children.
    /// </summary>
    public static string Hash(Codec codec, HashAlgorithm hash)
    {
        Hash(hash, codec);
        byte[] hashBytes = hash.TransformFinalBlock([], 0, 0);
        string hashHex = Convert.ToHexString(hash.Hash);
        return hashHex;
    }

    /* Private methods. */
    /// <summary>
    /// Compute the hashcode of this codec and its children.
    /// </summary>
    private static void Hash(HashAlgorithm hash, Codec codec)
    {
        // Hash start tag.
        Hash(hash, "<");
        Hash(hash, codec.Tag);

        foreach (var attribute in codec.Attributes)
        {
            if (attribute.Key == Codec.Checksum)
                continue;

            Hash(hash, " ");
            Hash(hash, attribute.Key);
            Hash(hash, "=\"");
            Hash(hash, attribute.Value);
            Hash(hash, "\"");
        }

        Hash(hash, ">");

        // Hash contents.
        if (codec.Children.Count == 0)
            Hash(hash, codec.InnerText);
        else
        {
            foreach (Codec child in codec.Children)
            {
                Hash(child, hash);
            }
        }

        // Hash end tag.
        Hash(hash, "</");
        Hash(hash, codec.Tag);
        Hash(hash, ">");
    }

    /// <summary>
    /// Compute the hashcode of a string.
    /// </summary>
    private static void Hash(HashAlgorithm hash, string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        hash.TransformBlock(bytes, 0, bytes.Length, null, 0);
    }
}