namespace Rusty.ActionGraph.Compilation;

/// <summary>
/// A compiler unit representing a goto.
/// </summary>
internal sealed class GotoUnit : MonoUnit
{
    /* Constructors */
    public GotoUnit() { }

    /* Public methods. */
    public override string ToString() => "GOTO: " + To;
}