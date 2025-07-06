namespace Rusty.ISA.Editor;

public partial class CheckBoxBorderContainer : BorderContainer
{
    /* Public properties. */
    public string CheckBoxText
    {
        get => CheckBox.LabelText;
        set => CheckBox.LabelText = value;
    }
    public bool CheckBoxEnabled
    {
        get => CheckBox.Value;
        set => CheckBox.Value = value;
    }

    /* Private properties. */
    private CheckBoxField CheckBox => GetFromHeader(0) as CheckBoxField;

    /* Public methods. */
    public override IGuiElement Copy()
    {
        CheckBoxBorderContainer copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    /* Godot overrides. */
    public override void _EnterTree()
    {
        base._EnterTree();
    }

    public override void _Ready()
    {
        _Process(0.0);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        HideContents = !CheckBox.Value;
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();
        
        // Add check-box.
        CheckBoxField checkbox = new();
        checkbox.LabelText = CheckBoxText;
        checkbox.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        AddToHeader(checkbox);
    }
}