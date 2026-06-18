using Godot;

namespace Rusty.ISA.Console;

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