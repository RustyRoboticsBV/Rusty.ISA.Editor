using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// A choice inspector definition.
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