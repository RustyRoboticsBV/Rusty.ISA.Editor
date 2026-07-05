using Godot;

namespace Rusty.ActionGraph.Consoles;

/// <summary>
/// A foldout label that reacts to mouse input events.
/// </summary>
public partial class Foldout : HBoxContainer
{
    /* Public properties. */
    public bool IsOpen { get; set; }

    public string LabelText
    {
        get => _labelText;
        set
        {
            _labelText = value;
            UpdateLabel();
        }
    }
    public Color LabelColor { get; set; } = Colors.White;
    public int LabelWidth
    {
        get => (int)Label.CustomMinimumSize.X;
        set
        {
            Label.CustomMinimumSize = new(value, 0f);
            Label.Visible = Label.Text != "";
        }
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

    /* Private fields. */
    private readonly Label Label;
    private string _labelText = "";

    /* Constructors. */
    public Foldout()
    {
        Label = new Label
        {
            SizeFlagsHorizontal = SizeFlags.ShrinkBegin
        };

        AddChild(Label);
    }

    /* Godot overrides. */
    public override void _Ready()
    {
        UpdateLabel();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventMouse mouse)
            return;

        if (GetGlobalRect().HasPoint(mouse.GlobalPosition))
        {
            Label.Modulate = new Color(LabelColor.R, LabelColor.G, LabelColor.B, 0.5f);

            if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
            {
                IsOpen = !IsOpen;
                UpdateLabel();
            }
        }
        else
            Label.Modulate = LabelColor;
    }

    /* Private methods. */
    private void UpdateLabel()
    {
        char c = IsOpen ? '\u25BC' : '\u25B6';
        Label.Text = $"{c}  {_labelText}";
    }
}