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

    /* Public methods. */
    public override LabeledButton Copy()
    {
        LabeledButton copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(LabeledButton other)
    {
        CopyFrom(other as LabeledElement);
        ButtonText = other.ButtonText;
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Add button.
        Button = new();
        Button.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Button.Pressed += OnPressed;
        AddChild(Button);
    }

    /* Private methods. */
    private void OnPressed()
    {
        Pressed?.Invoke();
    }
}