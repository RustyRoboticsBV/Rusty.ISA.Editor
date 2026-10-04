using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for serializing FileCodec objects to XML.
/// </summary>
internal static class XmlSerializer
{
    /// <summary>
    /// Serialize a FileCodec to a string of XML.
    /// </summary>
    public static string Serialize(FileCodec file)
    {
        // Compute checksum.
        Hasher.StoreHash(file, MD5.Create());

        // Serialize.
        XmlWriterSettings settings = new()
        {
            Indent = true,
            Encoding = new UTF8Encoding(false),
            OmitXmlDeclaration = false
        };

        StringBuilder output = new();
        using (var writer = XmlWriter.Create(output, settings))
        {
            SerializeCodec(file, writer);
            writer.Flush();
        }

        return output.ToString();
    }

    /* Private methods. */
    /// <summary>
    /// Convert this node to XML.
    /// </summary>
    private static void SerializeCodec(Codec codec, XmlWriter writer)
    {
        // Handle start tag.
        writer.WriteStartElement(codec.Tag);

        // Handle attributes.
        foreach (var attribute in codec.Attributes)
        {
            writer.WriteStartAttribute(attribute.Key);
            writer.WriteValue(attribute.Value);
            writer.WriteEndAttribute();
        }

        // Handle children.
        foreach (Codec child in codec.Children)
        {
            SerializeCodec(child, writer);
        }

        // Handle end tag.
        writer.WriteEndElement();
    }
}