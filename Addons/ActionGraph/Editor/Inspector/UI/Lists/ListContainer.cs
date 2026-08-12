using Godot;

namespace Rusty.ActionGraph.Editor;

internal sealed partial class ListContainer : VBoxContainer
{
    /* Public properties. */
    public string TitleText
    {
        get => Foldable.Text;
        set => Foldable.Text = value;
    }
    public string AddButtonText
    {
        get => AddButton.Text;
        set => AddButton.Text = value;
    }
    public string TemplateTitle { get; set; }
    public Control ElementTemplate { get; set; }

    /* Private properties. */
    private FoldableHeader Foldable { get; set; }
    private VBoxContainer Elements { get; set; }
    private Button AddButton { get; set; }

    /* Constructors. */
    public ListContainer(string elementTitle, Control elementTemplate)
    {
        TemplateTitle = elementTitle;
        ElementTemplate = elementTemplate;

        MouseFilter = MouseFilterEnum.Pass;
        SizeFlagsHorizontal = SizeFlags.ExpandFill;

        // Foldable header.
        Foldable = new();
        Foldable.Name = "Foldable";
        Foldable.MouseFilter = MouseFilterEnum.Pass;
        Foldable.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Foldable.SizeFlagsVertical = SizeFlags.ShrinkCenter;
        Foldable.CustomMinimumSize = new(0, 31f);
        Foldable.Text = "Unnamed List";
        AddChild(Foldable);

        // Elements container.
        Elements = new();
        Elements.Name = "Elements";
        Elements.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(Elements);

        // Add button.
        AddButton = new();
        AddButton.Text = "Add Element";
        AddButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddButton.Pressed += () => AddElement();
        AddChild(AddButton);

        // Set up foldable toggle.
        Foldable.Pressed += () =>
        {
            Elements.Visible = Foldable.IsOpen;
            AddButton.Visible = Foldable.IsOpen;
        };
    }

    public ListContainer(string listTitle, string elementTitle, Control elementTemplate, string addButtonText) : this(elementTitle, elementTemplate)
    {
        TitleText = listTitle;
        AddButtonText = addButtonText;
    }

    /* Private methods. */
    private ListElement AddElement()
    {
        Control templateCopy = ElementTemplate is IField field
            ? field.DuplicateField() as Control
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
            ListElement newElement = AddElement();
            Elements.MoveChild(newElement, index);
        };
        element.PressedDuplicate += (element) =>
        {
            int index = GetElementIndex(element);
            ListElement newElement = element.DuplicateElement();
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