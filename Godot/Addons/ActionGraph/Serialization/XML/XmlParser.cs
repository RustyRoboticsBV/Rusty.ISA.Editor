////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using Godot;
using System;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for parsing strings of XML as FileElement objects.
/// </summary>
internal static class XmlParser
{
    /* Public methods. */
    /// <summary>
    /// Parse a string of XML as a FileElement.
    /// </summary>
    public static FileElement Parse(string xml)
    {
        // Load XML.
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);

        // Parse DOM.
        foreach (XmlNode node in doc)
        {
            if (node is XmlElement element)
            {
                Element codec = ParseXml(element);
                if (codec is FileElement file)
                    return file;
                else
                    throw new InvalidCastException($"XML files must have a <file> root element.");
            }
        }
        throw new FormatException("Empty XML file!");
    }

    /* Private methods. */
    private static Element ParseXml(XmlElement xml)
    {
        int elementIndex = Schema.FindElement(xml.Name);
        Schema.Element element = Schema.Elements[elementIndex];
        
        Element result = Schema.Instantiate(elementIndex);

        foreach (XmlNode child in xml.ChildNodes)
        {
            if (child is XmlElement childElement)
                result.AddChild(ParseXml(childElement));
        }

        foreach (XmlAttribute attribute in xml.Attributes)
        {
            int attributeIndex = element.FindAttribute(attribute.Name);
            Schema.ValueType type = element.Attributes[attributeIndex].ValueType;
            object value = ParseValue(type, attribute.Value);
            result.SetAttribute(attributeIndex, value);
        }

        return result;
    }

    private static object ParseValue(Schema.ValueType type, string value) => type switch
    {
        Schema.ValueType.Bool => bool.Parse(value),
        Schema.ValueType.Int => int.Parse(value),
        Schema.ValueType.Uint => uint.Parse(value),
        Schema.ValueType.String => value,
        Schema.ValueType.Color => Color.FromHtml(value),
        Schema.ValueType.Ref => value,
        _ => throw new InvalidOperationException("Invalid type.")
    };
}