using Godot;

namespace Rusty.ISA;

/// <summary>
/// An ISA instruction inspector definition.
/// </summary>
[GlobalClass]
public sealed partial class InstructionInspectorDefinition : InspectorDefinition
{
    /* Public properties. */
    /// <summary>
    /// The runtime instruction definition.
    /// </summary>
    [Export] public InstructionDefinition Instruction { get; set; }
    /// <summary>
    /// The widgets of the inspector.
    /// </summary>
    [Export] public WidgetDefinition[] Widgets { get; set; } = [];
}