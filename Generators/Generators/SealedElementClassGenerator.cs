public sealed class SealedElementClassGenerator : Generator
{
    public string Generate(Schema schema, Sealed @sealed)
    {
        // Generate script.
        bool needsGodot = false;
        CodeWriter properties = new();
        CodeWriter indexOf = new();
        CodeWriter nameOf = new();
        CodeWriter typeOf = new();
        CodeWriter get = new();
        CodeWriter set = new();
        properties.Indent = 1;
        indexOf.Indent = 2;
        nameOf.Indent = 2;
        typeOf.Indent = 2;
        get.Indent = 2;
        set.Indent = 3;
        for (int i = 0; i < @sealed.Attributes.Count; i++)
        {
            Attribute attribute = @sealed.Attributes[i];
            string name = attribute.Name;
            string property = Names.CamelCase(name);
            string type = Types.Get(attribute.ValueType);
            string @default = Types.GetDefault(attribute.ValueType);
            if (attribute.ValueType == ValueType.Color)
                needsGodot = true;

            properties.Line("/// <summary>");
            properties.Line($"/// {attribute.Description}");
            properties.Line("/// </summary>");
            if (schema.HasGlobalAttribute(attribute))
                properties.Line($"public override {type} {property} {{ get; set; }}");
            else
                properties.Line($"public {type} {property} {{ get; set; }}");
            if (@default != null)
                properties.Append($" = {@default};");

            indexOf.Line($"\"{name}\" => {i},");
            nameOf.Line($"{i} => \"{name}\",");
            typeOf.Line($"{i} => {property}.GetType(),");
            get.Line($"{i} => {property},");
            set.Line($"case {i}:");
            set.Indent++;
            set.Line($"{property} = ({type})value;");
            set.Line($"break;");
            set.Indent--;
        }

        string interfaces = "";
        if (@sealed.Inherits != null)
            interfaces = $", I{Names.ClassName(@sealed.Inherits.Name)}";

        return $@"{Notice}

{(needsGodot ? "using Godot;\n" : "")}using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A definition for an option inspector.
/// </summary>
internal sealed class {Names.ClassName(@sealed.Name)} : Element{interfaces}
{{
    /* Public constants. */
    public const int ELEMENT_INDEX = {@sealed.Index};
    public const string ELEMENT_NAME = ""{@sealed.Name}"";
    
    /* Public properties. */
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    
    public override int AttributeCount => {@sealed.Attributes.Count};
{properties}
    
    public override bool AllowsChildren => {(@sealed.Children.Count > 0 ? "true" : "false")};
    
    /* Public methods. */
    public override int IndexOfAttribute(string name) => name switch
    {{
{indexOf}
        _ => throw new ArgumentOutOfRangeException($""No attribute corresponds to the name '{{name}}'."")
    }};
    
    
    public override string NameOfAttribute(int index) => index switch
    {{
{nameOf}
        _ => throw new ArgumentOutOfRangeException($""No attribute corresponds to the index {{index}}."")
    }};
    
    public override Type TypeOfAttribute(int index) => index switch
    {{
{typeOf}
        _ => throw new ArgumentOutOfRangeException($""No attribute corresponds to the index {{index}}."")
    }};
    
    public override object GetAttribute(int index) => index switch
    {{
{get}
        _ => throw new ArgumentOutOfRangeException($""No attribute corresponds to the index {{index}}."")
    }};
    
    public override void SetAttribute(int index, object value)
    {{
        switch (index)
        {{
{set}
            default:
                throw new ArgumentOutOfRangeException($""No attribute corresponds to the index {{index}}."");
        }}
    }}
    
    public override void AddChild(Element child)
{AddChild(@sealed)}
}}";
    }

    private static CodeWriter AddChild(Sealed @sealed)
    {
        CodeWriter w = new();

        if (@sealed.Children.Count == 0)
        {
            w.Indent = 2;
            w.Line($" => throw new NotImplementedException($\"Elements of type '{{ElementName}}' cannot have child elements.\");");
            return w;
        }

        w.Indent = 1;
        w.Line("{");

        w.Indent++;
        if (!@sealed.MultipleChildren)
        {
            w.Line("if (Children.Count > 0)");
            w.Indent++;
            w.Line("throw new InvalidOperationException($\"Elements of type '{ElementName}' cannot have multiple child elements.\");");
            w.Indent--;
            w.Line();
        }

        w.Line("switch (child)");
        w.Line("{");

        w.Indent++;
        foreach (string child in @sealed.Children)
        {
            w.Line($"case {Names.ClassName(child)}:");
        }
        w.Indent++;
        w.Line($"Children.Add(child);");
        w.Line($"break;");
        w.Indent--;

        w.Line("default:");
        w.Indent++;
        w.Line($"throw new ArgumentOutOfRangeException($\"Elements of type '{{ElementName}}' cannot have children of type '{{child.ElementName}}'.\");");
        w.Indent--;
        w.Indent--;

        w.Line("}");
        w.Indent--;

        w.Line("}");

        return w;
    }
}