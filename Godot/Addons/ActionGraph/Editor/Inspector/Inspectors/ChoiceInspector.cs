using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A choice inspector.
/// </summary>
internal sealed partial class ChoiceInspector : MarginContainer, IInspector<ChoiceInspector>
{
    /* Public properties. */
    public ChoiceDefinition Definition { get; private set; }
    public ChoiceWidget Choice { get; private set; }
    public UndoRedo UndoRedo
    {
        get => Choice.UndoRedo;
        set => Choice.UndoRedo = value;
    }

    /* Public events. */
    public event Action StateChanged;

    /* Constructors. */
    private ChoiceInspector() { }

    public ChoiceInspector(ChoiceDefinition definition)
    {
        Definition = definition;

        // Create choice inspectors.
        Dictionary<string, Control> choices = new();
        foreach (var choice in definition.Choices)
        {
            choices[choice.ID] = InspectorCreator.Create(choice) as Control;
        }

        // Create choice container.
        Choice = new(definition.Title, choices, definition.Selected);
        Choice.StateChanged += () => StateChanged?.Invoke();
        AddChild(Choice);
    }

    /* Public methods. */
    public ChoiceInspector DuplicateWidget()
    {
        ChoiceInspector copy = new();
        copy.Definition = Definition;
        copy.Choice = Choice.DuplicateWidget();
        copy.AddChild(copy.Choice);
        copy.UndoRedo = UndoRedo;
        return copy;
    }
}