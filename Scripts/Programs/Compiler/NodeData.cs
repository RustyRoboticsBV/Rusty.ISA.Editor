using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A compiler node data object.
/// </summary>
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
        Godot.GD.Print("FOOKIN " + id + " AND " + value + " MATE");
        Instance.Arguments[index] = str;
    }

    /// <summary>
    /// Get an argument from the instruction instance.
    /// </summary>
    public string GetArgument(string id)
    {
        int index = Definition.GetParameterIndex(id);
        return Instance.Arguments[index];
    }
}