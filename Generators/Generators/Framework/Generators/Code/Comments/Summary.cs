/// <summary>
/// A summary comment generator.
/// </summary>
public sealed class Summary : Statement
{
    public override bool SingleLine => !Text.Contains('\n');
    public string Text { get; set; }

    public Summary(string text) => Text = text;

    public static implicit operator Summary(string v) => new(v);

    public override bool Generate(CodeWriter w)
    {
        string[] lines = Text.Split('\n');

        if (lines.Length == 0)
            return false;

        w.Append("/// <summary>");
        for (int i = 0; i < lines.Length; i++)
        {
            w.Line("/// ");
            w.Append(lines[i]);
        }
        w.Line("/// </summary>");

        return true;
    }
}