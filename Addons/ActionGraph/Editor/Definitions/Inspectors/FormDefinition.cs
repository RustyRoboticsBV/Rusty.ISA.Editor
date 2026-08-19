namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A form inspector definition.
/// </summary>
public sealed partial class FormDefinition : InspectorDefinition
{
    /* Public properties. */
    /// <summary>
    /// The form's instruction definition.
    /// </summary>
    public InstructionDefinition Definition { get; private set; }

    /* Constructors. */
    public FormDefinition(string title, string description, InstructionDefinition definition) : base(title, description)
    {
        Definition = definition;
    }
}