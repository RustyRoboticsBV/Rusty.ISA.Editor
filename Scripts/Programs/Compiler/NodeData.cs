namespace Rusty.ISA.Editor;

public class NodeData : Graphs.NodeData
{
    /* Public properties. */
    public InstructionSet Set { get; set; }
    public InstructionDefinition Definition { get; set; }
    public InstructionInstance Instance { get; set; }

    /* Constructors. */
    public NodeData(InstructionSet set, string opcode)
    {
        Set = set;
        Definition = set[opcode];
        Instance = new(Definition);
    }

    /* Public methods. */
    public override string ToString()
    {
        return Instance.ToString();
    }

    public override NodeData Copy()
    {
        return new NodeData(Set, Definition.Opcode);
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