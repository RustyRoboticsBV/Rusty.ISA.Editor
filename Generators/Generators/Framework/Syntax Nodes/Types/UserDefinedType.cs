/// <summary>
/// A base class for user-defined type generators.
/// </summary>
public abstract class UserDefinedType : SyntaxNode
{
    /* Public properties. */
    public Summary Summary { get; set; }
    public Access Access { get; set; } = new();
    public Modifier Modifier { get; set; } = new();
    public Id Name { get; set; }
}