using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// A form definition.
/// </summary>
[GlobalClass]
public sealed partial class FormDefinition : InspectorDefinition
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