using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A list widget.
/// </summary>
internal sealed partial class ListWidget : VBoxContainer, IWidget
{
    /* Public properties. */
    public string AddButtonText
    {
        get => AddButton.Text;
        set => AddButton.Text = value;
    }
    public string TemplateTitle { get; set; }
    public Control ElementTemplate { get; set; }
    public UndoRedo UndoRedo
    {
        get => _UndoRedo;
        set
        {
            _UndoRedo = value;
            foreach (Node child in Elements.GetChildren())
            {
                if (child is IWidget widget)
                    widget.UndoRedo = value;
            }
        }
    }

    /* Private properties. */
    private VBoxContainer Elements { get; set; }
    private Button AddButton { get; set; }
    private UndoRedo _UndoRedo { get; set; }

    /* Constructors. */
    public ListWidget(string elementTitle, Control elementTemplate, string addButtonText)
    {
        TemplateTitle = elementTitle;
        ElementTemplate = elementTemplate;

        MouseFilter = MouseFilterEnum.Pass;
        SizeFlagsHorizontal = SizeFlags.ExpandFill;

        // Elements container.
        Elements = new();
        Elements.Name = "Elements";
        Elements.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(Elements);

        // Add button.
        AddButton = new();
        AddButton.Text = "Add";
        AddButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddButton.Pressed += () => CreateElement();
        AddChild(AddButton);
    }

    /* Public methods. */
    IWidget IWidget.DuplicateWidget() => DuplicateWidget();

    public ListWidget DuplicateWidget()
    {
        ListWidget copy = new(TemplateTitle, ElementTemplate, AddButtonText);
        copy.SizeFlagsHorizontal = SizeFlagsHorizontal;
        copy.SizeFlagsVertical = SizeFlagsVertical;
        foreach (Control child in Elements.GetChildren())
        {
            if (child is ListElement element)
            {
                copy.AddElement(element.DuplicateWidget());
            }
        }
        copy.UndoRedo = UndoRedo;
        return copy;
    }

    /* Private methods. */
    private ListElement CreateElement()
    {
        Control templateCopy = ElementTemplate is IWidget widget
            ? widget.DuplicateWidget() as Control
            : ElementTemplate.Duplicate() as Control;
        ListElement element = new(TemplateTitle, templateCopy);
        AddElement(element);
        return element;
    }

    private void AddElement(ListElement element)
    {
        element.PressedInsert += (element) =>
        {
            int index = GetElementIndex(element);
            ListElement newElement = CreateElement();
            Elements.MoveChild(newElement, index);
        };
        element.PressedDuplicate += (element) =>
        {
            int index = GetElementIndex(element);
            ListElement newElement = element.DuplicateWidget();
            AddElement(newElement);
            Elements.MoveChild(newElement, index);
        };
        element.PressedDelete += Elements.RemoveChild;
        element.MovedUp += (element) =>
        {
            int index = GetElementIndex(element);
            if (index > 0)
                Elements.MoveChild(element, index - 1);
        };
        element.MovedDown += (element) =>
        {
            int index = GetElementIndex(element);
            if (index < Elements.GetChildCount() - 1)
                Elements.MoveChild(element, index + 1);
        };
        element.UndoRedo = UndoRedo;
        Elements.AddChild(element);
    }

    private int GetElementIndex(ListElement element)
    {
        for (int i = 0; i < Elements.GetChildCount(); i++)
        {
            if (Elements.GetChild(i) == element)
                return i;
        }
        return -1;
    }
}