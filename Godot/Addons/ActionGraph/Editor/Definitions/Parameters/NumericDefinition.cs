namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A numeric field definition.
/// </summary>
public sealed partial class NumericDefinition : ParameterDefinition
{
    /* Public properties. */
    /// <summary>
    /// The default value.
    /// </summary>
    public double DefaultValue { get; private set; } = double.MinValue;
    /// <summary>
    /// The minimum value.
    /// </summary>
    public double MinValue { get; private set; } = double.MinValue;
    /// <summary>
    /// The maximum value.
    /// </summary>
    public double MaxValue { get; private set; } = double.MaxValue;
    /// <summary>
    /// The step value.
    /// </summary>
    public double Step { get; private set; } = 0.001f;
    /// <summary>
    /// Whether or not this field should be drawn with a slider.
    /// </summary>
    public bool Slider { get; private set; }

    /* Constructors. */
    public NumericDefinition(string id, string title, string description, string type,
        double defaultValue, double minValue, double maxValue, double step, bool slider) : base(id, title, description, type)
    {
        DefaultValue = defaultValue;
        MinValue = minValue;
        MaxValue = maxValue;
        Step = step;
        Slider = slider;
    }
}