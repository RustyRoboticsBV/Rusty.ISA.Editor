using System;
using System.Text;

public sealed class CodeWriter
{
    /* Fields. */
    private readonly StringBuilder sb = new();
    private int indent;

    /* Public methods. */
    public override string ToString() => sb.ToString();

    /// <summary>
    /// Write some text without starting a new line or respecting indentation.
    /// </summary>
    public void Append(string text) => sb.Append(text);

    /// <summary>
    /// Add a line.
    /// </summary>
    public void Line(string text = "")
    {
        if (sb.Length > 0)
            sb.Append('\n');
        sb.Append(' ', indent * 4);
        sb.Append(text);
    }

    /// <summary>
    /// Add an indented line.
    /// </summary>
    public void IndentedLine(string text = "")
    {
        indent++;
        Line(text);
        indent--;
    }

    /// <summary>
    /// Start an indentated block.
    /// </summary>
    public void OpenBlock(string declaration = "")
    {
        if (declaration.Length > 0)
            Line(declaration);
        Line("{");
        indent++;
    }

    /// <summary>
    /// End an indentated block.
    /// </summary>
    public void CloseBlock(string suffix = "")
    {
        if (indent == 0)
            throw new InvalidOperationException();

        indent--;
        Line("}" + suffix);
    }

    /// <summary>
    /// Generate a multiline comment.
    /// </summary>
    public void MultilineComment(string text = "") => Line($"/* {text} */");

    /// <summary>
    /// Generate a summary.
    /// </summary>
    public void Summary(string text)
    {
        Line("/// <summary>");
        Line($"/// {text}");
        Line("/// </summary>");
    }

    /// <summary>
    /// Generate a property declaration.
    /// </summary>
    public void Property(string type, string name, string modifier = "", string access = "public")
    {
        string prefix = string.IsNullOrWhiteSpace(modifier) ? "" : modifier + " ";
        Line($"{access} {prefix}{type} {name} {{ get; set; }}");
    }
}