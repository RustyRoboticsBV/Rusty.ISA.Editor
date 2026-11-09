using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// A generic instruction instance.
/// </summary>
[GlobalClass]
public sealed partial class GenericInstruction : Instruction
{
    /* Public properties. */
    /// <summary>
    /// The opcode of the instruction.
    /// </summary>
    [Export] public string Opcode { get; private set; } = "";
    /// <summary>
    /// The arguments of the instruction.
    /// </summary>
    [Export] public InstructionArgument[] Arguments { get; private set; } = [];

    /* Constructors. */
    public GenericInstruction() : this("", (InstructionArgument[])null) { }

    public GenericInstruction(string opcode, InstructionArgument[] arguments)
    {
        Opcode = opcode;
        Arguments = arguments ?? [];
        ResourceName = ToString();
    }

    /* Public methods. */
    public override string ToString()
    {
        string args = "";
        foreach (var arg in Arguments)
        {
            if (args.Length > 0)
                args += ", ";
            args += $"\"{arg}\"";
        }
        return base.ToString() + $"_{Opcode}({args})";
    }
}