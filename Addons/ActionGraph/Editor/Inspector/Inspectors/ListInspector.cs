using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An tuple inspector.
/// </summary>
internal sealed partial class ListInspector : MarginContainer, IInspector<ListInspector>
{
    /* Public properties. */
    public ListDefinition Definition { get; private set; }
    public ListWidget List { get; private set; }
    public UndoRedo UndoRedo
    {
        get => List.UndoRedo;
        set => List.UndoRedo = value;
    }

    /* Public events. */
    public event Action StateChanged;

    /* Constructors. */
    private ListInspector() { }

    public ListInspector(ListDefinition definition)
    {
        Definition = definition;

        List = new(definition.Title, "Element", InspectorCreator.Create(definition.Type) as Control, definition.AddButtonText);
        List.StateChanged += () => StateChanged?.Invoke();
        AddChild(List);
    }

    /* Public methods. */
    public ListInspector DuplicateWidget()
    {
        ListInspector copy = new();
        copy.Definition = Definition;
        copy.List = List.DuplicateWidget();
        copy.AddChild(copy.List);
        copy.UndoRedo = UndoRedo;
        return copy;
    }
}