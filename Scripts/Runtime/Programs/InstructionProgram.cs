using Godot;

namespace Rusty.ActionGraph.Runtime;

/// <summary>
/// An program that can be executed by a InstructionProcess.
/// </summary>
[GlobalClass, Icon("InstructionProgram.svg")]
public partial class InstructionProgram : Resource
{
    /* Public properties. */
    [Export] public Metadata Metadata { get; private set; } = new();
    [Export] public InstructionSet InstructionSet { get; private set; } = new();
    [Export] public Instruction[] Instructions { get; private set; } = [];

    /* Constructors. */
    public InstructionProgram() : this(new(), new(), []) { }

    public InstructionProgram(Metadata metadata, InstructionSet instructionSet, Instruction[] instructions)
    {
        Metadata = metadata;
        InstructionSet = instructionSet;
        Instructions = instructions;
    }
}