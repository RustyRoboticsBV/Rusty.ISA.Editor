using System.Text;

public class SchemaClassGenerator : ClassGenerator
{
    public static string Generate(Schema schema)
    {
        StringBuilder sb = new();

        // Header.
        sb.AppendLine(Notice);
        sb.AppendLine();
        sb.AppendLine(Namespace);
        sb.AppendLine();

        // Start of class.
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// The serializer codec schema.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine($"internal static class CodecSchema");
        sb.AppendLine("{");

        // Constants.
        sb.AppendLine("    /* Public properties. */");
        sb.AppendLine($"    public const int CodecCount = {schema.Elements.Count};");
        sb.AppendLine();

        // Nested classes.
        sb.AppendLine("    /* Public types. */");
        foreach (Element element in schema.Elements)
        {
            if (element.Name == "id")
                continue;
            string id = NameUtility.CamelCase(element.Name);
            if (element.Description.Length > 0)
            {
                sb.AppendLine("    /// <summary>");
                sb.AppendLine("    /// " + element.Description);
                sb.AppendLine("    /// </summary>");
            }
            sb.AppendLine($"    public static class {id}");
            sb.AppendLine("    {");
            sb.AppendLine($"        public const int AttributeCount = {element.Attributes.Count};");
            for (int i = 0; i < element.Attributes.Count; i++)
            {
                Attribute attribute = element.Attributes[i];
                sb.AppendLine($"        public const int {NameUtility.CamelCase(attribute.Name)} = {i};");
            }
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        // Methods.
        sb.AppendLine("    /* Public methods. */");

        // End of class.
        sb.AppendLine("}");

        return sb.ToString();
    }
}