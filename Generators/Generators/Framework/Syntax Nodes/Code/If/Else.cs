/// <summary>
/// An else statement generator.
/// </summary>
public sealed class Else : ConditionBlock
{
    /* Public properties. */
    public override bool SingleLine => false;

    /* Protected methods. */
    protected override void GenerateHeader(CodeWriter w)
    {
        w.Append("else");
    }
}