using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A toggleable, single-line text field.
/// </summary>
public partial class ToggleTextField : GenericField<string, LineEdit>
{
    /* Public properties. */
    public override string Value
    {
        get => CheckBox.ButtonPressed ? Field.Text : null;
        set
        {
            CheckBox.ButtonPressed = value != null;
            Field.Text = value != null ? value : "";
        }
    }
    public override string TooltipText
    {
        get => base.TooltipText;
        set
        {
            base.TooltipText = value;
            CheckBox.TooltipText = value;
        }
    }

    /* Private properties. */
    private CheckBox CheckBox { get; set; }

    /* Constructors. */
    public ToggleTextField() : base()
    {
        CheckBox = new();
        AddChild(CheckBox);
        MoveChild(CheckBox, 1);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        ToggleTextField copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        base._Process(delta);

        Field.Visible = CheckBox.ButtonPressed;
    }
}