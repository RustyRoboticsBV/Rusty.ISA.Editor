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
    private LabeledButton FoldButton => GetFromHeader(1) as LabeledButton;
    private LabeledButton ExpandButton => GetFromHeader(2) as LabeledButton;

    /* Constructors. */
    public ListBorderContainer() : base()
    {
        // Never hide the border depending on if the container is empty.
        HideBorderIfEmpty = false;

        // Create "fold" and "unfold" buttons.
        LabeledButton foldButton = new();
        foldButton.ButtonText = "Fold All";
        foldButton.Pressed += OnFoldButtonPressed;
        AddToHeader(foldButton);

        LabeledButton expandButton = new();
        expandButton.ButtonText = "Expand All";
        expandButton.Pressed += OnExpandButtonPressed;
        AddToHeader(expandButton);

        // Create "add element" button.
        LabeledButton addButton = new();
        addButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        addButton.ButtonText = "Connect FromElement";
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
            FoldButton.Pressed += OnFoldButtonPressed;
            ExpandButton.Pressed += OnExpandButtonPressed;
            AddButton.Pressed += OnAddButtonPressed;
        }
    }

    /* Godot overrides. */
    public override void _Ready()
    {
        _Process(0.0);
    }

    public override void _Process(double delta)
    {
        ForceHideBorder = !IsOpen;
        FoldButton.Visible = IsOpen;
        ExpandButton.Visible = IsOpen;
        AddButton.Visible = IsOpen;

        base._Process(delta);
    }

    /* Private methods. */
    private void OnFoldButtonPressed()
    {
        for (int i = 0; i < GetContentsCount(); i++)
        {
            if (GetFromContents(i) is Inspector inspector)
            {
                if (inspector.ContentsContainer is FoldoutBorderContainer foldout)
                    foldout.IsOpen = false;
            }
        }
    }

    private void OnExpandButtonPressed()
    {
        for (int i = 0; i < GetContentsCount(); i++)
        {
            if (GetFromContents(i) is Inspector inspector)
            {
                if (inspector.ContentsContainer is FoldoutBorderContainer foldout)
                    foldout.IsOpen = true;
            }
        }
    }

    private void OnAddButtonPressed()
    {
        if (Template != null)
            AddToContents(Template.Copy());
        else
            GD.PrintErr($"We couldn't create an element because the list inspector '{Name}' did not have a template!");
    }
}