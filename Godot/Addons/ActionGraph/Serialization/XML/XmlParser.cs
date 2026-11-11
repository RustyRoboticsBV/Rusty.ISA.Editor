////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated file. It should not be edited directly.                //
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
        int elementIndex = Elements.IndexOfElement(xml.Name);
        Element element = Elements.Instantiate(elementIndex);
        
        foreach (XmlNode child in xml.ChildNodes)
        {
            if (child is XmlElement childElement)
                element.AddChild(ParseXml(childElement));
        }
        
        foreach (XmlAttribute attribute in xml.Attributes)
        {
            int attributeIndex = element.IndexOfAttribute(attribute.Name);
            Type type = element.TypeOfAttribute(attributeIndex);
            
            if (type == typeof(bool))
                element.SetAttribute(attributeIndex, bool.Parse(attribute.Value));
            else if (type == typeof(uint))
                element.SetAttribute(attributeIndex, uint.Parse(attribute.Value));
            else if (type == typeof(int))
                element.SetAttribute(attributeIndex, int.Parse(attribute.Value));
            else if (type == typeof(string))
                element.SetAttribute(attributeIndex, attribute.Value);
            else if (type == typeof(Color))
                element.SetAttribute(attributeIndex, Color.FromHtml(attribute.Value));
            else
                throw new InvalidOperationException($"Invalid type {type}.");
        }
        
        return element;
    }
}