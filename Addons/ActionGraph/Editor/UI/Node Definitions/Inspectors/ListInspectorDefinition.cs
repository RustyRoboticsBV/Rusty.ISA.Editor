using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// A list inspector definition.
/// </summary>
[GlobalClass]
public sealed partial class ListInspectorDefinition : InspectorDefinition
{
    /* Public properties. */
    /// <summary>
    /// The element type.
    /// </summary>
    [Export] public InspectorDefinition Type { get; set; }
    /// <summary>
    /// The "add element" button text.
    /// </summary>
    [Export] public string ButtonText { get; set; }
}