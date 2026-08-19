using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An editor node definition.
/// </summary>
public sealed partial class NodeDefinition : EditorDefinition
{
    /* Public properties. */
    /// <summary>
    /// The category of the node.
    /// </summary>
    public string Category { get; private set; } = "";
    /// <summary>
    /// The color of the node.
    /// </summary>
    public Color Color { get; private set; } = Colors.Blue;
    /// <summary>
    /// The inspectors of the node.
    /// </summary>
    public InspectorDefinition[] Inspectors { get; private set; } = [];

    /* Constructors. */
    public NodeDefinition(string title, string description, string category, Color color, InspectorDefinition[] inspectors)
        : base(title, description)
    {
        Category = category;
        Color = color;
        Inspectors = inspectors;
    }
}