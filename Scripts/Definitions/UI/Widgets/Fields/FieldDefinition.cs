using Godot;

namespace Rusty.ISA;

/// <summary>
/// A parameter field definition for an ISA instruction inspector.
/// </summary>
[GlobalClass]
public abstract partial class FieldDefinition : WidgetDefinition
{
    /* Public properties. */
    /// <summary>
    /// The display name of the widget.
    /// </summary>
    [Export] public string ParameterID { get; private set; } = "";
}