public class BinaryParserGenerator : Generator
{
    public string Generate(Schema schema)
    {
        string fileSnake = schema.Sealeds[0].Name;
        string fileElement = Names.ClassName(fileSnake);

        return @$"{Notice}

using Godot;
using System;
using System.IO;
using System.Text;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for parsing binary files as {fileElement} objects.
/// </summary>
internal static class BinaryParser
{{
    /* Constants. */
    private static readonly byte[] MagicBytes = Encoding.UTF8.GetBytes(""{Magic.binaryHeader}"");
    private const string Version = ""{Magic.version}"";

    /* Public methods. */
    /// <summary>
    /// Parse an entire binary file.
    /// </summary>
    public static {fileElement} Parse(string path)
    {{
        return Parse(File.ReadAllBytes(path));
    }}

    /// <summary>
    /// Parse an entire binary file.
    /// </summary>
    public static {fileElement} Parse(byte[] data)
    {{
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        using MemoryStream stream = new(data, false);
        using BinaryReader reader = new(stream);

        ReadMagic(reader);
        ReadVersion(reader);

        Element element = ReadElement(reader);

        if (element is not {fileElement} file)
            throw new FormatException($""Expected root element to be '{fileSnake}', got '{{element.ElementName}}'."");

        if (stream.Position != stream.Length)
            throw new FormatException($""Trailing data after '{fileSnake}' element at offset 0x{{stream.Position:X}}."");

        return file;
    }}

    /* Private methods. */
    /// <summary>
    /// Read the magic header.
    /// </summary>
    private static void ReadMagic(BinaryReader reader)
    {{
        byte[] magic = ReadBytes(reader, (uint)MagicBytes.Length);

        for (int i = 0; i < MagicBytes.Length; i++)
        {{
            if (magic[i] != MagicBytes[i])
                throw new FormatException(""Invalid magic header."");
        }}
    }}

    /// <summary>
    /// Read the format version.
    /// </summary>
    private static void ReadVersion(BinaryReader reader)
    {{
        string version = ReadString(reader);

        if (version != Version)
            throw new FormatException($""Unsupported version '{{version}}', expected '{{Version}}'."");
    }}

    /// <summary>
    /// Read an element.
    /// </summary>
    private static Element ReadElement(BinaryReader reader)
    {{
        // Instantiate element of correct type.
        int index = reader.ReadByte();
        if (index >= Elements.ELEMENT_COUNT)
            throw FormatError(reader, $""Invalid element index {{index}}."");

        Element element = Elements.Instantiate(index);

        // Read attributes.
        for (int i = 0; i < element.AttributeCount; i++)
        {{
            if (element.TypeOfAttribute(i) == typeof(bool))
                element.SetAttribute(i, ReadBool(reader));
            else if (element.TypeOfAttribute(i) == typeof(uint))
                element.SetAttribute(i, ReadUint(reader));
            else if (element.TypeOfAttribute(i) == typeof(int))
                element.SetAttribute(i, ReadInt(reader));
            else if (element.TypeOfAttribute(i) == typeof(double))
                element.SetAttribute(i, ReadDouble(reader));
            else if (element.TypeOfAttribute(i) == typeof(string))
                element.SetAttribute(i, ReadString(reader));
            else if (element.TypeOfAttribute(i) == typeof(Color))
                element.SetAttribute(i, ReadColor(reader));
        }}
        
        // Read children.
        if (element.AllowsChildren)
        {{
            uint childCount = ReadUint(reader);
            for (int i = 0; i < childCount; i++)
            {{
                Element child = ReadElement(reader);
                element.AddChild(child);
            }}
        }}

        return element;
    }}

    /// <summary>
    /// Read a boolean value.
    /// </summary>
    private static bool ReadBool(BinaryReader reader)
    {{
        if (reader.BaseStream.Position == reader.BaseStream.Length)
            throw new EndOfStreamException($""Unexpected end of stream while reading boolean."");

        byte val = reader.ReadByte();
        if (val == 0x00)
            return false;
        else if (val == 0x01)
            return true;
        else
            throw FormatError(reader, $""Invalid boolean value '{{val}}'."");
    }}

    /// <summary>
    /// Read an ULEB128 value.
    /// </summary>
    private static uint ReadUint(BinaryReader reader)
    {{
        uint result = 0;
        int shift = 0;
        byte @byte;
        do
        {{
            if (reader.BaseStream.Position == reader.BaseStream.Length)
                throw new EndOfStreamException($""Unexpected end of stream while reading ULEB128."");

            if (shift >= 7 * 5)
                throw FormatError(reader, ""Size of ULEB128 exceeds size of Uint32."");

            @byte = reader.ReadByte();
            if (shift == 7 * 4 && (@byte & 0xF0) != 0)
                throw FormatError(reader, ""Invalid final byte of ULEB128."");

            result |= (uint)((@byte & 0x7F) << shift);
            shift += 7;
        }}
        while ((@byte & 0x80) != 0);

        return result;
    }}

    /// <summary>
    /// Read an IEEE 754 Binary64 (little-endian) value.
    /// </summary>
    private static double ReadDouble(BinaryReader reader)
    {{
        ulong bits = reader.ReadUInt64();
        return BitConverter.UInt64BitsToDouble(bits);
    }}

    /// <summary>
    /// Read an SLEB128 value.
    /// </summary>
    private static int ReadInt(BinaryReader reader)
    {{
        const int size = 32;
        int result = 0;
        int shift = 0;
        byte @byte;
        do
        {{
            if (reader.BaseStream.Position == reader.BaseStream.Length)
                throw new EndOfStreamException($""Unexpected end of stream while reading SLEB128."");

            if (shift >= 7 * 5)
                throw FormatError(reader, ""Size of SLEB128 exceeds size of Int32."");

            @byte = reader.ReadByte();
            if (shift == 7 * 4 && (@byte & 0x70) != 0 && (@byte & 0x70) != 0x70)
                throw FormatError(reader, ""Invalid final byte of SLEB128."");

            result |= (@byte & 0x7F) << shift;
            shift += 7;
        }}
        while ((@byte & 0x80) != 0);

        if ((shift < size) && ((@byte & 0x40) != 0))
            result |= ~0 << shift;
 
        return result;
    }}

    /// <summary>
    /// Read a length-prefixed string value.
    /// </summary>
    private static string ReadString(BinaryReader reader)
    {{
        uint length = ReadUint(reader);
        if (length > reader.BaseStream.Length - reader.BaseStream.Position)
            throw new EndOfStreamException($""String of {{length}} bytes exceeds remaining data."");

        byte[] bytes = ReadBytes(reader, length);
        if (bytes.Length != length)
            throw new EndOfStreamException(""Unexpected end of stream while reading string."");

        return Encoding.UTF8.GetString(bytes);
    }}

    /// <summary>
    /// Read an RGBA8 color value.
    /// </summary>
    private static Color ReadColor(BinaryReader reader)
    {{
        if (4 > reader.BaseStream.Length - reader.BaseStream.Position)
            throw new EndOfStreamException($""Unexpected end of stream while reading RGBA8 color."");
        byte r = reader.ReadByte();
        byte g = reader.ReadByte();
        byte b = reader.ReadByte();
        byte a = reader.ReadByte();
        return new Color(r, g, b, a);
    }}

    /// <summary>
    /// Read a number of bytes.
    /// </summary>
    private static byte[] ReadBytes(BinaryReader reader, uint count)
    {{
        byte[] bytes = reader.ReadBytes((int)count);

        if (bytes.Length != count)
            throw new EndOfStreamException(""Unexpected end of stream."");

        return bytes;
    }}
    
    private static FormatException FormatError(BinaryReader reader, string message)
        => new FormatException($""At 0x{{reader.BaseStream.Position:X}}: {{message}}"");
}}";
    }
}