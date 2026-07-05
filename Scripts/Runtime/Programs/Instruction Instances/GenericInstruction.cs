using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// A generic instruction instance.
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