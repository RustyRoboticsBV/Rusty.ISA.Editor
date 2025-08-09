namespace Rusty.ISA.Editor;

public class NodeData : Graphs.NodeData
{
    /* Public properties. */
    public InstructionSet Set { get; set; }
    public InstructionDefinition Definition { get; set; }
    public InstructionInstance Instance { get; set; }

    /* Public methods. */
    public override string ToString()
    {
        return Instance.ToString();
    }

    /// <summary>
    /// Set an argument on the instruction instance.
    /// </summary>
    public void SetArgument(string id, object value)
    {
        int index = Definition.GetParameterIndex(id);
        Instance.Arguments[index] = value.ToString();
    }
}