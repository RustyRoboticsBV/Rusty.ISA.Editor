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
        writer.Write((byte)Codecs.GetIndex(codec));

        // Handle attributes.
        if (codec.AllowsAttributes())
        {
            BinaryAttributes attributes = new();
            foreach (var attribute in codec.Attributes)
            {
                int index = codec.GetAttributeIndex(attribute.Key);
                if (attributes[index] == null)
                    attributes[index] = attribute.Value;
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
                WriteCodec(writer, child);
            }
        }
    }
}