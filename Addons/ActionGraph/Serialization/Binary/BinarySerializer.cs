using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for serializing FileCodec objects to binary.
/// </summary>
internal static class BinarySerializer
{
    /// <summary>
    /// Serialize a FileCodec to a string of XML.
    /// </summary>
    public static byte[] Serialize(FileCodec file)
    {
        // Compute checksum.
        MD5 md5 = MD5.Create();
        string hashHex = Hasher.Hash(file, md5);
        file.SetAttribute(Codecs.Checksum, hashHex);

        // Serialize.
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);

        writer.Write(Encoding.ASCII.GetBytes("\0BINAGP\0"));
        writer.Write(BinaryStringValue.Encode("0.1"));
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
        // Handle tag.
        writer.Write((byte)Codecs.GetIndex(codec));

        // Handle attributes.
        if (codec.AllowsAttributes())
        {
            BinaryAttributes attributes = new();
            foreach (var attr in codec.Attributes)
            {
                int index = codec.GetAttributeIndex(attr.Key);
                if (index == -1)
                    throw new KeyNotFoundException($"Codec '{codec.GetType().Name}' does not allow name {attr.Key}.");

                if (attributes[index] == null)
                    attributes[index] = attr.Value;
            }

            writer.Write(attributes.GetBitmask());
            for (int i = 0; i < 8; i++)
            {
                if (attributes[i] != null)
                    writer.Write(BinaryStringValue.Encode(attributes[i]));
            }
        }

        // Handle children.
        if (codec.AllowsChildren())
        {
            writer.Write(Uleb128.Encode(codec.Children.Count));
            foreach (Codec child in codec.Children)
            {
                if (!codec.AllowsChild(child.Tag))
                    throw new KeyNotFoundException($"Codec '{codec.GetType().Name}' does not allow child elements with xml tag '{child.Tag}'.");
                WriteCodec(writer, child);
            }
        }
    }
}