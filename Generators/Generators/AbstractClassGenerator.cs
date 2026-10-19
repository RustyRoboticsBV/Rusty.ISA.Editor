using System.Text;

public class AbstractClassGenerator : ClassGenerator
{
    public static string Generate(Schema schema, Abstract @abstract)
    {
        CodeWriter w = new();

        /*// Header.
        w.Line(Notice);
        w.Line();
        w.Line(Namespace);
        w.Line();

        // Class.
        w.OpenBlock($"internal abstract class {NameUtility.GetCodecName(@abstract.Name)} : Codec");

        // Properties.
        w.MultilineComment("Public properties.");
        foreach (var attribute in @abstract.Attributes)
        {
            if (!schema.HasSharedAttribute(attribute))
                w.Property(TypeUtility.GetType(attribute.ValueType), NameUtility.CamelCase(attribute.Name), "abstract");
        }

        // End of class.
        w.CloseBlock();*/

        return w.ToString();
    }
}
