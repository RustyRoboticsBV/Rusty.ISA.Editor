using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An tuple inspector.
/// </summary>
internal sealed partial class TupleInspector : MarginContainer, IInspector<TupleInspector>
{
    /* Public properties. */
    public TupleDefinition Definition { get; private set; }
    public TupleWidget Tuple { get; private set; }
    public UndoRedo UndoRedo
    {
        get => Tuple.UndoRedo;
        set => Tuple.UndoRedo = value;
    }

    /* Public events. */
    public event Action StateChanged;

    /* Constructors. */
    private TupleInspector() { }

    public TupleInspector(TupleDefinition definition)
    {
        Definition = definition;

        // Create choice inspectors.
        Control[] elements = new Control[definition.Elements.Length];
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i] = InspectorCreator.Create(definition.Elements[i]) as Control;
        }

        Tuple = new(definition.Title, elements);
        Tuple.StateChanged += () => StateChanged?.Invoke();
        AddChild(Tuple);
    }

    /* Public methods. */
    public TupleInspector DuplicateWidget()
    {
        TupleInspector copy = new();
        copy.Definition = Definition;
        copy.Tuple = Tuple.DuplicateWidget();
        copy.AddChild(copy.Tuple);
        copy.UndoRedo = UndoRedo;
        return copy;
    }
}