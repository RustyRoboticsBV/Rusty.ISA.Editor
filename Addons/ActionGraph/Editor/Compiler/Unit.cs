using Rusty.ActionGraph.Serialization;
using System;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Compilation;

public sealed class Unit
{
    public List<Unit> Input { get; } = new();
    public Unit[] Outputs { get; }
    public bool IsGoto { get; set; }
    public Codec Contents { get; set; }

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

    /// <summary>
    /// Insert a goto in the middle of an edge.
    /// </summary>
    public static Unit InsertGoto(Unit from, int port, Unit to)
    {
        Unit @goto = new Unit(1);
        @goto.IsGoto = true;
        from.ConnectTo(port, @goto);
        @goto.ConnectTo(0, to);
        return @goto;
    }
}