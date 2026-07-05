using Godot;

namespace Rusty.ISA;

/// <summary>
/// An ISA instruction.
/// </summary>
[GlobalClass]
public abstract partial class Instruction : Resource
{
    /* Public properties. */
    [Export] public string Start { get; set; } = null;
    [Export] public string Label { get; set; } = null;
}