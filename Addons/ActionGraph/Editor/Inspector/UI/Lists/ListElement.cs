using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

internal sealed partial class ListElement : HBoxContainer
{
    /* Public properties. */
    public IField Content { get; private set; }

    /* Private properties. */
    private Button DragHandle { get; set; }
    private VBoxContainer ActionButtons { get; set; }

    private bool Hovered { get; set; }
    private bool Dragging { get; set; }
    private Vector2 DragStartPosition { get; set; }

    /* Public events. */
    public event Action<ListElement> PressedInsert;
    public event Action<ListElement> PressedDuplicate;
    public event Action<ListElement> PressedDelete;
    public event Action<ListElement> MovedUp;
    public event Action<ListElement> MovedDown;

    /* Constructors. */
    public ListElement(Control content)
    {
        Content = content as IField;

        MouseFilter = MouseFilterEnum.Pass;
        SizeFlagsHorizontal = SizeFlags.ExpandFill;

        // Drag handle.
        DragHandle = new Button();
        DragHandle.CustomMinimumSize = new(32f, 64f);
        DragHandle.Text = "\u2637";
        DragHandle.TooltipText = "Drag to reorder element up or down.";
        DragHandle.MouseDefaultCursorShape = CursorShape.Drag;
        DragHandle.ButtonDown += OnHandlePressed;
        DragHandle.ButtonUp += OnHandleReleased;

        AddChild(DragHandle);

        // Contents.
        content.MouseFilter = MouseFilterEnum.Pass;
        content.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        content.SizeFlagsVertical = SizeFlags.ShrinkCenter;

        AddChild(content);

        // Action buttons.
        ActionButtons = new();
        ActionButtons.MouseFilter = MouseFilterEnum.Pass;

        Button insertButton = CreateActionButton("+", "Insert");
        insertButton.Pressed += () => PressedInsert?.Invoke(this);
        ActionButtons.AddChild(insertButton);

        Button duplicateButton = CreateActionButton("\u29C9", "Duplicate");
        duplicateButton.Pressed += () => PressedDuplicate?.Invoke(this);
        ActionButtons.AddChild(duplicateButton);

        Button deleteButton = CreateActionButton("\u00D7", "Delete");
        deleteButton.Pressed += () => PressedDelete?.Invoke(this);
        ActionButtons.AddChild(deleteButton);

        AddChild(ActionButtons);

        // Mouse events.
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

    /* Godot overrides. */
    public override void _Input(InputEvent @event)
    {
        if (!Dragging)
            return;

        if (@event is InputEventMouseMotion)
        {
            Vector2 mousePosition = GetGlobalMousePosition();
            Rect2 bounds = GetGlobalRect();

            if (mousePosition.Y < bounds.Position.Y)
            {
                MovedUp?.Invoke(this);
                DragStartPosition = mousePosition;
            }
            else if (mousePosition.Y > bounds.End.Y)
            {
                MovedDown?.Invoke(this);
                DragStartPosition = mousePosition;
            }

            AcceptEvent();
        }
    }

    /* Private methods. */
    private static Button CreateActionButton(string text, string tooltip)
    {
        Button button = new();
        button.Text = text;
        button.TooltipText = tooltip;
        button.CustomMinimumSize = new Vector2(28, 0);
        button.MouseDefaultCursorShape = CursorShape.PointingHand;
        return button;
    }

    private void OnMouseEntered()
    {
        Hovered = true;
        ActionButtons.Visible = true;
    }

    private void OnMouseExited()
    {
        Hovered = false;

        //if (!Dragging)
        //    ActionButtons.Visible = false;
    }

    private void OnHandlePressed()
    {
        Dragging = true;
        DragStartPosition = GetGlobalMousePosition();
    }

    private void OnHandleReleased()
    {
        Dragging = false;

        //if (!Hovered)
        //    ActionButtons.Visible = false;
    }
}