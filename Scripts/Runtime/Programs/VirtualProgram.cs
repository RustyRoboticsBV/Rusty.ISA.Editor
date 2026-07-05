using Godot;
using Godot.Collections;

namespace Rusty.ISA.Runtime;

/// <summary>
/// An ISA program.
/// </summary>
[GlobalClass]
public partial class VirtualProgram : Resource
{
    /* Public properties. */
    [Export] public Dictionary<string, string> Metadata { get; private set; } = new();
    [Export] public InstructionSet InstructionSet { get; private set; } = new();
    [Export] public Instruction[] Instructions { get; private set; } = [];

    /* Constructors. */
    public VirtualProgram() : this(new(), new(), []) { }

    public VirtualProgram(Dictionary<string, string> metadata, InstructionSet instructionSet, Instruction[] instructions)
    {
        Metadata = metadata;
        InstructionSet = instructionSet;
        Instructions = instructions;
    }
}