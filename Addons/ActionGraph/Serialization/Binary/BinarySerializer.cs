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
    private static readonly byte[] MagicBytes = Encoding.UTF8.GetBytes("\0BINAGP\0");
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
        if (codec.AllowsAttributes())
        {
            AttributeMask attributes = new();
            foreach (var attribute in codec.Attributes)
            {
                int index = codec.GetAttributeIndex(attribute.Key);
                if (attributes[index] == null)
                    attributes[index] = attribute.Value;
            }

            writer.Write(attributes.GetBitmask());
            for (int i = 0; i < AttributeMask.Size; i++)
            {
                if (attributes[i] != null)
                    WriteString(writer, attributes[i]);
            }
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
        byte[] value = Encoding.UTF8.GetBytes(str ?? "");
        WriteUint(writer, value.Length);
        writer.Write(value);
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
}