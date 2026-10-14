namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A field or output parameter definition.
/// </summary>
public abstract partial class ParameterDefinition : InspectorDefinition
{
    /* Public properties. */
    /// <summary>
    /// The ID of the parameter.
    /// </summary>
    public string Type { get; private set; } = "";

    /* Constructors. */
    public ParameterDefinition(string id, string title, string description, string type)
        : base(id, title, description)
    {
        Type = type;
    }
}