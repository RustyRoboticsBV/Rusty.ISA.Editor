using Rusty.ActionGraph.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

public class ElementClassGenerator
{
    public static string Generate(Schema schema, Element element)
    {
        StringBuilder sb = new();

        // Header.
        sb.AppendLine("// THIS IS A GENERATED SCRIPT FILE! IT SHOULD NOT BE EDITED MANUALLY!");
        sb.AppendLine();
        sb.AppendLine("using System;");
        sb.AppendLine();
        sb.AppendLine("namespace Rusty.ActionGraph.Serialization;");
        sb.AppendLine();

        // Start of class.
        string inherit = element.Inherits != "none" ? ClassName(element.Inherits) : "Codec";
        string group = CheckGroup(schema, element);
        string groupInterface = group != "none" ? $", ICodecGroup<{ClassName(group)}>" : "";

        sb.AppendLine($"internal sealed class {ClassName(element.Name)} : {inherit}{groupInterface}");
        sb.AppendLine("{");

        // Attribute properties.
        sb.AppendLine("    /* Public properties. */");

        sb.AppendLine($"    public override int AttributeCount => {element.Attributes.Count};");
        sb.AppendLine();

        foreach (Attribute attribute in element.Attributes)
        {
            string id = CamelCase(attribute.Name);
            string type = GetType(attribute.ValueType, attribute.RefType);
            if (attribute.Description.Length > 0)
            {
                sb.AppendLine("    /// <summary>");
                sb.AppendLine("    /// " + attribute.Description);
                sb.AppendLine("    /// </summary>");
            }
            sb.AppendLine($"    public {(id == "ID" ? "override " : "")}{type} {id}" + " { get; set; }");
        }

        // Attribute getter.
        sb.AppendLine();
        sb.AppendLine("    /* Public methods. */");
        sb.AppendLine("    public override object GetAttribute(int index) => index switch");
        sb.AppendLine("    {");
        for (int i = 0; i < element.Attributes.Count; i++)
        {
            var attribute = element.Attributes[i];
            sb.AppendLine($"        {i} => {CamelCase(attribute.Name)},");
        }
        sb.AppendLine($"        _ => throw new ArgumentOutOfRangeException(nameof(index))");
        sb.AppendLine("    };");
        sb.AppendLine();

        // Attribute setter.
        sb.AppendLine("    public override void SetAttribute(int index, object value)");
        sb.AppendLine("    {");
        sb.AppendLine("        switch (index)");
        sb.AppendLine("        {");
        for (int i = 0; i < element.Attributes.Count; i++)
        {
            var attribute = element.Attributes[i];
            string type = GetType(attribute.ValueType, attribute.RefType);
            sb.AppendLine($"            case {i}: {CamelCase(attribute.Name)} = ({type})value; break;");
        }
        sb.AppendLine($"            default: throw new ArgumentOutOfRangeException(nameof(index));");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine();

        // Add child.
        sb.AppendLine("    public override void AddChild(Codec child)");
        sb.AppendLine("    {");

        if (element.Children.Count > 0)
        {
            sb.Append("        if (!(");
            for (int i = 0; i < element.Children.Count; i++)
            {
                if (i > 0 && i % 2 == 0)
                    sb.Append("\n          ");
                if (i > 0)
                    sb.Append(" || ");
                sb.Append($"child is {ClassName(element.Children[i])}");
            }
            sb.AppendLine("))");
            sb.AppendLine("        {");
            sb.AppendLine("            throw new ArgumentException("
                + "\n                $\"Codec '{GetType().Name}' does not allow children of type '{child.GetType().Name}'.\","
                + "\n                nameof(child)"
                + "\n            );");
            sb.AppendLine("        }");
            sb.AppendLine("        Children.Add(child);");
        }
        else
        {
            sb.AppendLine("        throw new InvalidOperationException($\"Codec '{GetType().Name}' does not allow any children.\");");
        }
        sb.AppendLine("    }");

        // End of class.
        sb.AppendLine("}");

        // Godot using.
        string str = sb.ToString();
        if (str.Contains("Godot."))
            str = str
                .Replace("Godot.", "")
                .Replace("using System;", "using Godot;\nusing System;");

        return str;
    }

    private static string ClassName(string elementName)
    {
        return $"{CamelCase(elementName)}Codec";
    }

    private static string CamelCase(string snake_case)
    {
        StringBuilder sb = new();
        for (int i = 0; i < snake_case.Length; i++)
        {
            if (i == 0)
                sb.Append(snake_case[0].ToString().ToUpperInvariant());
            else if (i < snake_case.Length - 1 && snake_case[i] == '_')
            {
                sb.Append(snake_case[i + 1].ToString().ToUpperInvariant());
                i++;
            }
            else
                sb.Append(snake_case[i]);
        }
        string name = sb.ToString();
        if (name == "Id")
            return "ID";
        else
            return name;
    }

    private static string CheckGroup(Schema schema, Element element)
    {
        string inherits = "none";
        foreach (string child in element.Children)
        {
            Element found = schema.Get(child);
            if (inherits == "none")
                inherits = found.Inherits;
            else if (inherits != found.Inherits)
                return "none";
        }
        return inherits;
    }

    private static string GetType(ValueType valueType, string refType)
    {
        switch (valueType)
        {
            case ValueType.Bool:
                return "bool";
            case ValueType.Int:
                return "int";
            case ValueType.Uint:
                return "uint";
            case ValueType.String:
                return "string";
            case ValueType.Color:
                return "Godot.Color";
            case ValueType.Ref:
                return "string";
            default:
                throw new Exception();
        }
    }

    private string ICodecGroup = 
"""
/// <summary>
/// A codec that represents a group of child codecs.
/// </summary>
internal interface ICodecGroup<CodecT>
    where CodecT : Codec
    {
        /* Public properties. */
        public List<Codec> Children { get; }

        /* Public methods. */
        /// <summary>
        /// Try to find a child codec of some type with an ID. Returns null if the child codec cannot be found.
        /// </summary>
        public CodecT Find(string id)
        {
            foreach (Codec child in Children)
            {
                if (child is CodecT typed && typed.GetAttribute([[ID]]) == id)
                    return typed;
            }
            return null;
        }
    }
""";
}
