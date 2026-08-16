using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A tuple widget.
/// </summary>
internal sealed partial class TupleWidget : VBoxContainer, IWidget
{
    /* Public properties. */
    public Control[] Elements { get; private set; }
    public UndoRedo UndoRedo
    {
        get
        {
            foreach (Control element in Elements)
            {
                if (element is IWidget widget)
                    return widget.UndoRedo;
            }
            return null;
        }
        set
        {
            foreach (Control element in Elements)
            {
                if (element is IWidget widget)
                    widget.UndoRedo = value;
            }
        }
    }

    /* Constructors. */
    public TupleWidget(Control[] elements)
    {
        MouseFilter = MouseFilterEnum.Pass;
        SizeFlagsHorizontal = SizeFlags.ExpandFill;

        Elements = elements;
        foreach (Control element in Elements)
        {
            AddChild(element);
        }
    }

    /* Public methods. */
    IWidget IWidget.DuplicateWidget() => DuplicateWidget();

    public TupleWidget DuplicateWidget()
    {
        Control[] elements = new Control[Elements.Length];
        for (int i = 0; i < Elements.Length; i++)
        {
            Control element = Elements[i] is IWidget widget
                ? widget.DuplicateWidget() as Control
                : Elements[i].Duplicate() as Control;
            elements[i] = element;
        }

        TupleWidget copy = new(elements);
        copy.MouseFilter = MouseFilter;
        copy.SizeFlagsHorizontal = SizeFlagsHorizontal;
        copy.SizeFlagsVertical = SizeFlagsVertical;
        copy.UndoRedo = UndoRedo;

        return copy;
    }
}