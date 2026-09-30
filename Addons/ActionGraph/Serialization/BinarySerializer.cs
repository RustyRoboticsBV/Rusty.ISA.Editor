using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Godot;

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
        file.SetAttribute(Codec.Checksum, hashHex);

        // Serialize.
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);

        writer.Write(Encoding.ASCII.GetBytes("~BAGP"));
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
        GD.Print("Codec " + codec.Tag + " has index " + (byte)GetIndex(codec));
        writer.Write((byte)GetIndex(codec));

        // Handle attributes.
        writer.Write(Uleb128.Encode(codec.Attributes.Count));
        foreach (var attr in codec.Attributes)
        {
            if (!codec.AllowsAttribute(attr.Key))
                throw new KeyNotFoundException($"Codec '{codec.GetType().Name}' does not allow name {attr.Key}.");

            writer.Write(BinaryStringValue.Encode(attr.Key));
            writer.Write(BinaryStringValue.Encode(attr.Value));
        }

        // Handle children.
        writer.Write(Uleb128.Encode(codec.Children.Count));
        foreach (Codec child in codec.Children)
        {
            if (!codec.AllowsChild(child.Tag))
                throw new KeyNotFoundException($"Codec '{codec.GetType().Name}' does not allow child elements with xml tag '{child.Tag}'.");
            WriteCodec(writer, child);
        }
    }

    /// <summary>
    /// Insert an XML comment before a block of XM elements and return the result.
    /// </summary>
    private static int GetIndex(Codec codec)
    {
        return codec switch
        {
            FileCodec => 0,

            // Metadata.
            MetaCodec => 1,
            LangCodec => 2,

            // Schema.
            IdefCodec => 3,
            PdefCodec => 4,

            NdefCodec => 5,

            FdefCodec => 6,
            OdefCodec => 7,
            CdefCodec => 8,
            TdefCodec => 9,
            LdefCodec => 10,

            VdefCodec => 11,
            JdefCodec => 12,

            // Graph.
            NodeCodec => 13,
            JointCodec => 14,
            FrameCodec => 15,
            MemoCodec => 16,

            EdgeCodec => 17,

            FormCodec => 18,
            OptionCodec => 19,
            ChoiceCodec => 20,
            TupleCodec => 21,
            ListCodec => 22,

            ArgCodec => 23,
            LocCodec => 24,
            OutCodec => 25,

            _ => throw new InvalidOperationException($"Unknown codec '{codec.Tag}'.")
        };
    }
}