using Godot;

namespace Rusty.ISA;

/// <summary>
/// An ISA choice inspector definition.
/// </summary>
[GlobalClass]
public sealed partial class ChoiceInspectorDefinition : InspectorDefinition
{
    /* Public properties. */
    /// <summary>
    /// The inspector choices.
    /// </summary>
    [Export] public InspectorDefinition[] Choices { get; set; }
}