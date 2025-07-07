using Godot;

namespace Rusty.ISA.Editor;

public partial class ListBorderContainer : FoldoutBorderContainer
{
    /* Public properties. */
    public IGuiElement Template { get; set; }
    public string AddButtonText
    {
        get => AddButton.ButtonText;
        set => AddButton.ButtonText = value;
    }

    /* Private properties. */
    private LabeledButton AddButton => GetFromFooter(0) as LabeledButton;

    /* Constructors. */
    public ListBorderContainer() : base()
    {
        // Never hide the border depending on if the container is empty.
        HideBorderIfEmpty = false;

        // Create "add element" button.
        LabeledButton addButton = new();
        addButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        addButton.ButtonText = "Add Element";
        addButton.Pressed += OnAddButtonPressed;
        Bottom.ShrinkRightEdge = true;
        AddToFooter(addButton);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        ListBorderContainer copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public override void CopyFrom(IGuiElement other)
    {
        base.CopyFrom(other);

        if (other is ListBorderContainer list)
        {
            Template = list.Template;
            AddButton.Pressed += OnAddButtonPressed;
        }
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        base._Process(delta);

        // Hide the border & add button if the foldout is closed.
        ForceHideBorder = !IsOpen;
        AddButton.Visible = IsOpen;
    }

    /* Private methods. */
    private void OnAddButtonPressed()
    {
        if (Template != null)
            AddToContents(Template.Copy());
        else
            GD.PrintErr($"We couldn't create an element because the list inspector '{Name}' did not have a template!");
    }
}