using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// A numeric parameter field definition for an instruction inspector.
/// </summary>
[GlobalClass]
public sealed partial class NumericFieldDefinition : FieldDefinition
{
    /* Public properties. */
    /// <summary>
    /// The default value of the field.
    /// </summary>
    [Export] public double DefaultValue { get; set; }

    /// <summary>
    /// The field's step-size.
    /// </summary>
    [Export] public double Step { get; set; } = 1.0;
    /// <summary>
    /// The minimum value of the field.
    /// </summary>
    [Export] public double MinValue { get; set; } = double.MinValue;
    /// <summary>
    /// The maximum value of the field.
    /// </summary>
    [Export] public double MaxValue { get; set; } = double.MaxValue;
}