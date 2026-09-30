using System;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for parsing strings of XML as FileCodec objects.
/// </summary>
internal static class XmlParser
{
    /// <summary>
    /// Parse a string of XML as a FileCodec.
    /// </summary>
    public static FileCodec Parse(string xml)
    {
        // Load XML.
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);

        // Parse DOM.
        foreach (XmlNode node in doc)
        {
            if (node is XmlElement element)
            {
                Codec codec = CodecFromXml(element);
                if (codec is FileCodec file)
                    return file;
                else
                    throw new InvalidCastException($"Files must have a <{FileCodec.TAG}> root element.");
            }
        }
        throw new FormatException("Empty XML file!");
    }

    /* Private methods. */
    private static Codec CodecFromXml(XmlElement xml)
    {
        Codec codec = Codecs.Instantiate(xml.Name);

        foreach (XmlNode child in xml.ChildNodes)
        {
            if (child is XmlElement element)
                codec.AddChild(CodecFromXml(element));
        }

        foreach (XmlAttribute attribute in xml.Attributes)
        {
            codec.SetAttribute(attribute.Name, attribute.Value);
        }

        return codec;
    }
}