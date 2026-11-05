public class ElementPrinterGenerator : Generator
{
    public string Generate(Schema schema)
    {
        string fileSnake = schema.Sealeds[0].Name;
        string fileElement = Names.ElementName(fileSnake);

        return @$"{Notice}

using System.Text;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A serializer element.
/// </summary>
internal static class ElementPrinter
{{
    /* Public methods. */
    /// <summary>
    /// Return the string representation of an element.
    /// </summary>
    public static string ToString(Element element)
    {{
        StringBuilder sb = new();
        AppendToString(element, sb, """", true, true, true);
        return sb.ToString();
    }}

    /// <summary>
    /// Return the string representation of an element.
    /// </summary>
    public static string ToString(Element element, bool omitChildren)
    {{
        if (!omitChildren)
            return ToString(element);

        StringBuilder sb = new();
        AppendToString(element, sb, """", true, true, false);
        return sb.ToString();
    }}

    /* Private methods. */
    /// <summary>
    /// Helper function for ToString.
    /// </summary>
    private static void AppendToString(Element element, StringBuilder sb, string prefix, bool last, bool root, bool recurse)
    {{
        if (!root)
        {{
            sb.Append(prefix);
            sb.Append(last ? ""\u2514\u2500"" : ""\u251C\u2500"");
        }}

        sb.Append(element.ElementName);

        if (element.AttributeCount > 0)
        {{
            sb.Append("" {{"");

            bool first = true;
            for (int i = 0; i < element.AttributeCount; i++)
            {{
                if (!first)
                    sb.Append("", "");

                sb.Append(element.NameOfAttribute(i));
                sb.Append(""=\"""");
                sb.Append(element.GetAttribute(i));
                sb.Append('""');

                first = false;
            }}

            sb.Append('}}');
        }}

        if (recurse)
        {{
            sb.AppendLine();

            string childPrefix = root ? """" : prefix + (last ? ""  "" : ""\u2502 "");

            for (int i = 0; i < element.ChildCount; i++)
            {{
                AppendToString(element.GetChild(i), sb, childPrefix, i == element.ChildCount - 1, false, recurse);
            }}
        }}
        else if (element.ChildCount > 0)
            sb.Append(""..."");
    }}
}}";
    }
}