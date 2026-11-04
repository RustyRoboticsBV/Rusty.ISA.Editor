public class SchemaGenerator : Generator
{
    public string Generate(Schema schema)
    {
        CodeWriter elements = new();
        elements.Indent = 2;
        foreach (Sealed @sealed in schema.Sealeds)
        {
            if (!elements.IsEmpty)
                elements.Append(',');

            CodeWriter attributes = new();
            foreach (Attribute attribute in @sealed.Attributes)
            {
                if (!attributes.IsEmpty)
                    attributes.Append(',');
                if (attribute.ValueType == ValueType.Ref)
                    attributes.Append($"new(\"{attribute.Name}\", ValueType.{ValueType.String}");
                else
                    attributes.Append($"new(\"{attribute.Name}\", ValueType.{attribute.ValueType}");
                attributes.Append($")");
            }

            elements.Line($"new(typeof({Names.ElementName(@sealed.Name)}), \"{@sealed.Name}\", [{attributes}])");
        }

        return @$"{Notice}

using System;

namespace Rusty.ActionGraph.Serialization;

using SElement = Element;

/// <summary>
/// The ActionGraph schema.
/// </summary>
internal static class Schema
{{
    /* Public types. */
    public enum ValueType {{ Bool, Uint, Int, Color, String }}
    
    public class Attribute
    {{
        public string Name {{ get; init; }}
        public ValueType ValueType {{ get; init; }}
        
        public Attribute(string name, ValueType valueType)
        {{
            Name = name;
            ValueType = valueType;
        }}
    }}
    
    public class Element
    {{
        public Type Type {{ get; init; }}
        public string Name {{ get; init; }}
        public Attribute[] Attributes {{ get; init; }}

        public Element(Type type, string name, Attribute[] attributes)
        {{
            Type = type;
            Name = name;
            Attributes = attributes;
        }}

        public int FindAttribute(string attributeName)
        {{
            for (int i = 0; i < Attributes.Length; i++)
            {{
                if (Attributes[i].Name == attributeName)
                    return i;
            }}
            throw new ArgumentException($""Unknown element '{{attributeName}}'."", nameof(attributeName));
        }}
    }}
    
    /* Public properties. */
    public static Element[] Elements {{ get; }} =
    [
{elements}
    ];
    
    /* Public methods. */
    public static int FindElement(Type type)
    {{
        for (int i = 0; i < Elements.Length; i++)
        {{
            if (Elements[i].Type == type)
                return i;
        }}
        throw new ArgumentException($""Unknown element '{{type.Name}}'."", nameof(type));
    }}
    
    public static int FindElement(string elementName)
    {{
        for (int i = 0; i < Elements.Length; i++)
        {{
            if (Elements[i].Name == elementName)
                return i;
        }}
        throw new ArgumentException($""Unknown element '{{elementName}}'."", nameof(elementName));
    }}
    
    public static SElement Instantiate(int index) => (SElement)Activator.CreateInstance(Elements[index].Type);
}}";
    }
}