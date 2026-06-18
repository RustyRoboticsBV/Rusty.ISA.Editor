using Godot;

namespace Rusty.ISA.Consoles;

/// <summary>
/// A foldout label that reacts to mouse input events.
/// </summary>
public partial class Foldout : LabeledElement
{
    /* Public methods. */
    public bool IsOpen { get; set; }
    public new string LabelText { get; set; }
    public new Color LabelColor { get; set; } = Colors.White;

    /* Godot overrides. */
    public override void _Ready()
    {
        base._Ready();
        UpdateLabel();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        UpdateLabel();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouse mouse)
        {
            if (GetGlobalRect().HasPoint(mouse.GlobalPosition))
            {
                base.LabelColor = new(LabelColor.R, LabelColor.G, LabelColor.B, 0.5f);
                if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left
                    && mouseButton.Pressed)
                {
                    IsOpen = !IsOpen;
                }
            }
            else
                base.LabelColor = LabelColor;
        }
    }

    /* Private methods. */
    private void UpdateLabel()
    {
        if (IsOpen)
            base.LabelText = $"\u25BC  {LabelText}";
        else
            base.LabelText = $"\u25B6  {LabelText}";
    }
}





// TODO: merge into foldout class.

/// <summary>
/// An element with a label.
/// </summary>
public partial class LabeledElement : HBoxContainer
{
    /* Public properties. */
    public int LabelWidth
    {
        get => (int)Label.CustomMinimumSize.X;
        set
        {
            Label.CustomMinimumSize = new(LabelWidth, 0f);
            Label.Visible = Label.Text != "";
        }
    }
    public string LabelText
    {
        get => Label.Text;
        set => Label.Text = value;
    }
    public Color LabelColor
    {
        get => Label.Modulate;
        set => Label.Modulate = value;
    }
    public int LabelFontSize
    {
        get => Label.GetThemeFontSize("font_size");
        set => Label.AddThemeFontSizeOverride("font_size", value);
    }
    public Font LabelFont
    {
        get => Label.GetThemeFont("font");
        set => Label.AddThemeFontOverride("font", value);
    }
    public virtual new string TooltipText
    {
        get => base.TooltipText;
        set
        {
            base.TooltipText = value;
            Label.TooltipText = value;
        }
    }

    /* Protected properties. */
    protected Label Label { get; private set; }

    /* Constructors. */
    public LabeledElement()
    {
        // Create label.
        Label = new();
        Label.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        AddChild(Label);
    }
}