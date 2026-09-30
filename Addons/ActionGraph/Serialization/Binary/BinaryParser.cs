using System;
using System.IO;
using System.Text;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for parsing binary AGP as FileCodec objects.
/// </summary>
internal static class BinaryParser
{
    /* Constants. */
    private static readonly byte[] MAGIC = Encoding.ASCII.GetBytes("\0BINAGP\0");

    private const string VERSION = "0.1";

    /* Public methods. */
    /// <summary>
    /// Parse an entire binary AGP file.
    /// </summary>
    public static FileCodec Parse(byte[] data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        using MemoryStream stream = new(data, writable: false);
        using BinaryReader reader = new(stream);

        ReadMagic(reader);
        ReadVersion(reader);

        Codec codec = ReadCodec(reader);

        if (codec is not FileCodec file)
            throw new FormatException($"Expected root codec to be 'file', got '{codec.Tag}'.");

        if (stream.Position != stream.Length)
            throw new FormatException($"Trailing data after root codec at offset {stream.Position}.");

        return file;
    }

    /* Private methods. */
    private static void ReadMagic(BinaryReader reader)
    {
        byte[] magic = ReadExactly(reader, MAGIC.Length);

        for (int i = 0; i < MAGIC.Length; i++)
        {
            if (magic[i] != MAGIC[i])
                throw new FormatException("Invalid AGP binary header.");
        }
    }

    private static void ReadVersion(BinaryReader reader)
    {
        string version = ReadString(reader);

        if (version != VERSION)
            throw new FormatException($"Unsupported AGP binary version '{version}'. Expected '{VERSION}'.");
    }

    private static Codec ReadCodec(BinaryReader reader)
    {
        int tagIndex = reader.ReadByte();

        Codec codec;

        try
        {
            codec = Codecs.Instantiate(tagIndex);
        }
        catch (Exception ex)
        {
            throw new FormatException($"Invalid codec tag index {tagIndex}.", ex);
        }

        ReadAttributes(reader, codec);
        ReadChildren(reader, codec);

        return codec;
    }

    private static void ReadAttributes(BinaryReader reader, Codec codec)
    {
        if (!codec.AllowsAttributes())
            return;

        byte mask = reader.ReadByte();

        for (int i = 0; i < 8; i++)
        {
            if (!Bitmask.GetBit(mask, i))
                continue;

            string value = ReadString(reader);

            string name;
            try
            {
                name = codec.GetAttributeFromIndex(i);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new FormatException($"Codec '{codec.Tag}' contains an invalid attribute bit {i}.", ex);
            }

            codec.SetAttribute(name, value);
        }
    }

    private static void ReadChildren(BinaryReader reader, Codec codec)
    {
        if (!codec.AllowsChildren())
            return;

        int count = ReadUleb128(reader);

        for (int i = 0; i < count; i++)
        {
            Codec child = ReadCodec(reader);

            if (!codec.AllowsChild(child.Tag))
                throw new FormatException($"Codec '{codec.Tag}' cannot contain child '{child.Tag}'.");

            codec.Children.Add(child);
        }
    }

    private static string ReadString(BinaryReader reader)
    {
        int length = ReadUleb128(reader);

        if (length < 0)
            throw new FormatException("Negative string length.");

        if (length > reader.BaseStream.Length - reader.BaseStream.Position)
            throw new EndOfStreamException($"String of {length} bytes exceeds remaining data.");

        byte[] bytes = reader.ReadBytes(length);

        if (bytes.Length != length)
            throw new EndOfStreamException("Unexpected end of stream while reading string.");

        return Encoding.ASCII.GetString(bytes);
    }

    private static int ReadUleb128(BinaryReader reader)
    {
        uint result = 0;
        int shift = 0;

        for (int i = 0; i < 5; i++)
        {
            byte current = reader.ReadByte();

            if (i == 4 && (current & 0xF0) != 0)
                throw new OverflowException("ULEB128 value exceeds Int32.");

            result |= (uint)(current & 0x7F) << shift;

            if ((current & 0x80) == 0)
                return checked((int)result);

            shift += 7;
        }

        throw new FormatException("Invalid ULEB128 sequence.");
    }

    private static byte[] ReadExactly(BinaryReader reader, int count)
    {
        byte[] bytes = reader.ReadBytes(count);

        if (bytes.Length != count)
            throw new EndOfStreamException("Unexpected end of stream.");

        return bytes;
    }
}
