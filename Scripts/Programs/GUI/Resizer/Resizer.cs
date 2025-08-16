using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A horizontal resizer for a container.
/// </summary>
[GlobalClass]
public partial class Resizer : MarginContainer
{
    /* Public properties. */
    public Container Target { get; set; }

    /* Private properties. */
    private bool Dragging { get; set; }
    private float StartDragPosition { get; set; }
    private float StartTargetWidth { get; set; }
    private int Margin => 4;

    /* Godot overrides. */
    public override void _EnterTree()
    {
        MouseFilter = MouseFilterEnum.Stop;
        MouseDefaultCursorShape = CursorShape.Hsize;

        ColorRect line = new();
        line.CustomMinimumSize = Vector2.One;
        line.SizeFlagsVertical = SizeFlags.ExpandFill;
        line.MouseFilter = MouseFilterEnum.Ignore;
        AddChild(line);

        AddThemeConstantOverride("margin_left", Margin);
        AddThemeConstantOverride("margin_right", Margin);
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouse && mouse.ButtonIndex == MouseButton.Left)
        {
            if (mouse.Pressed)
            {
                Dragging = true;
                StartDragPosition = GetGlobalMousePosition().X;
                StartTargetWidth = Target.CustomMinimumSize.X;
            }
            else
                Dragging = false;
        }
    }

    public override void _Process(double delta)
    {
        if (Dragging)
        {
            // Get target position.
            float dragDelta = GetGlobalMousePosition().X - StartDragPosition;
            float targetX = StartTargetWidth + dragDelta;

            // Apply target position as custom minimum width.
            Target.CustomMinimumSize = new Vector2(targetX, Target.CustomMinimumSize.Y);
        }
    }
}