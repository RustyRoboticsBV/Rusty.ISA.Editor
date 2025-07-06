using Godot;

namespace Rusty.ISA.Editor;

public partial class ListBorderContainer : FoldoutBorderContainer
{
    /* Public properties. */
    public Element Template { get; set; }
    public string AddButtonText
    {
        get => AddButton.ButtonText;
        set => AddButton.ButtonText = value;
    }

    /* Private properties. */
    private LabeledButton AddButton { get; set; }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        base._Process(delta);

        // Hide the border & add button if the foldout is closed.
        ForceHideBorder = !FoldoutOpen;
        AddButton.Visible = FoldoutOpen;
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Never hide the border depending on if the container is empty.
        HideBorderIfEmpty = false;

        // Create "add element" button.
        AddButton = new();
        AddButton.ButtonText = "Add Element";
        AddButton.Pressed += OnAddButtonPressed;
        AddToFooter(AddButton);
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