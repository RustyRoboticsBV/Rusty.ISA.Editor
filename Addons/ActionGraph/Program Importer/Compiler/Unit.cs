using Rusty.ActionGraph.Serialization;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Compilation;

public sealed class Unit
{
    public List<Unit> Input { get; } = new();
    public Unit[] Outputs { get; }
    public Codec Contents { get; set; }
    public List<Instruction> Compiled { get; set; } = new();
    public string Start { get; set; } = null;
    public string Label { get; set; } = null;

    /* Constructors. */
    public Unit(int outputs)
    {
        Outputs = new Unit[outputs];
    }

    public Unit(Codec contents, int outputs) : this(outputs)
    {
        Contents = contents;
    }

    /* Public methods. */
    /// <summary>
    /// Connect an output port
    /// </summary>
    public void ConnectTo(int port, Unit to)
    {
        Disconnect(port);
        Outputs[port] = to;
        to.Input.Add(this);
    }

    /// <summary>
    /// Disconnect an output port.
    /// </summary>
    public void Disconnect(int port)
    {
        if (Outputs[port] == null)
            return;

        Outputs[port].Input.Remove(this);
        Outputs[port] = null;
    }
}