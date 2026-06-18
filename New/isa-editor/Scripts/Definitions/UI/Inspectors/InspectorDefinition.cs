using Godot;

namespace Rusty.ISA;

/// <summary>
/// An ISA instruction inspector definition.
/// </summary>
[GlobalClass]
public abstract partial class InspectorDefinition : Resource
{
    /* Public properties. */
    /// <summary>
    /// The display name of the instruction.
    /// </summary>
    [Export] public string Title { get; set; } = "";
    /// <summary>
    /// The tooltip description of the instruction.
    /// </summary>
    [Export] public string Description { get; set; } = "";
}