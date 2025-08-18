using Rusty.Graphs;

namespace Rusty.ISA.Editor;

/// <summary>
/// A compiler node output port.
/// </summary>
public class OutputPort : Graphs.OutputPort
{
    /* Public properties. */
    public new InputPort To => base.To as InputPort;
    public new RootNode Node => base.Node as RootNode;

    public bool IsDefaultOutput => OutputParameterNode == null;
    public INode OutputParameterNode { get; set; }
    public string OutputParameterID { get; set; } = "";
}