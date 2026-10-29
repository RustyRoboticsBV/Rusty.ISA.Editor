/// <summary>
/// A function call statement generator.
/// </summary>
public sealed class New : Call
{
    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        if (Name == null)
        {
            w.Append("new()");
            return true;
        }

        w.Append("new ");
        base.Generate(w);

        return true;
    }
}