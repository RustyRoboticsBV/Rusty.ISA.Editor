namespace Rusty.ISA.Editor;

/// <summary>
/// A compiler root node.
/// </summary>
public class RootNode : Graphs.RootNode
{
    /* Public properties. */
    public new NodeData Data
    {
        get => base.Data as NodeData;
        set => base.Data = value;
    }

    /* Public methods. */
    public override SubNode CreateChild()
    {
        SubNode child = new();
        AddChild(child);
        return child;
    }

    public override SubNode ToChild()
    {
        SubNode child = new();
        child.Data = Data.Copy();
        return child;
    }

    /// <summary>
    /// Set an argument on the data object.
    /// </summary>
    public void SetArgument(string id, object value)
    {
        Data?.SetArgument(id, value);
    }

    /// <summary>
    /// Get the output data of this node.
    /// </summary>
    public OutputArguments GetOutputArguments()
    {
        return new(this);
    }
}