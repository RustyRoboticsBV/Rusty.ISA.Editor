using System.IO;
using System.Text;
using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.Documentation;

/// <summary>
/// Generates Markdown documentation for registered codecs.
/// </summary>
internal static class DocsGen
{
    /// <summary>
    /// Generate Markdown documentation for all registered codecs.
    /// </summary>
    public static string Generate()
    {
        StringBuilder markdown = new();

        markdown.AppendLine("# Codec Reference");
        markdown.AppendLine();

        for (int i = 0; i < Codecs.Count; i++)
        {
            string name = Codecs.GetName(i);
            Codec codec = Codecs.Instantiate(i);

            markdown.AppendLine($"### `{FormatIndex(i)} ({name})`");

            markdown.AppendLine("| Property | Value(s) |");
            markdown.AppendLine("| --- | --- |");
            markdown.AppendLine($"| Index | {i} (`{FormatIndex(i)}`) |");
            markdown.AppendLine($"| Name | `{name}` |");
            markdown.AppendLine($"| Description | .................... |");

            // Attributes.
            markdown.Append("| Attributes | ");
            AppendAttributes(markdown, codec);
            markdown.AppendLine(" |");

            // Allowed children.
            markdown.Append("| Allowed children | ");
            for (int j = 0; j < codec.AllowedChildCount; j++)
            {
                if (j > 0)
                    markdown.Append(", ");
                markdown.Append($"`{codec.GetAllowedChildName(j)}`");
            }
            if (codec.AllowedChildCount == 0)
                markdown.Append("*None*");
            markdown.AppendLine(" |");

            markdown.AppendLine();
        }

        return markdown.ToString();
    }

    /// <summary>
    /// Generate Markdown documentation and write it to a file.
    /// </summary>
    public static void WriteToFile(string path)
    {
        File.WriteAllText(path, Generate(), Encoding.UTF8);
    }

    /// <summary>
    /// Append the allowed attributes as a nested HTML table.
    /// </summary>
    private static void AppendAttributes(StringBuilder markdown, Codec codec)
    {
        if (codec.AllowedAttributeCount == 0)
        {
            markdown.Append("*None*");
            return;
        }

        markdown.Append("<table>");
        markdown.Append("<thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead>");
        markdown.Append("<tbody>");

        for (int i = 0; i < codec.AllowedAttributeCount; i++)
        {
            string attribute = codec.GetAttributeNameFromIndex(i);
            string type = GetAttributeType(attribute);
            string description = "?????";

            markdown.Append($"<tr>");
            markdown.Append($"<td><code>{FormatIndex(i)}</code></td>");
            markdown.Append($"<td><code>{attribute}</code></td>");
            markdown.Append($"<td><code>{type}</code></td>");
            markdown.Append($"<td><code>{description}</code></td>");
            markdown.Append($"</tr>");
        }

        markdown.Append("</tbody>");
        markdown.Append("</table>");
    }

    /// <summary>
    /// Get the documented type of an attribute.
    /// </summary>
    private static string GetAttributeType(string attribute)
    {
        return attribute switch
        {
            Codecs.Localizable or
            Codecs.NoDefault or
            Codecs.Folded => "bool",

            Codecs.X or
            Codecs.Y => "int",

            Codecs.Width or
            Codecs.Height or
            Codecs.Index or
            Codecs.Port => "uint",

            Codecs.Color => "color",

            _ => "string"
        };
    }

    /// <summary>
    /// Format an index as hexadecimal with at least two digits.
    /// </summary>
    private static string FormatIndex(int index)
    {
        return $"0x{index:X2}";
    }
}