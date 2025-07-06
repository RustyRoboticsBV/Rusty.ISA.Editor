using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A foldout label that reacts to mouse input events.
/// </summary>
public partial class Foldout : LabeledElement
{
    /* Public methods. */
    public bool IsOpen { get; set; }
    public new string LabelText { get; set; }
    public new Color LabelColor { get; set; } = Colors.White;

    /* Public methods. */
    public override Foldout Copy()
    {
        Foldout copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(Foldout other)
    {
        CopyFrom(other as LabeledElement);
        IsOpen = other.IsOpen;
        LabelText = other.LabelText;
        LabelColor = other.LabelColor;
    }

    /* Godot overrides. */
    public override void _Ready()
    {
        base._Ready();
        _Process(0.0);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (IsOpen)
            base.LabelText = $"\u25BC  {LabelText}";
        else
            base.LabelText = $"\u25B6  {LabelText}";
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouse mouse)
        {
            if (GetGlobalRect().HasPoint(mouse.GlobalPosition))
            {
                base.LabelColor = new(LabelColor.R, LabelColor.G, LabelColor.B, 0.5f);
                if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed)
                    IsOpen = !IsOpen;
            }
            else
                base.LabelColor = LabelColor;
            }
        }
}