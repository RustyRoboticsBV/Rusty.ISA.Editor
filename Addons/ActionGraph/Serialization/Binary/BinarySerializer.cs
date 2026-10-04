using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for serializing FileCodec objects to binary.
/// </summary>
internal static class BinarySerializer
{
    /* Constants. */
    private static readonly byte[] MAGIC = Encoding.UTF8.GetBytes("\0BINAGP\0");
    private const string VERSION = "1.0";

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

        writer.Write(MAGIC);
        writer.Write(BinaryStringValue.Encode(VERSION));
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
        int tagIndex = Codecs.GetIndex(codec);
        if (tagIndex == -1)
            throw new ArgumentException($"Unknown codec '{codec.GetType().Name}'.");

        writer.Write((byte)tagIndex);

        // Handle attributes.
        if (codec.AllowsAttributes())
        {
            BinaryAttributes attributes = new();
            foreach (var attr in codec.Attributes)
            {
                int attrIndex = codec.GetAttributeIndex(attr.Key);
                if (attrIndex == -1)
                    throw new KeyNotFoundException($"Codec '{codec.GetType().Name}' does not allow name {attr.Key}.");

                if (attributes[attrIndex] == null)
                    attributes[attrIndex] = attr.Value;
            }

            writer.Write(attributes.GetBitmask());
            for (int i = 0; i < BinaryAttributes.Size; i++)
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
                    throw new KeyNotFoundException($"Codec '{codec.GetType().Name}' does not allow child elements with tag '{child.Tag}'.");
                WriteCodec(writer, child);
            }
        }
    }
}