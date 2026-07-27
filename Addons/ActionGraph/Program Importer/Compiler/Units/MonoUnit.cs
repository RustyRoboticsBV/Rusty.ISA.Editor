namespace Rusty.ActionGraph.Compilation;

/// <summary>
/// A compiler unit with one output port.
/// </summary>
internal abstract class MonoUnit : Unit
{
    /* Public properties. */
    /// <summary>
    /// The output port. Can connect to one unit.
    /// </summary>
    public Unit To { get; private set; }

    /* Public methods. */
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
