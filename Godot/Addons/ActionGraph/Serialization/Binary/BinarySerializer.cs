////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated file. It should not be edited directly.                //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using Godot;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for serializing FileElement objects to binary.
/// </summary>
internal static class BinarySerializer
{
    /* Constants. */
    private static readonly byte[] MagicBytes = Encoding.UTF8.GetBytes("AGBP\0\0\0\0");
    private const string Version = "1.0";

    /// <summary>
    /// Serialize a FileElement to a string of XML.
    /// </summary>
    public static byte[] Serialize(FileElement file)
    {
        // Compute checksum.
        Hasher.StoreHash(file, MD5.Create());

        // Serialize.
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);

        writer.Write(MagicBytes);
        WriteString(writer, Version);
        WriteElement(writer, file);

        writer.Close();
        return stream.ToArray();
    }

    /* Private methods. */
    /// <summary>
    /// Convert this node to binary.
    /// </summary>
    private static void WriteElement(BinaryWriter writer, Element element)
    {
        // Write tag index.
        writer.Write((byte)Elements.IndexOfElement(element.ElementName));

        // Write attributes.
        for (int i = 0; i < element.AttributeCount; i++)
        {
            object value = element.GetAttribute(i);
            switch (value)
            {
                case bool @bool:
                    WriteBool(writer, @bool);
                    break;
                case uint @uint:
                    WriteUint(writer, @uint);
                    break;
                case int @int:
                    WriteInt(writer, @int);
                    break;
                case double @double:
                    WriteDouble(writer, @double);
                    break;
                case Color color:
                    WriteColor(writer, color);
                    break;
                case string str:
                    WriteString(writer, str);
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported value type {value.GetType().Name}");
            }
        }

        // Handle children.
        if (element.AllowsChildren)
        {
            WriteUint(writer, (uint)element.ChildCount);
            for (int i = 0; i < element.ChildCount; i++)
            {
                WriteElement(writer, element.GetChild(i));
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
    /// Write an unsigned integer to binary, using ULEB128 encoding.
    /// </summary>
    private static void WriteUint(BinaryWriter writer, uint value)
    {
        do
        {
            byte @byte = (byte)(value & 0x7F);
            value >>= 7;

            if (value != 0)
                @byte |= 0x80;

            writer.Write(@byte);
        }
        while (value != 0);
    }

    /// <summary>
    /// Write a signed integer to binary, using SLEB128 encoding.
    /// </summary>
    private static void WriteInt(BinaryWriter writer, int value)
    {
        const int size = 32;
        bool more = true;
        bool negative = value < 0;
        while (more)
        {
            byte @byte = (byte)(value & 0x7F);
            value >>= 7;

            if (negative)
                value |= ~0 << (size - 7);

            int signBit = @byte & 0x40;
            if ((value == 0 && signBit == 0) || (value == -1 && signBit != 0))
                more = false;
            else
                @byte |= 0x80;

            writer.Write(@byte);
        }
    }

    /// <summary>
    /// Write a double to binary, using IEEE 754 Binary64 (little-endian) encoding.
    /// </summary>
    private static void WriteDouble(BinaryWriter writer, double value)
    {
        ulong bits = BitConverter.DoubleToUInt64Bits(value);
        writer.Write(bits);
    }

    /// <summary>
    /// Write a string to binary, using ULEB128 length-prefixed UTF-8 encoding.
    /// </summary>
    private static void WriteString(BinaryWriter writer, string value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        byte[] bytes = Encoding.UTF8.GetBytes(value);
        WriteUint(writer, (uint)bytes.Length);
        writer.Write(bytes);
    }

    /// <summary>
    /// Write a color to binary, using RGBA8 encoding.
    /// </summary>
    private static void WriteColor(BinaryWriter writer, Color value)
    {
        writer.Write((byte)value.R8);
        writer.Write((byte)value.G8);
        writer.Write((byte)value.B8);
        writer.Write((byte)value.A8);
    }
}