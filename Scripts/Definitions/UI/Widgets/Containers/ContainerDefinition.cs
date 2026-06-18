using Godot;

namespace Rusty.ISA;

/// <summary>
/// A vbox container definition for an ISA instruction inspector.
/// </summary>
[GlobalClass]
public abstract partial class ContainerDefinition : WidgetDefinition
{
    /* Public properties. */
    /// <summary>
    /// The element widgets.
    /// </summary>
    [Export] public WidgetDefinition[] Widgets { get; set; } = [];

    /// <summary>
    /// The indentation applied to the element widgets.
    /// </summary>
    [Export] public int Indentation { get; set; } = 20;
}