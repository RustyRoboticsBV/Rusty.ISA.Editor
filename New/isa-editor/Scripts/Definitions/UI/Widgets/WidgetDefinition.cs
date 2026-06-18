using Godot;

namespace Rusty.ISA;

/// <summary>
/// An widget definition for an ISA instruction inspector.
/// </summary>
[GlobalClass]
public abstract partial class WidgetDefinition : Resource
{
    /* Public properties. */
    /// <summary>
    /// The display name of the widget.
    /// </summary>
    [Export] public string Title { get; set; } = "";
    /// <summary>
    /// The tooltip description of the widget.
    /// </summary>
    [Export(PropertyHint.MultilineText) ] public string Description { get; set; } = "";
}