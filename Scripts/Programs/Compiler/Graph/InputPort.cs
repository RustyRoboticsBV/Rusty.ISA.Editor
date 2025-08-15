namespace Rusty.ISA.Editor;

/// <summary>
/// A compiler node input port.
/// </summary>
public class InputPort : Graphs.InputPort
{
    /* Public properties. */
    public override OutputPort From => base.From as OutputPort;
    public override RootNode Node => base.Node as RootNode;
}