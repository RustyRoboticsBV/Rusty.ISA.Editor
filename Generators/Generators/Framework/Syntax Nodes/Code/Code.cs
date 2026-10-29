/// <summary>
/// A generic code generator.
/// </summary>
public sealed class Code : Statement
{
    public override bool SingleLine => !CodeString.Contains('\n');
    public string CodeString { get; set; }

    public Code(string code) => CodeString = code;

    public static implicit operator Code(string code) => new Code(code);

    public override bool Generate(CodeWriter w)
    {
        if (SingleLine)
            w.Append(CodeString);
        else
        {
            string[] lines = CodeString.Split('\n');

            if (lines.Length == 0)
                return false;

            w.Append(lines[0]);
            for (int i = 1; i < lines.Length; i++)
            {
                w.Line(lines[i]);
            }
        }
        return CodeString.Length > 0;
    }
}