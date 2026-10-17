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
        sb.AppendLine($"internal abstract class {NameUtility.GetCodecName(name)} : Codec");
        sb.AppendLine("{");
        sb.AppendLine("}");

        return sb.ToString();
    }
}
