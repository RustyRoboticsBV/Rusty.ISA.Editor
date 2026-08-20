using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A node inspector.
/// </summary>
internal sealed partial class NodeInspector : VBoxContainer, IInspector<NodeInspector>
{
    /* Public properties. */
    public NodeDefinition Definition { get; private set; }
    public TupleWidget Node { get; private set; }
    public string StartName => StartOption.Enabled ? Start.Text : null;
    public UndoRedo UndoRedo
    {
        get => Node.UndoRedo;
        set => Node.UndoRedo = value;
    }

    /* Private properties. */
    private OptionWidget StartOption { get; set; }
    private LineField Start { get; set; }

    /* Public events. */
    public event Action StateChanged;

    /* Constructors. */
    private NodeInspector()
    {
        StartOption = new("Is Start Node", Start, false);
        StartOption.Name = "Start?";
        AddChild(StartOption);
    }

    public NodeInspector(NodeDefinition definition)
    {
        Definition = definition;

        // Create element inspectors.
        Control[] elements = new Control[definition.Inspectors.Length];
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i] = InspectorCreator.Create(definition.Inspectors[i]) as Control;
        }

        Node = new(definition.Title, elements);
        Node.StateChanged += () => StateChanged?.Invoke();
        AddChild(Node);
    }

    /* Public methods. */
    public NodeInspector DuplicateWidget()
    {
        NodeInspector copy = new();
        copy.Definition = Definition;
        copy.Node = Node.DuplicateWidget();
        copy.AddChild(copy.Node);
        copy.UndoRedo = UndoRedo;
        return copy;
    }
}