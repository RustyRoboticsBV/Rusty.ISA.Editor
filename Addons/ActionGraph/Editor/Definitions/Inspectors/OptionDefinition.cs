namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An option inspector definition.
/// </summary>
public sealed partial class OptionDefinition : InspectorDefinition
{
    /* Public properties. */
    /// <summary>
    /// The optional inspector.
    /// </summary>
    public InspectorDefinition Optional { get; private set; }
    /// <summary>
    /// Whether the option is enabled by default.
    /// </summary>
    public bool Enabled { get; private set; }

    /* Constructors. */
    public OptionDefinition(string title, string description, InspectorDefinition optional, bool enabled)
        : base(title, description)
    {
        Optional = optional;
        Enabled = enabled;
    }
}