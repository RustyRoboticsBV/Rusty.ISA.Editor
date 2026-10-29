/// <summary>
/// A namespace generator.
/// </summary>
public sealed class Namespace : SyntaxNode
{
    /* Public properties. */
    public Id[] Names { get; set; } = [];
    public UserDefinedType[] Members { get; set; } = [];

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        w.Line("namespace ");
        for (int i = 0; i < Names.Length; i++)
        {
            if (i > 0)
                w.Append('.');
            Names[i].Generate(w);
        }

        if (Members.Length == 0)
            w.Append(';');
        else
            w.OpenBlock();

        for (int i = 0; i < Members.Length; i++)
        {
            if (i > 0)
            {
                w.Line();
                w.Line();
            }
            Members[i].Generate(w);
        }

        if (Members.Length > 0)
            w.CloseBlock();

        return true;
    }
}