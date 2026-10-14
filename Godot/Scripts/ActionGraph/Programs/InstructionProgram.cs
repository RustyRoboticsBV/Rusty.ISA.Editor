using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// An program that can be executed by a InstructionProcess.
/// </summary>
[GlobalClass, Icon("InstructionProgram.svg")]
public partial class InstructionProgram : Resource
{
    /* Public properties. */
    [Export] public Metadata Metadata { get; private set; } = new();
    [Export] public InstructionSet InstructionSet { get; private set; } = new();
    [Export] public InstructionList Instructions { get; private set; } = new();

    /* Constructors. */
    public InstructionProgram() : this(new(), new(), new()) { }

    public InstructionProgram(Metadata metadata, InstructionSet instructionSet, InstructionList instructions)
    {
        Metadata = metadata;
        InstructionSet = instructionSet;
        Instructions = instructions;
    }

    /* Public methods. */
    public override string ToString() => Instructions.ToString();
}