/// <summary>
/// A namespace generator.
/// </summary>
public sealed class Using : SyntaxNode
{
    /* Public properties. */
    public Namespace Namespace { get; set; }

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        w.Line("using ");
        for (int i = 0; i < Namespace.Names.Length; i++)
        {
            if (i > 0)
                w.Append('.');
            Namespace.Names[i].Generate(w);
        }
        w.Append(';');
        return true;
    }
}