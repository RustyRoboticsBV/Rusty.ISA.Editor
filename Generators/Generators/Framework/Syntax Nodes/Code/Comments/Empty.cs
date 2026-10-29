/// <summary>
/// A dummy generator that generates nothing.
/// </summary>
public sealed class Empty : Statement, IClassMember
{
    /* Public properties. */
    public override bool SingleLine => true;

    /* Public methods. */
    public override bool Generate(CodeWriter w) => false;
}