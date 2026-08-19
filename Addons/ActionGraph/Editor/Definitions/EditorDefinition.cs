using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An ActionGraph editor definition resource.
/// </summary>
public abstract partial class EditorDefinition : Resource
{
    /* Public properties. */
    /// <summary>
    /// The ID of the resource.
    /// </summary>
    public string ID { get; private set; } = "";
    /// <summary>
    /// The title of the resource.
    /// </summary>
    public string Title { get; private set; } = "";
    /// <summary>
    /// The tooltip of the resource.
    /// </summary>
    public string Description { get; private set; } = "";

    /* Constructors. */
    public EditorDefinition(string title, string definition)
    {
        Title = title;
        Description = definition;
    }
}