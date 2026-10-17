using System;
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
        string inherit = element.Inherits != "none" ? NameUtility.GetCodecName(element.Inherits) : "Codec";
        string group = CheckGroup(schema, element);
        string groupInterface = group != "none" ? $", ICodecGroup<{NameUtility.GetCodecName(group)}>" : "";

        if (element.Description != "")
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"/// {element.Description}");
            sb.AppendLine("/// </summary>");
        }

        sb.AppendLine($"internal sealed class {NameUtility.GetCodecName(element.Name)} : {inherit}{groupInterface}");
        sb.AppendLine("{");

        // Attribute properties.
        sb.AppendLine("    /* Public properties. */");

        sb.AppendLine($"    public override int AttributeCount => {element.Attributes.Count};");
        sb.AppendLine();

        foreach (Attribute attribute in element.Attributes)
        {
            if (attribute.Name == "id")
                continue;
            string id = NameUtility.CamelCase(attribute.Name);
            string type = GetType(attribute.ValueType, attribute.RefType);
            if (attribute.Description.Length > 0)
            {
                sb.AppendLine("    /// <summary>");
                sb.AppendLine("    /// " + attribute.Description);
                sb.AppendLine("    /// </summary>");
            }
            sb.AppendLine($"    public {type} {id}" + " { get; set; }");
        }

        // Attribute getter.
        sb.AppendLine();
        sb.AppendLine("    /* Public methods. */");
        sb.AppendLine("    public override object GetAttribute(int index) => index switch");
        sb.AppendLine("    {");
        for (int i = 0; i < element.Attributes.Count; i++)
        {
            var attribute = element.Attributes[i];
            sb.AppendLine($"        {i} => {NameUtility.CamelCase(attribute.Name)},");
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
            sb.AppendLine($"            case {i}: {NameUtility.CamelCase(attribute.Name)} = ({type})value; break;");
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
                sb.Append($"child is {NameUtility.GetCodecName(element.Children[i])}");
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

        // Find method.
        if (group != "none")
        {
            string returnType = NameUtility.GetCodecName(group);
            sb.AppendLine();
            sb.AppendLine($"    public {returnType} FindChild(string id)");
            sb.AppendLine("    {");
            sb.AppendLine("        for (int i = 0; i < ChildCount; i++)");
            sb.AppendLine("        {");
            sb.AppendLine($"            if (Children[i] is {returnType} typed && typed.ID == id)");
            sb.AppendLine("                return typed;");
            sb.AppendLine("        }");
            sb.AppendLine("        return null;");
            sb.AppendLine("    }");
        }

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
}
