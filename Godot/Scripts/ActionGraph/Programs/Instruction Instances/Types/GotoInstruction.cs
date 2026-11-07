using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// An goto instruction instance.
/// </summary>
[GlobalClass]
public sealed partial class GotoInstruction : Instruction
{
    /* Public properties. */
    /// <summary>
    /// The label of the instruction that wil get jumped to.
    /// </summary>
    [Export] public string TargetLabel { get; private set; } = "";

    /* Constructors. */
    public GotoInstruction() : this("") { }

    public GotoInstruction(string targetLabel)
    {
        TargetLabel = targetLabel;
        ResourceName = ToString();
    }

    /* Public methods. */
    public override string ToString()
    {
        return base.ToString() + $"GOTO(\"{TargetLabel}\")";
    }
}