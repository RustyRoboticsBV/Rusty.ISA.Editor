public class XmlSerializerGenerator : Generator
{
    public string Generate(Schema schema)
    {
        string fileSnake = schema.Sealeds[0].Name;
        string fileElement = Names.ElementName(fileSnake);

        return @$"{Notice}

using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for serializing {fileElement} objects to XML.
/// </summary>
internal static class XmlSerializer
{{
    /* Public methods. */
    /// <summary>
    /// Serialize a {fileElement} to a string of XML.
    /// </summary>
    public static string Serialize({fileElement} file)
    {{
        // Compute checksum.
        Hasher.StoreHash(file, MD5.Create());

        // Serialize.
        XmlWriterSettings settings = new()
        {{
            Indent = true,
            Encoding = new UTF8Encoding(false),
            OmitXmlDeclaration = false
        }};

        StringBuilder output = new();
        using (var writer = XmlWriter.Create(output, settings))
        {{
            SerializeElement(file, writer);
            writer.Flush();
        }}

        return output.ToString();
    }}

    /* Private methods. */
    /// <summary>
    /// Convert this node to XML.
    /// </summary>
    private static void SerializeElement(Element element, XmlWriter writer)
    {{
        // Handle start tag.
        int elementIndex = Schema.FindElement(element.GetType());
        Schema.Element schemaElement = Schema.Elements[elementIndex];
        writer.WriteStartElement(schemaElement.Name);

        // Handle attributes.
        for (int i = 0; i < element.AttributeCount; i++)
        {{
            Schema.Attribute schemaAttribute = schemaElement.Attributes[element.AttributeCount];
            writer.WriteStartAttribute(schemaAttribute.Name);
            writer.WriteValue(element.GetAttribute(i));
            writer.WriteEndAttribute();
        }}

        // Handle children.
        for (int i = 0; i < element.ChildCount; i++)
        {{
            SerializeElement(element.GetChild(i), writer);
        }}

        // Handle end tag.
        writer.WriteEndElement();
    }}
}}";
    }
}