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
    public NodeCodec Codec { get; set; }
    /// <summary>
    /// The data of the outputs. Contains whether or not the default output exists or not, and the number of parameter outputs.
    /// </summary>
    public OutputCountResult OutputData { get; set; }

    /* Constructors. */
    public NodeUnit()
    {
        To = [];
    }

    public NodeUnit(FileCodec file, NodeCodec codec)
    {
        OutputData = new(file, codec);
        To = new Unit[OutputData.CountOutputs()];
        Codec = codec;
    }

    /* Public methods. */
    public override string ToString() => (Codec?.ToString(true) ?? "") + " " + OutputData.ToString();

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