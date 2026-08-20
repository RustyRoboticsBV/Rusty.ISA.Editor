namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A boolean field definition.
/// </summary>
public sealed partial class BoolDefinition : ParameterDefinition
{
    /* Public properties. */
    /// <summary>
    /// The default value.
    /// </summary>
    public bool DefaultValue { get; private set; }

    /* Constructors. */
    public BoolDefinition(string id, string title, string description, string type, bool defaultValue)
        : base(id, title, description, type)
    {
        DefaultValue = defaultValue;
    }
}