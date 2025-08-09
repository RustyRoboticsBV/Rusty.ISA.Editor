namespace Rusty.ISA.Editor;

public class RootNode : Graphs.RootNode
{
    /* Public properties. */
    public new NodeData Data
    {
        get => base.Data as NodeData;
        set => base.Data = value;
    }

    /* Public methods. */
    /// <summary>
    /// Set an argument on the data object.
    /// </summary>
    public void SetArgument(string id, object value)
    {
        Data.SetArgument(id, value);
    }
}