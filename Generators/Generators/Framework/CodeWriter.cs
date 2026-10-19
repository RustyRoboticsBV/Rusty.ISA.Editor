using System;
using System.Text;

/// <summary>
/// A simple code writer, used by generator classes.
/// </summary>
public sealed class CodeWriter
{
    /* Fields. */
    private readonly StringBuilder sb = new();
    public int indent;

    /* Public properties. */
    public int Indent
    {
        get => indent;
        set
        {
            if (value < 0)
                throw new InvalidOperationException("Indentation level cannot go below 0.");
            indent = value;
        }
    }

    /* Public methods. */
    public override string ToString() => sb.ToString();

    /// <summary>
    /// Write some text without starting a new line or respecting indentation.
    /// </summary>
    public void Append(char c, int count = 1) => sb.Append(c, count);

    /// <summary>
    /// Write some text without starting a new line or respecting indentation.
    /// </summary>
    public void Append(string text) => sb.Append(text);

    /// <summary>
    /// Add a line.
    /// </summary>
    public void Line(string text = "")
    {
        if (sb.Length == 0 && text.Length == 0)
        {
            Append('\n');
            return;
        }

        if (sb.Length > 0)
            Append('\n');
        Append(' ', Indent * 4);
        Append(text);
    }

    /// <summary>
    /// Start an indentated block.
    /// </summary>
    public void OpenBlock()
    {
        Line("{");
        Indent++;
    }

    /// <summary>
    /// End an indentated block.
    /// </summary>
    public void CloseBlock()
    {
        Indent--;
        Line("}");
    }

    /// <summary>
    /// Write a space.
    /// </summary>
    public void Space() => Append(' ');
}