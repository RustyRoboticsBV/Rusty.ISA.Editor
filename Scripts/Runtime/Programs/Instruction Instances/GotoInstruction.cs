using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// An goto instruction instance.
/// </summary>
[GlobalClass]
public sealed partial class GotoInstruction : Instruction
{
    /* Public properties. */
    [Export] public string TargetLabel { get; set; } = "";

    /* Constructors. */
    public GotoInstruction() : this("") { }

    public GotoInstruction(string targetLabel)
    {
        TargetLabel = targetLabel;
    }

    /* Public methods. */
    public override string ToString()
    {
        return base.ToString() + $"GOTO(\"{TargetLabel}\")";
    }
}