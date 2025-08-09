using Godot;

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

    public NodeData(InstructionSet set, InstructionInstance instance)
    {
        Set = set;
        Definition = set[instance.Opcode];
        Instance = new(instance);
    }

    /* Public methods. */
    public override string ToString()
    {
        return Instance.ToString();
    }

    public override NodeData Copy()
    {
        return new NodeData(Set, Instance);
    }

    /// <summary>
    /// Set an argument on the instruction instance.
    /// </summary>
    public void SetArgument(string id, object value)
    {
        int index = Definition.GetParameterIndex(id);
        string str = value is Color color ? '#' + color.ToHtml(color.A != 1f) : value.ToString();
        Instance.Arguments[index] = str;
    }
}