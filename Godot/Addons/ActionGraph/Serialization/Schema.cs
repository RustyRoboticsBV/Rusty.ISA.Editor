////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using System;

namespace Rusty.ActionGraph.Serialization;

using SElement = Element;

/// <summary>
/// The ActionGraph schema.
/// </summary>
internal static class Schema
{
    /* Public types. */
    public enum ValueType { Bool, Uint, Int, Color, String, Ref }
    
    public class Attribute
    {
        public string Name { get; init; }
        public ValueType ValueType { get; init; }
        public Element RefType { get; init; }

        public Attribute(string name, ValueType valueType, Element refType = null)
        {
            Name = name;
            ValueType = valueType;
            RefType = refType;
        }
    }
    
    public class Element
    {
        public Type Type { get; init; }
        public string Name { get; init; }
        public Attribute[] Attributes { get; init; }

        public Element(Type type, string name, Attribute[] attributes)
        {
            Type = type;
            Name = name;
            Attributes = attributes;
        }

        public int FindAttribute(string attributeName)
        {
            for (int i = 0; i < Attributes.Length; i++)
            {
                if (Attributes[i].Name == attributeName)
                    return i;
            }
            throw new ArgumentException($"Unknown element '{attributeName}'.", nameof(attributeName));
        }
    }
    
    /* Public properties. */
    public static Element[] Elements { get; } =
    [
        new(typeof(FileElement), "file", [new("id", ValueType.String),new("editor", ValueType.String),new("schema_version", ValueType.String),new("format_version", ValueType.String),new("checksum", ValueType.String)]),
        new(typeof(MetadataElement), "metadata", [new("id", ValueType.String),new("key", ValueType.String),new("value", ValueType.String)]),
        new(typeof(LanguageDefinitionElement), "language_definition", [new("id", ValueType.String)]),
        new(typeof(InstructionDefinitionElement), "instruction_definition", [new("id", ValueType.String),new("execution_handler", ValueType.String)]),
        new(typeof(ParameterDefinitionElement), "parameter_definition", [new("id", ValueType.String),new("localizable", ValueType.Bool)]),
        new(typeof(NodeDefinitionElement), "node_definition", [new("id", ValueType.String)]),
        new(typeof(FormDefinitionElement), "form_definition", [new("id", ValueType.String),new("instruction_definition", ValueType.Ref, Elements[3])]),
        new(typeof(OptionDefinitionElement), "option_definition", [new("id", ValueType.String)]),
        new(typeof(ChoiceDefinitionElement), "choice_definition", [new("id", ValueType.String)]),
        new(typeof(TupleDefinitionElement), "tuple_definition", [new("id", ValueType.String)]),
        new(typeof(ListDefinitionElement), "list_definition", [new("id", ValueType.String)]),
        new(typeof(FieldDefinitionElement), "field_definition", [new("id", ValueType.String),new("parameter", ValueType.Ref, Elements[4])]),
        new(typeof(PortDefinitionElement), "port_definition", [new("id", ValueType.String),new("parameter", ValueType.Ref, Elements[4]),new("remove_default_port", ValueType.Bool)]),
        new(typeof(NodeElement), "node", [new("id", ValueType.String),new("definition", ValueType.Ref, Elements[5]),new("x", ValueType.Int),new("y", ValueType.Int),new("width", ValueType.Uint),new("height", ValueType.Uint),new("folded", ValueType.Bool),new("parent_frame", ValueType.Ref, Elements[15]),new("text", ValueType.String),new("color", ValueType.Color),new("start_point_name", ValueType.String)]),
        new(typeof(MemoElement), "memo", [new("id", ValueType.String),new("x", ValueType.Int),new("y", ValueType.Int),new("width", ValueType.Uint),new("height", ValueType.Uint),new("folded", ValueType.Bool),new("parent_frame", ValueType.Ref, Elements[15]),new("text", ValueType.String),new("color", ValueType.Color)]),
        new(typeof(FrameElement), "frame", [new("id", ValueType.String),new("x", ValueType.Int),new("y", ValueType.Int),new("width", ValueType.Uint),new("height", ValueType.Uint),new("folded", ValueType.Bool),new("parent_frame", ValueType.Ref, Elements[15]),new("text", ValueType.String),new("color", ValueType.Color)]),
        new(typeof(JointElement), "joint", [new("id", ValueType.String),new("edge", ValueType.Ref, Elements[17]),new("x", ValueType.Int),new("y", ValueType.Int),new("size", ValueType.Uint),new("parent_frame", ValueType.Ref, Elements[15]),new("text", ValueType.String),new("color", ValueType.Color)]),
        new(typeof(EdgeElement), "edge", [new("id", ValueType.String),new("from", ValueType.Ref, Elements[17]),new("port", ValueType.Uint),new("to", ValueType.Ref, Elements[17]),new("thickness", ValueType.Uint),new("text", ValueType.String),new("color", ValueType.Color)]),
        new(typeof(FormElement), "form", [new("id", ValueType.String),new("definition", ValueType.Ref, Elements[6]),new("folded", ValueType.Bool),new("text", ValueType.String),new("color", ValueType.Color)]),
        new(typeof(OptionElement), "option", [new("id", ValueType.String),new("definition", ValueType.Ref, Elements[7]),new("folded", ValueType.Bool),new("text", ValueType.String),new("color", ValueType.Color)]),
        new(typeof(ChoiceElement), "choice", [new("id", ValueType.String),new("definition", ValueType.Ref, Elements[8]),new("selected", ValueType.Ref, Elements[-1]),new("folded", ValueType.Bool),new("text", ValueType.String),new("color", ValueType.Color)]),
        new(typeof(TupleElement), "tuple", [new("id", ValueType.String),new("definition", ValueType.Ref, Elements[9]),new("folded", ValueType.Bool),new("text", ValueType.String),new("color", ValueType.Color)]),
        new(typeof(ListElement), "list", [new("id", ValueType.String),new("definition", ValueType.Ref, Elements[10]),new("folded", ValueType.Bool),new("text", ValueType.String),new("color", ValueType.Color)]),
        new(typeof(FieldElement), "field", [new("id", ValueType.String),new("definition", ValueType.Ref, Elements[11]),new("value", ValueType.String),new("text", ValueType.String),new("color", ValueType.Color)]),
        new(typeof(PortElement), "port", [new("id", ValueType.String),new("definition", ValueType.Ref, Elements[12]),new("index", ValueType.Uint),new("text", ValueType.String),new("color", ValueType.Color)]),
        new(typeof(LocalizedValueElement), "localized_value", [new("id", ValueType.String),new("definition", ValueType.Ref, Elements[2]),new("value", ValueType.String)])
    ];
    
    /* Public methods. */
    public static int FindElement(Type type)
    {
        for (int i = 0; i < Elements.Length; i++)
        {
            if (Elements[i].Type == type)
                return i;
        }
        throw new ArgumentException($"Unknown element '{type.Name}'.", nameof(type));
    }
    
    public static int FindElement(string elementName)
    {
        for (int i = 0; i < Elements.Length; i++)
        {
            if (Elements[i].Name == elementName)
                return i;
        }
        throw new ArgumentException($"Unknown element '{elementName}'.", nameof(elementName));
    }
    
    public static SElement Instantiate(int index) => (SElement)Activator.CreateInstance(Elements[index].Type);
}