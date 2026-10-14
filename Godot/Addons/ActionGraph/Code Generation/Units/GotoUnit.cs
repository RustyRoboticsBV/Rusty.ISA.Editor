namespace Rusty.ActionGraph.CodeGen;

/// <summary>
/// A compiler unit representing a goto.
/// </summary>
internal sealed class GotoUnit : Unit
{
    /* Public properties. */
    /// <summary>
    /// The output port. Can connect to one unit.
    /// </summary>
    public Unit To { get; private set; }

    /* Constructors */
    public GotoUnit() { }

    /* Public methods. */
    public override string ToString() => "GOTO: " + To;

    /// <summary>
    /// Connect the output port.
    /// </summary>
    public void ConnectTo(Unit to)
    {
        Disconnect();
        To = to;
        to.From.Add(this);
    }

    /// <summary>
    /// Disconnect the output port.
    /// </summary>
    public void Disconnect()
    {
        if (To == null)
            return;

        To.From.Remove(this);
        To = null;
    }
}