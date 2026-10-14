using System.Text;

public class ElementClassGenerator
{
    public static string Generate(Schema schema, Element element)
    {
        string inherit = element.Inherits != "none" ? ClassName(element.Inherits) : "Codec";
        string group = CheckGroup(schema, element);
        string groupInterface = group != "none" ? $", ICodecGroup<{ClassName(group)}>" : "";
        StringBuilder sb = new();
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine();
        sb.AppendLine("namespace Rusty.ActionGraph.Serialization;");
        sb.AppendLine();
        sb.AppendLine($"internal sealed internal class {ClassName(element.Name)} : {inherit}{groupInterface}");
        sb.AppendLine("{");
        sb.AppendLine("    /* Public properties. */");
        sb.AppendLine($"    public override string Name => \"{element.Name}\";");
        sb.AppendLine("}");
        return sb.ToString();
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
}
