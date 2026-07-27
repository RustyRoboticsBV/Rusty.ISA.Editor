using System;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for parsing strings of XML as FileCodec objects.
/// </summary>
public static class Parser
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
            if (node is XmlElement)
            {
                Codec codec = Codec.Load(node);
                if (codec is FileCodec file)
                    return file;
                else
                    throw new InvalidCastException($"Files must have a <{FileCodec.TAG}> root element.");
            }
        }
        throw new FormatException("Empty XML file!");
    }
}