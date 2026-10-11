using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Rusty.ActionGraph.Serialization.Binary;

/// <summary>
/// A utility for serializing FileCodec objects to binary.
/// </summary>
internal static class BinarySerializer
{
    /* Constants. */
    private static readonly byte[] MagicBytes = Encoding.UTF8.GetBytes("AGBP\0\0\0\0");
    private const string Version = "1.0";

    /// <summary>
    /// Serialize a FileCodec to a string of XML.
    /// </summary>
    public static byte[] Serialize(FileCodec file)
    {
        // Compute checksum.
        Hasher.StoreHash(file, MD5.Create());

        // Serialize.
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);

        writer.Write(MagicBytes);
        WriteString(writer, Version);
        WriteCodec(writer, file);

        writer.Close();
        return stream.ToArray();
    }

    /* Private methods. */
    /// <summary>
    /// Convert this node to binary.
    /// </summary>
    private static void WriteCodec(BinaryWriter writer, Codec codec)
    {
        // Write tag index.
        writer.Write((byte)Codecs.GetIndex(codec));

        // Write attributes.
        for (int i = 0; i < codec.AllowedAttributeCount; i++)
        {
            string name = codec.GetAttributeNameFromIndex(i);
            WriteString(writer, codec.GetAttribute(name));
        }

        // Handle children.
        if (codec.AllowsChildren())
        {
            WriteUint(writer, codec.Children.Count);
            foreach (Codec child in codec.Children)
            {
                WriteCodec(writer, child);
            }
        }
    }

    private static void WriteString(BinaryWriter writer, string str)
    {
        // Write length.
        WriteUint(writer, Encoding.UTF8.GetByteCount(str));

        // Write string in UTF-8 encoding.
        Encoder encoder = Encoding.UTF8.GetEncoder();
        Span<byte> buffer = stackalloc byte[512];
        int charOffset = 0;
        while (charOffset < str.Length)
        {
            encoder.Convert(str.AsSpan(charOffset), buffer, true, out int charsUsed, out int bytesUsed, out _);
            writer.Write(buffer.Slice(0, bytesUsed));
            charOffset += charsUsed;
        }
    }

    private static void WriteUint(BinaryWriter writer, int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "ULEB128 values must be non-negative.");

        uint remaining = (uint)value;
        do
        {
            byte current = (byte)(remaining & 0x7F);
            remaining >>= 7;

            if (remaining != 0)
                current |= 0x80;

            writer.Write(current);
        }
        while (remaining != 0);
    }

    private static void WriteColor(BinaryWriter writer, byte r, byte g, byte b, byte a)
    {
        writer.Write(r);
        writer.Write(g);
        writer.Write(b);
        writer.Write(a);
    }
}