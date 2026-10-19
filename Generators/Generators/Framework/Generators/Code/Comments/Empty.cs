/// <summary>
/// A dummy generator that generates nothing.
/// </summary>
public sealed class Empty : Statement
{
    public override bool SingleLine => true;

    public override bool Generate(CodeWriter w) => false;
}