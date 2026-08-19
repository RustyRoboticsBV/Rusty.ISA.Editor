using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An option inspector.
/// </summary>
internal sealed partial class OptionInspector : MarginContainer, IInspector<OptionInspector>
{
    /* Public properties. */
    public OptionDefinition Definition { get; private set; }
    public OptionWidget Option { get; private set; }
    public UndoRedo UndoRedo
    {
        get => Option.UndoRedo;
        set => Option.UndoRedo = value;
    }

    /* Public events. */
    public event Action StateChanged;

    /* Constructors. */
    private OptionInspector() { }

    public OptionInspector(OptionDefinition definition)
    {
        Definition = definition;

        Option = new(definition.Title, InspectorCreator.Create(definition.Optional) as Control, definition.Enabled);
        Option.StateChanged += () => StateChanged?.Invoke();
        AddChild(Option);
    }

    /* Public methods. */
    public OptionInspector DuplicateWidget()
    {
        OptionInspector copy = new();
        copy.Definition = Definition;
        copy.Option = Option.DuplicateWidget();
        copy.AddChild(copy.Option);
        copy.UndoRedo = UndoRedo;
        return copy;
    }
}