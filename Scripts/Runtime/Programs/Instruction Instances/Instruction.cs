using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// An instruction instance.
/// </summary>
[GlobalClass]
public abstract partial class Instruction : Resource
{
    /* Public properties. */
    [Export] public string Start { get; set; } = null;
    [Export] public string Label { get; set; } = null;
}