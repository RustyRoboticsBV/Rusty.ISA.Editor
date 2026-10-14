using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A form inspector.
/// </summary>
internal sealed partial class FormInspector : MarginContainer, IInspector<FormInspector>
{
    /* Public properties. */
    public FormDefinition Definition { get; private set; }
    public TupleWidget Form { get; private set; }
    public UndoRedo UndoRedo
    {
        get => Form.UndoRedo;
        set => Form.UndoRedo = value;
    }

    /* Public events. */
    public event Action StateChanged;

    /* Constructors. */
    private FormInspector() { }

    public FormInspector(FormDefinition definition)
    {
        Definition = definition;

        Form = new(definition.Title, []);
        Form.StateChanged += () => StateChanged?.Invoke();
        AddChild(Form);
    }

    /* Public methods. */
    public FormInspector DuplicateWidget()
    {
        FormInspector copy = new();
        copy.Definition = Definition;
        copy.Form = Form.DuplicateWidget();
        copy.AddChild(copy.Form);
        copy.UndoRedo = UndoRedo;
        return copy;
    }
}