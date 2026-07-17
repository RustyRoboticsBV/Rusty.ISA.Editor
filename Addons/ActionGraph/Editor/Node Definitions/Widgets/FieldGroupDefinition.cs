using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// A grouping of form widgets, with a layout applied to them.
/// </summary>
[GlobalClass]
public sealed partial class FieldGroupDefinition : WidgetDefinition
{
    /* Public properties. */
    /// <summary>
    /// Whether or not the fields should be drawn with a foldout.
    /// </summary>
    [Export] bool Foldable { get; set; }
    /// <summary>
    /// The layout direction of the members.
    /// </summary>
    [Export] LayoutDirection LayoutDirection { get; set; }
    /// <summary>
    /// The margins of the field group.
    /// </summary>
    [Export] Margins Margins { get; set; }
    /// <summary>
    /// The members.
    /// </summary>
    [Export] WidgetDefinition[] Members { get; set; }
}