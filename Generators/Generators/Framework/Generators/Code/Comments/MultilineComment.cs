/// <summary>
/// A multiline comment generator.
/// </summary>
public sealed class MultilineComment : Statement
{
    public override bool SingleLine => !Text.Contains('\n');
    public string Text { get; set; }

    public MultilineComment(string text) => Text = text;

    public static implicit operator MultilineComment(string v) => new(v);

    public override bool Generate(CodeWriter w)
    {
        string[] lines = Text.Split('\n');

        if (lines.Length == 0)
            return false;

        w.Append("/* ");
        w.Append(lines[0]);
        for (int i = 1; i < lines.Length; i++)
        {
            w.Line(" * ");
            w.Append(lines[i]);
        }
        w.Append(" */");

        return true;
    }
}