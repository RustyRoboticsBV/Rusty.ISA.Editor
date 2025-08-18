using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A vertical resizer for a container.
/// </summary>
[GlobalClass]
public partial class VResizer : MarginContainer
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
        MouseDefaultCursorShape = CursorShape.Vsize;

        ColorRect line = new();
        line.CustomMinimumSize = Vector2.One;
        line.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        line.MouseFilter = MouseFilterEnum.Ignore;
        AddChild(line);

        AddThemeConstantOverride("margin_bottom", Margin);
        AddThemeConstantOverride("margin_top", Margin);
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouse && mouse.ButtonIndex == MouseButton.Left)
        {
            if (mouse.Pressed)
            {
                Dragging = true;
                StartDragPosition = GetGlobalMousePosition().Y;
                StartTargetWidth = Target.CustomMinimumSize.Y;
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
            float dragDelta = GetGlobalMousePosition().Y - StartDragPosition;
            float targetY = StartTargetWidth + dragDelta;

            // Apply target position as custom minimum width.
            Target.CustomMinimumSize = new Vector2(Target.CustomMinimumSize.X, targetY);
        }
    }
}