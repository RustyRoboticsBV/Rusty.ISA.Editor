using Godot;
using System;

namespace Rusty.ISA.Editor;

/// <summary>
/// A vertical container that can display two elements and a divider. The divider can be dragged to resize the elements on
/// the container.
/// </summary>
public partial class VSplitContainer : VBoxContainer
{
    /* Private constants. */
    private const int DragMargin = 4;

    /* Public properties. */
    public Control Top => GetChild(0) as Control;
    public Control Divider => GetChild(1) as Control;
    public Control Bottom => GetChild(2) as Control;

    public float TopMinSize { get; set; }
    public float BottomMinSize { get; set; }
    public float CurrentFactor { get; set; } = 0.5f;

    /* Private properties. */
    private bool IsDragging { get; set; }
    private float DragStartPos { get; set; }
    private float DragStartTopSize { get; set; }
    private float TotalSize => GetRect().Size.Y - Divider.Size.Y;

    /* Constructors. */
    public VSplitContainer(Control top, Control bottom)
    {
        AddChild(top);
        top.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        top.SizeFlagsVertical = SizeFlags.ShrinkBegin;

        AddChild(CreateDivider());

        AddChild(bottom);
        bottom.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        bottom.SizeFlagsVertical = SizeFlags.ExpandFill;
        AddThemeConstantOverride("separation", 0);
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        // Calculate total & top size.
        float newTopSize = Mathf.Clamp(CurrentFactor, 0f, 1f) * TotalSize;

        // Apply new size.
        Top.CustomMinimumSize = new(Top.CustomMinimumSize.X, newTopSize);

        if (CurrentFactor < 0.2f)
            GD.Print(CurrentFactor + " => " + newTopSize);
    }

    /* Private methods. */
    private Control CreateDivider()
    {
        MarginContainer margin = new();
        margin.AddThemeConstantOverride("margin_top", DragMargin);
        margin.AddThemeConstantOverride("margin_bottom", DragMargin);
        margin.MouseFilter = MouseFilterEnum.Stop;
        margin.MouseDefaultCursorShape = CursorShape.Vsize;
        margin.FocusMode = FocusModeEnum.Click;
        margin.GuiInput += OnDividerGuiInput;

        ColorRect line = new();
        line.CustomMinimumSize = Vector2.One;
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
                DragStartPos = GetGlobalMousePosition().Y;
                DragStartTopSize = Top.Size.Y;
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
            float to = GetGlobalMousePosition().Y;
            float newDragPos = DragStartTopSize + (to - from);

            // Calculate factor.
            if (TotalSize > 0f)
                CurrentFactor = newDragPos / TotalSize;
            else
                CurrentFactor = 0f;

            GD.Print(CurrentFactor);

            AcceptEvent();
        }
    }
}