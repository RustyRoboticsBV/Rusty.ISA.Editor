using System;
using System.Text;

public class ElementClassGenerator
{
    public static string Generate(Schema schema, Element element)
    {
        string inherit = element.Inherits != "none" ? ClassName(element.Inherits) : "Codec";
        string group = CheckGroup(schema, element);
        string groupInterface = group != "none" ? $", ICodecGroup<{ClassName(group)}>" : "";
        StringBuilder sb = new();
        sb.AppendLine("// THIS IS A GENERATED SCRIPT FILE! IT SHOULD NOT BE EDITED MANUALLY!");
        sb.AppendLine();
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine();
        sb.AppendLine("namespace Rusty.ActionGraph.Serialization;");
        sb.AppendLine();
        sb.AppendLine($"internal sealed internal class {ClassName(element.Name)} : {inherit}{groupInterface}");
        sb.AppendLine("{");
        sb.AppendLine("    /* Public properties. */");
        foreach (Attribute attribute in element.Attributes)
        {
            sb.AppendLine($"    public {GetType(attribute.ValueType, attribute.RefType)} {CamelCase(attribute.Name)}");
            sb.AppendLine("    {");
            if (attribute.ValueType == ValueType.Bool)
            {
                sb.AppendLine($"        get => bool.TryParse(GetAttribute(\"{attribute.Name}\"), out bool result) ? result : false;");
                sb.AppendLine($"        set => SetAttribute(\"{attribute.Name}\", value.ToString());");
            }
            else if (attribute.ValueType == ValueType.Int)
            {
                sb.AppendLine($"        get => int.TryParse(GetAttribute(\"{attribute.Name}\"), out int result) ? result : 0;");
                sb.AppendLine($"        set => SetAttribute(\"{attribute.Name}\", value.ToString());");
            }
            else if (attribute.ValueType == ValueType.Uint)
            {
                sb.AppendLine($"        get => uint.TryParse(GetAttribute(\"{attribute.Name}\"), out uint result) ? result : 0;");
                sb.AppendLine($"        set => SetAttribute(\"{attribute.Name}\", value.ToString());");
            }
            else if (attribute.ValueType == ValueType.Color)
            {
                sb.AppendLine($"        get => Godot.Color.FromHtml(GetAttribute(\"{attribute.Name}\"));");
                sb.AppendLine($"        set => SetAttribute(\"{attribute.Name}\", value.ToHtml());");
            }
            else if (attribute.ValueType == ValueType.String || attribute.ValueType == ValueType.Ref)
            {
                sb.AppendLine($"        get => GetAttribute(\"{attribute.Name}\");");
                sb.AppendLine($"        set => SetAttribute(\"{attribute.Name}\", value);");
            }
            sb.AppendLine("    }");
        }
        sb.AppendLine("}");

        string str = sb.ToString();
        if (str.Contains("Godot."))
            str = str
                .Replace("Godot.", "")
                .Replace("using System.Collections.Generic;", "using Godot;\nusing System.Collections.Generic;");

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
        return sb.ToString();
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
