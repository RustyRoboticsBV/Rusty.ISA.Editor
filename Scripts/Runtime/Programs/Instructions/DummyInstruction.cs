using Godot;

namespace Rusty.ISA;

/// <summary>
/// An ISA dummy instruction. It does nothing, but can be marked with a start point or label.
/// </summary>
[GlobalClass]
public sealed partial class DummyInstruction : Instruction
{
    /* Constructors. */
    public DummyInstruction() { }
}