using Godot;
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
            string value = codec.GetAttribute(name);
            switch (name)
            {
                case Codecs.Localizable:
                case Codecs.NoDefault:
                    if (bool.TryParse(value, out bool boolValue))
                        WriteBool(writer, boolValue);
                    else
                        WriteBool(writer, false);
                    break;

                case Codecs.X:
                case Codecs.Y:
                    if (int.TryParse(value, out int intValue))
                        WriteInt(writer, intValue);
                    else
                        WriteInt(writer, 0);
                    break;

                case Codecs.Width:
                case Codecs.Height:
                case Codecs.Index:
                case Codecs.Port:
                    if (uint.TryParse(value, out uint uintValue))
                        WriteUint(writer, uintValue);
                    else
                        WriteUint(writer, 0);
                    break;

                case Codecs.Color:
                    Color colorValue;
                    try
                    {
                        colorValue = Color.FromHtml(value);
                    }
                    catch
                    {
                        colorValue = Colors.Transparent;
                    }
                    WriteColor(writer, colorValue);
                    break;

                default:
                    WriteString(writer, codec.GetAttribute(name));
                    break;
            }
        }

        // Handle children.
        if (codec.AllowsChildren())
        {
            WriteUint(writer, (uint)codec.Children.Count);
            foreach (Codec child in codec.Children)
            {
                WriteCodec(writer, child);
            }
        }
    }

    /// <summary>
    /// Write a boolean to binary, 0x01 for true and 0x00 for false.
    /// </summary>
    private static void WriteBool(BinaryWriter writer, bool value)
    {
        writer.Write((byte)(value ? 0x01 : 0x00));
    }

    /// <summary>
    /// Write a signed integer to binary, using SLEB128 encoding.
    /// </summary>
    private static void WriteInt(BinaryWriter writer, int value)
    {
        int remaining = value;
        bool more;
        do
        {
            byte current = (byte)(remaining & 0x7F);
            remaining >>= 7;

            more = !((remaining == 0 && (current & 0x40) == 0) || (remaining == -1 && (current & 0x40) != 0));
            if (more)
                current |= 0x80;

            writer.Write(current);
        }
        while (more);
    }

    /// <summary>
    /// Write an unsigned integer to binary, using ULEB128 encoding.
    /// </summary>
    private static void WriteUint(BinaryWriter writer, uint value)
    {
        uint remaining = value;
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

    /// <summary>
    /// Write a string to binary, using ULEB128 length-prefixed UTF-8 encoding.
    /// </summary>
    private static void WriteString(BinaryWriter writer, string str)
    {
        if (str == null)
            throw new ArgumentNullException(nameof(str));

        byte[] bytes = Encoding.UTF8.GetBytes(str);
        WriteUint(writer, (uint)bytes.Length);
        writer.Write(bytes);
    }

    /// <summary>
    /// Write a color to binary, using RGBA8 encoding.
    /// </summary>
    private static void WriteColor(BinaryWriter writer, Color color)
    {
        writer.Write(color.R8);
        writer.Write(color.G8);
        writer.Write(color.B8);
        writer.Write(color.A8);
    }
}