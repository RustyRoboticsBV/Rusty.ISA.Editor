using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An element of a list widget.
/// </summary>
internal sealed partial class ListElement : HBoxContainer, IWidget<ListElement>
{
    /* Public properties. */
    public Control Content { get; private set; }
    public UndoRedo UndoRedo
    {
        get
        {
            if (Content is IWidget widget)
                return widget.UndoRedo;
            return null;
        }
        set
        {
            if (Content is IWidget widget)
                widget.UndoRedo = value;
        }
    }

    /* Private properties. */
    private Button DragHandle { get; set; }
    private Button InsertButton { get; set; }
    private Button DuplicateButton { get; set; }
    private Button DeleteButton { get; set; }

    private bool Dragging { get; set; }

    /* Public events. */
    public event Action StateChanged;

    public event Action<ListElement> PressedInsert;
    public event Action<ListElement> PressedDuplicate;
    public event Action<ListElement> PressedDelete;
    public event Action<ListElement> MovedUp;
    public event Action<ListElement> MovedDown;

    /* Constructors. */
    public ListElement(Control content)
    {
        Content = content;

        MouseFilter = MouseFilterEnum.Pass;
        SizeFlagsHorizontal = SizeFlags.ExpandFill;

        // Drag handle.
        DragHandle = new Button();
        DragHandle.MouseFilter = MouseFilterEnum.Pass;
        DragHandle.CustomMinimumSize = new(32f, 0f);
        DragHandle.Text = "\u2637";
        DragHandle.TooltipText = "Drag to reorder element up or down.";
        DragHandle.MouseDefaultCursorShape = CursorShape.Drag;
        DragHandle.ButtonDown += () => Dragging = true;
        DragHandle.ButtonUp += () => Dragging = false;
        AddChild(DragHandle);

        // Action buttons.
        VBoxContainer actionButtons = new();
        actionButtons.SizeFlagsVertical = SizeFlags.ExpandFill;
        actionButtons.MouseFilter = MouseFilterEnum.Pass;
        AddChild(actionButtons);

        InsertButton = CreateActionButton("+\u2191", "Insert");
        InsertButton.SizeFlagsVertical = SizeFlags.ExpandFill;
        InsertButton.MouseFilter = MouseFilterEnum.Pass;
        InsertButton.Pressed += () => PressedInsert?.Invoke(this);
        actionButtons.AddChild(InsertButton);

        DuplicateButton = CreateActionButton("\u29C9", "Duplicate");
        DuplicateButton.SizeFlagsVertical = SizeFlags.ExpandFill;
        DuplicateButton.MouseFilter = MouseFilterEnum.Pass;
        DuplicateButton.Pressed += () => PressedDuplicate?.Invoke(this);
        actionButtons.AddChild(DuplicateButton);

        DeleteButton = CreateActionButton("\u00D7", "Delete");
        DeleteButton.SizeFlagsVertical = SizeFlags.ExpandFill;
        DeleteButton.MouseFilter = MouseFilterEnum.Pass;
        DeleteButton.Pressed += () => PressedDelete?.Invoke(this);
        actionButtons.AddChild(DeleteButton);

        // Contents.
        content.MouseFilter = MouseFilterEnum.Pass;
        content.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        content.SizeFlagsVertical = SizeFlags.ShrinkCenter;
        AddChild(content);

        if (content is IWidget widget)
            widget.StateChanged += () => StateChanged?.Invoke();
    }

    public ListElement(string title, Control content) : this(content) { }

    /* Public methods. */
    IWidget IWidget.DuplicateWidget() => DuplicateWidget();

    public ListElement DuplicateWidget()
    {
        ListElement copy = Content is IWidget widget
            ? new(widget.DuplicateWidget() as Control)
            : new(Content.Duplicate() as Control);

        return copy;
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
                MovedUp?.Invoke(this);
            else if (mousePosition.Y > bounds.End.Y)
                MovedDown?.Invoke(this);

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
}