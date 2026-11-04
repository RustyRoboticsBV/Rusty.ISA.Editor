////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for serializing FileElement objects to XML.
/// </summary>
internal static class XmlSerializer
{
    /// <summary>
    /// Serialize a FileElement to a string of XML.
    /// </summary>
    public static string Serialize(FileElement file)
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
            SerializeElement(file, writer);
            writer.Flush();
        }

        return output.ToString();
    }

    /* Private methods. */
    /// <summary>
    /// Convert this node to XML.
    /// </summary>
    private static void SerializeElement(Element element, XmlWriter writer)
    {
        // Handle start tag.
        int elementIndex = Schema.FindElement(element.GetType());
        writer.WriteStartElement(Schema.Elements[elementIndex].Name);

        // Handle attributes.
        for (int i = 0; i < element.AttributeCount; i++)
        {
            writer.WriteStartAttribute(Schema.Elements[elementIndex].Attributes[element.AttributeCount].Name);
            writer.WriteValue(element.GetAttribute(i));
            writer.WriteEndAttribute();
        }

        // Handle children.
        for (int i = 0; i < element.ChildCount; i++)
        {
            SerializeElement(element.GetChild(i), writer);
        }

        // Handle end tag.
        writer.WriteEndElement();
    }
}