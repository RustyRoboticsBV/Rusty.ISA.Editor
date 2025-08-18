using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A horizontal container that can display two elements and a divider. The divider can be dragged to resize the elements on
/// the container.
/// </summary>
public partial class HSplitContainer : HBoxContainer
{
    /* Private constants. */
    private const int DragMargin = 4;

    /* Public properties. */
    public Control Left => GetChild(0) as Control;
    public Control Divider => GetChild(1) as Control;
    public Control Right => GetChild(2) as Control;

    public float LeftMinSize { get; set; }
    public float RightMinSize { get; set; }
    public float CurrentFactor { get; set; } = 0.5f;

    /* Private properties. */
    private bool IsDragging { get; set; }
    private float DragStartPos { get; set; }
    private float DragStartLeftSize { get; set; }
    private float TotalSize => GetRect().Size.X - Divider.Size.X;

    /* Constructors. */
    public HSplitContainer(Control left, Control right)
    {
        AddThemeConstantOverride("separation", 0);

        AddChild(left);
        left.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        left.SizeFlagsVertical = SizeFlags.ExpandFill;

        AddChild(CreateDivider());

        AddChild(right);
        right.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        right.SizeFlagsVertical = SizeFlags.ExpandFill;
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        // Calculate total & top size.
        float newLeftSize = Mathf.Clamp(CurrentFactor, 0f, 1f) * TotalSize;

        // Respect minimum sizes.
        newLeftSize = Mathf.Clamp(newLeftSize, LeftMinSize, TotalSize - RightMinSize);

        // Apply new size.
        Left.CustomMinimumSize = new(newLeftSize, Left.CustomMinimumSize.Y);
    }

    /* Private methods. */
    private Control CreateDivider()
    {
        MarginContainer margin = new();
        margin.AddThemeConstantOverride("margin_left", DragMargin);
        margin.AddThemeConstantOverride("margin_right", DragMargin);
        margin.MouseFilter = MouseFilterEnum.Stop;
        margin.MouseDefaultCursorShape = CursorShape.Hsize;
        margin.FocusMode = FocusModeEnum.Click;
        margin.GuiInput += OnDividerGuiInput;

        ColorRect line = new();
        line.CustomMinimumSize = Vector2.One;
        line.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        line.SizeFlagsVertical = SizeFlags.ExpandFill;
        line.MouseFilter = MouseFilterEnum.Ignore;
        margin.AddChild(line);

        return margin;
    }

    private void OnDividerGuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton button && button.ButtonIndex == MouseButton.Left)
        {
            if (button.Pressed)
            {
                IsDragging = true;
                DragStartPos = GetGlobalMousePosition().X;
                DragStartLeftSize = Left.Size.X;
                Divider.GrabFocus();
            }
            else
                IsDragging = false;
            AcceptEvent();
        }
        else if (IsDragging && @event is InputEventMouseMotion motion)
        {
            // Figure out desired top element size.
            float from = DragStartPos;
            float to = GetGlobalMousePosition().X;
            float newDragPos = DragStartLeftSize + (to - from);

            // Calculate factor.
            if (TotalSize > 0f)
                CurrentFactor = newDragPos / TotalSize;
            else
                CurrentFactor = 0f;

            AcceptEvent();
        }
    }
}