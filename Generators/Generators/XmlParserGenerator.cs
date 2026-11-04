public class XmlParserGenerator : Generator
{
    public string Generate(Schema schema)
    {
        string fileSnake = schema.Sealeds[0].Name;
        string fileElement = Names.ElementName(fileSnake);

        return @$"{Notice}

using Godot;
using System;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

using ValueType = Schema.ValueType;

/// <summary>
/// A utility for parsing strings of XML as {fileElement} objects.
/// </summary>
internal static class XmlParser
{{
    /* Public methods. */
    /// <summary>
    /// Parse a string of XML as a {fileElement}.
    /// </summary>
    public static {fileElement} Parse(string xml)
    {{
        // Load XML.
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);

        // Parse DOM.
        foreach (XmlNode node in doc)
        {{
            if (node is XmlElement element)
            {{
                Element codec = ParseXml(element);
                if (codec is {fileElement} file)
                    return file;
                else
                    throw new InvalidCastException($""XML files must have a <{fileSnake}> root element."");
            }}
        }}
        throw new FormatException(""Empty XML file!"");
    }}

    /* Private methods. */
    private static Element ParseXml(XmlElement xml)
    {{
        int elementIndex = Schema.FindElement(xml.Name);
        Schema.Element element = Schema.Elements[elementIndex];
        
        Element result = Schema.Instantiate(elementIndex);

        foreach (XmlNode child in xml.ChildNodes)
        {{
            if (child is XmlElement childElement)
                result.AddChild(ParseXml(childElement));
        }}

        foreach (XmlAttribute attribute in xml.Attributes)
        {{
            int attributeIndex = element.FindAttribute(attribute.Name);
            ValueType type = element.Attributes[attributeIndex].ValueType;
            object value = ParseValue(type, attribute.Value);
            result.SetAttribute(attributeIndex, value);
        }}

        return result;
    }}

    private static object ParseValue(ValueType type, string value) => type switch
    {{
        ValueType.Bool => bool.Parse(value),
        ValueType.Int => int.Parse(value),
        ValueType.Uint => uint.Parse(value),
        ValueType.String => value,
        ValueType.Color => Color.FromHtml(value),
        _ => throw new InvalidOperationException($""Invalid type {{type}}."")
    }};
}}";
    }
}