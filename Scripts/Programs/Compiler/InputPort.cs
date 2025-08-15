namespace Rusty.ISA.Editor;

/// <summary>
/// A compiler node input port.
/// </summary>
public class InputPort : Graphs.InputPort
{
    /* Public properties. */
    public new OutputPort From => base.From as OutputPort;
    public new RootNode Node => base.Node as RootNode;
}