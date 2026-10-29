/// <summary>
/// A summary comment generator.
/// </summary>
public sealed class Summary : Commentary
{
    /* Public properties. */
    public override bool SingleLine => Text.Length > 0;

    /* Public methods. */
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