using System.IO;
using System.Text;
using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.Documentation;

/*/// <summary>
/// Generates Markdown documentation for all codecs.
/// </summary>
internal static class DocsGen
{
    /// <summary>
    /// Generate Markdown documentation for all codecs.
    /// </summary>
    public static string Generate()
    {
        StringBuilder markdown = new();

        markdown.AppendLine("# Codec Reference");
        markdown.AppendLine();

        for (int i = 0; i < Codecs.TagCount; i++)
        {
            string tag = Codecs.GetTag(i);
            Codec codec = Codecs.Instantiate(i);

            markdown.AppendLine($"## {FormatIndex(i)} - `{tag}`");
            markdown.AppendLine();

            AppendAttributes(markdown, codec);
            AppendChildren(markdown, codec);

            markdown.AppendLine("---");
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
    /// Append the allowed attributes of a codec.
    /// </summary>
    private static void AppendAttributes(
        StringBuilder markdown,
        Codec codec)
    {
        markdown.AppendLine("### Allowed Attributes");
        markdown.AppendLine();

        if (!codec.AllowsAttributes())
        {
            markdown.AppendLine("None.");
            markdown.AppendLine();
            return;
        }

        markdown.AppendLine("| Index | Attribute | Type |");
        markdown.AppendLine("| --- | --- | --- |");

        for (int i = 0; i < codec.AllowedAttributeCount; i++)
        {
            string attribute = codec.GetAttributeNameFromIndex(i);
            string type = GetAttributeType(attribute);

            markdown.AppendLine(
                $"| {FormatIndex(i)} | `{attribute}` | `{type}` |");
        }

        markdown.AppendLine();
    }

    /// <summary>
    /// Append the allowed children of a codec.
    /// </summary>
    private static void AppendChildren(
        StringBuilder markdown,
        Codec codec)
    {
        markdown.AppendLine("### Allowed Children");
        markdown.AppendLine();

        bool found = false;

        for (int i = 0; i < Codecs.TagCount; i++)
        {
            string tag = Codecs.GetTag(i);

            if (!codec.AllowsChild(tag))
                continue;

            markdown.AppendLine($"- `{tag}`");
            found = true;
        }

        if (!found)
            markdown.AppendLine("None.");

        markdown.AppendLine();
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
}*/

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

        for (int i = 0; i < Codecs.TagCount; i++)
        {
            string tag = Codecs.GetTag(i);
            Codec codec = Codecs.Instantiate(i);

            markdown.AppendLine($"## {FormatIndex(i)} - `{tag}`");
            markdown.AppendLine();

            markdown.AppendLine("| Codec hex | Codec name |");
            markdown.AppendLine("| --- | --- |");
            markdown.AppendLine($"| `{FormatIndex(i)}` | `{tag}` |");
            markdown.AppendLine("| Attributes |");
            markdown.AppendLine("| --- |");

            AppendAttributes(markdown, codec);

            markdown.AppendLine("| Children |");
            markdown.AppendLine("| --- |");

            AppendChildren(markdown, codec);

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
    private static void AppendAttributes(
        StringBuilder markdown,
        Codec codec)
    {
        markdown.AppendLine("| <table>");
        markdown.AppendLine("| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>");
        markdown.AppendLine("| <tbody>");

        for (int i = 0; i < codec.AllowedAttributeCount; i++)
        {
            string attribute = codec.GetAttributeNameFromIndex(i);
            string type = GetAttributeType(attribute);

            markdown.AppendLine(
                $"| <tr><td>{FormatIndex(i)}</td><td><code>{attribute}</code></td><td><code>{type}</code></td></tr>");
        }

        markdown.AppendLine("| </tbody>");
        markdown.AppendLine("| </table>");
    }

    /// <summary>
    /// Append the allowed children as a list.
    /// </summary>
    private static void AppendChildren(
        StringBuilder markdown,
        Codec codec)
    {
        bool found = false;

        markdown.AppendLine("| <ul>");

        for (int i = 0; i < Codecs.TagCount; i++)
        {
            string tag = Codecs.GetTag(i);

            if (!codec.AllowsChild(tag))
                continue;

            markdown.AppendLine($"| <li><code>{tag}</code></li>");
            found = true;
        }

        if (!found)
            markdown.AppendLine("| <li>None</li>");

        markdown.AppendLine("| </ul>");
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