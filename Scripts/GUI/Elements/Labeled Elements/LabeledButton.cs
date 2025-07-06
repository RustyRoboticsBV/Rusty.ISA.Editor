using Godot;
using System;

namespace Rusty.ISA.Editor;

/// <summary>
/// A labeled button.
/// </summary>
public partial class LabeledButton : LabeledElement
{
    /* Public properties. */
    public string ButtonText
    {
        get => Button.Text;
        set => Button.Text = value;
    }

    /* Private properties. */
    private Button Button { get; set; }

    /* Public events. */
    public event Action Pressed;

    /* Constructors. */
    public LabeledButton() : base()
    {
        // Add button.
        Button = new();
        Button.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Button.Pressed += OnPressed;
        AddChild(Button);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        LabeledButton copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public override void CopyFrom(IGuiElement other)
    {
        base.CopyFrom(other);

        if (other is LabeledButton button)
            ButtonText = button.ButtonText;
    }

    /* Private methods. */
    private void OnPressed()
    {
        Pressed?.Invoke();
    }
}