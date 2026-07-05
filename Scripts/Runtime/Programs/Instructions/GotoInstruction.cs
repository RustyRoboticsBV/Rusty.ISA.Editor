using Godot;

namespace Rusty.ISA;

/// <summary>
/// An ISA goto instruction.
/// </summary>
[GlobalClass]
public sealed partial class GotoInstruction : Instruction
{
    /* Public properties. */
    public string TargetLabel { get; set; } = "";

    /* Constructors. */
    public GotoInstruction() : this("") { }

    public GotoInstruction(string targetLabel)
    {
        TargetLabel = targetLabel;
    }
}