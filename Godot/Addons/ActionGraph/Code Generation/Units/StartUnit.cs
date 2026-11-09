namespace Rusty.ActionGraph.CodeGen;

/// <summary>
/// A compiler unit representing a start point.
/// </summary>
internal sealed class StartUnit : Unit
{
    /* Public properties. */
    /// <summary>
    /// The output port. Can connect to one unit.
    /// </summary>
    public Unit To { get; private set; }
    /// <summary>
    /// The start point name.
    /// </summary>
    public string Name { get; private set; }

    /* Constructors */
    public StartUnit(string name) => Name = name;

    /* Public methods. */
    public override string ToString() => $"START {Name}: " + To;

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