using Godot;

namespace Rusty.ISA;

/// <summary>
/// A generic ISA instruction.
/// </summary>
[GlobalClass]
public sealed partial class GenericInstruction : Instruction
{
    /* Public properties. */
    [Export] public string Opcode { get; set; } = "";
    [Export] public string[] Arguments { get; set; } = [];

    /* Constructors. */
    public GenericInstruction() : this("", []) { }

    public GenericInstruction(string opcode, string[] arguments)
    {
        Opcode = opcode;
        Arguments = arguments;
    }
}