using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// A list inspector definition.
/// </summary>
[GlobalClass]
public sealed partial class ListDefinition : InspectorDefinition
{
    /* Public properties. */
    /// <summary>
    /// The "add element" button text.
    /// </summary>
    [Export] public string ButtonText { get; set; }
    /// <summary>
    /// The element type.
    /// </summary>
    [Export] public InspectorDefinition Type { get; set; }
}