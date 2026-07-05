using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// A dummy instruction instance. It does nothing, but can be marked with a start point or label.
/// </summary>
[GlobalClass]
public sealed partial class DummyInstruction : Instruction
{
    /* Constructors. */
    public DummyInstruction() { }
}