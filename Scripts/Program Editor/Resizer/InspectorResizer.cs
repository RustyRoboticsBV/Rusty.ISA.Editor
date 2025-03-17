using Godot;

namespace Rusty.ISA.Editor
{
    /// <summary>
    /// A horizontal resizer for the inspector window.
    /// </summary>
    [GlobalClass]
    public partial class InspectorResizer : VSeparator
    {
        /* Public properties. */
        [Export] Container Target { get; set; }
        [Export] float MinLeftMargin { get; set; } = 32f;
        [Export] float MinRightMargin { get; set; } = 32f;

        /* Private properties. */
        private bool Dragging { get; set; }

        /* Godot overrides. */
        public override void _EnterTree()
        {
            MouseDefaultCursorShape = CursorShape.Hsize;
        }

        public override void _GuiInput(InputEvent @event)
        {
            if (@event is InputEventMouseButton mouse && mouse.ButtonIndex == MouseButton.Left)
            {
                if (mouse.Pressed)
                    Dragging = true;
                else
                    Dragging = false;
            }
        }

        public override void _Process(double delta)
        {
            if (Dragging)
            {
                float targetX = GetViewport().GetMousePosition().X;
                if (GetParent() is Control control)
                    targetX -= control.GlobalPosition.X;
                targetX = Mathf.Clamp(targetX, MinLeftMargin, GetViewport().GetVisibleRect().Size.X - MinRightMargin);
                Target.CustomMinimumSize = new Vector2(targetX, Target.CustomMinimumSize.Y);
            }
        }
    }
}