using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.CodeGen;

/// <summary>
/// A compiler unit representing a graph node.
/// </summary>
internal sealed class NodeUnit : Unit
{
    /// <summary>
    /// The output ports. Each can connect to one unit.
    /// </summary>
    public Unit[] To { get; }

    /// <summary>
    /// The loaded node data from the program file.
    /// </summary>
    public NodeElement Element { get; set; }
    /// <summary>
    /// The data of the outputs. Contains whether or not the default output exists or not, and the number of parameter outputs.
    /// </summary>
    public OutputCountResult OutputData { get; set; }

    /* Constructors. */
    public NodeUnit()
    {
        To = [];
    }

    public NodeUnit(FileElement file, NodeElement codec)
    {
        OutputData = new(file, codec);
        To = new Unit[OutputData.CountOutputs()];
        Element = codec;
    }

    /* Public methods. */
    public override string ToString() => (ElementPrinter.ToString(Element, true) ?? "") + " " + OutputData.ToString();

    /// <summary>
    /// Connect an output port.
    /// </summary>
    public void ConnectTo(uint port, Unit to)
    {
        Disconnect(port);
        To[port] = to;
        to.From.Add(this);
    }

    /// <summary>
    /// Disconnect an output port.
    /// </summary>
    public void Disconnect(uint port)
    {
        if (To[port] == null)
            return;

        To[port].From.Remove(this);
        To[port] = null;
    }
}