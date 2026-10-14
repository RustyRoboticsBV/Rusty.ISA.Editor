using System.Text;

public class AbstractClassGenerator
{
    public static string Generate(Schema schema, string name)
    {
        StringBuilder sb = new();

        // Header.
        sb.AppendLine("// THIS IS A GENERATED SCRIPT FILE! IT SHOULD NOT BE EDITED MANUALLY!");
        sb.AppendLine();
        sb.AppendLine("namespace Rusty.ActionGraph.Serialization;");
        sb.AppendLine();

        // Class.
        sb.AppendLine($"internal abstract class {ClassName(name)} : Codec");
        sb.AppendLine("{");
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
        string name = sb.ToString();
        if (name == "Id")
            return "ID";
        else
            return name;
    }
}
