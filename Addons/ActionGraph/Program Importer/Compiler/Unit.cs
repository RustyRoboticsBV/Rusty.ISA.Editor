using Rusty.ActionGraph.Serialization;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Compilation;

internal sealed class Unit
{
    public List<Unit> From { get; } = new();
    public Unit[] To { get; }

    public Codec Codec { get; set; }
    public OutputCountResult OutputData { get; set; }

    public List<Instruction> Compiled { get; set; } = new();
    public string Start { get; set; } = null;
    public string Label { get; set; } = null;

    /* Constructors. */
    public Unit()
    {
        To = [];
    }

    public Unit(OutputCountResult outputs, Codec codec)
    {
        To = new Unit[outputs.CountOutputs()];
        OutputData = outputs;
        Codec = codec;
    }

    /* Public methods. */
    public override string ToString() => (Codec?.GetTag() ?? "") + " " + OutputData.ToString();

    /// <summary>
    /// Connect an output port
    /// </summary>
    public void ConnectTo(int port, Unit to)
    {
        Disconnect(port);
        To[port] = to;
        to.From.Add(this);
    }

    /// <summary>
    /// Disconnect an output port.
    /// </summary>
    public void Disconnect(int port)
    {
        if (To[port] == null)
            return;

        To[port].From.Remove(this);
        To[port] = null;
    }
}