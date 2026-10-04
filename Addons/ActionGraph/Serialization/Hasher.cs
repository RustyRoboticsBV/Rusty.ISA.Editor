using System;
using System.Security.Cryptography;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for computing the hashcode of a codec.
/// </summary>
internal static class Hasher
{
    /* Public methods. */
    /// <summary>
    /// Compute the hashcode of a file codec and its children, and store it in the codec.
    /// </summary>
    public static void StoreHash(FileCodec file, HashAlgorithm hash)
    {
        // Compute checksum.
        string hashHex = Hash(file, hash);

        // Store the checksum.
        file.SetAttribute(Codecs.Checksum, hashHex);
    }

    /// <summary>
    /// Compute the hashcode of a codec and its children.
    /// </summary>
    public static string Hash(Codec codec, HashAlgorithm hash)
    {
        hash.Clear();
        Hash(hash, codec, [0x00, 0x00, 0x00, 0x00]);
        hash.TransformFinalBlock([], 0, 0);
        return Convert.ToHexString(hash.Hash);
    }

    /* Private methods. */
    /// <summary>
    /// Compute the hashcode of this codec and its children.
    /// </summary>
    private static void Hash(HashAlgorithm hash, Codec codec, byte[] buffer)
    {
        // Hash tag.
        Hash(hash, codec.Tag.Length, buffer);
        Hash(hash, codec.Tag, buffer);

        // Hash attributes.
        Hash(hash, codec.Attributes.Count, buffer);
        foreach (var attribute in codec.Attributes)
        {
            if (attribute.Key == Codecs.Checksum)
                continue;

            Hash(hash, attribute.Key.Length, buffer);
            Hash(hash, attribute.Key, buffer);

            Hash(hash, attribute.Value.Length, buffer);
            Hash(hash, attribute.Value, buffer);
        }

        // Hash child nodes.
        Hash(hash, codec.Children.Count, buffer);
        foreach (Codec child in codec.Children)
        {
            Hash(hash, child, buffer);
        }
    }

    /// <summary>
    /// Compute the hashcode of a string.
    /// </summary>
    private static void Hash(HashAlgorithm hash, string str, byte[] buffer)
    {
        for (int i = 0; i < str.Length; i++)
        {
            Hash(hash, str[i], buffer);
        }
    }

    /// <summary>
    /// Compute the hashcode of a character.
    /// </summary>
    private static void Hash(HashAlgorithm hash, char chr, byte[] buffer)
    {
        buffer[0] = (byte)chr;
        buffer[1] = (byte)(chr >> 8);
        hash.TransformBlock(buffer, 0, 2, null, 0);
    }

    /// <summary>
    /// Compute the hashcode of an integer.
    /// </summary>
    private static void Hash(HashAlgorithm hash, int value, byte[] buffer)
    {
        buffer[0] = (byte)value;
        buffer[1] = (byte)(value >> 8);
        buffer[2] = (byte)(value >> 16);
        buffer[3] = (byte)(value >> 24);
        hash.TransformBlock(buffer, 0, 4, null, 0);
    }
}