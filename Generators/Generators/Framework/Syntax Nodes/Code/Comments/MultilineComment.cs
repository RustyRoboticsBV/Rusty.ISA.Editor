/// <summary>
/// A multiline comment generator.
/// </summary>
public sealed class MultilineComment : Commentary, IClassMember
{
    /* Public methods. */
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