using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

internal sealed partial class ListElement : HBoxContainer
{
    /* Public properties. */
    public string TitleText
    {
        get => Foldable.Text;
        set => Foldable.Text = value;
    }
    public bool FoldoutOpen
    {
        get => Foldable.IsOpen;
        set => Foldable.SetOpen(value);
    }
    public Control Content { get; private set; }
    public UndoRedo UndoRedo
    {
        get
        {
            if (Content is IField field)
                return field.UndoRedo;
            return null;
        }
        set
        {
            if (Content is IField field)
                field.UndoRedo = UndoRedo;
        }
    }

    /* Private properties. */
    private Button DragHandle { get; set; }
    private FoldableHeader Foldable { get; set; }
    private Button InsertButton { get; set; }
    private Button DuplicateButton { get; set; }
    private Button DeleteButton { get; set; }

    private bool Dragging { get; set; }

    /* Public events. */
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
        DragHandle.ButtonDown += OnHandlePressed;
        DragHandle.ButtonUp += OnHandleReleased;
        AddChild(DragHandle);

        // Contents container.
        VBoxContainer contentsContainer = new();
        contentsContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(contentsContainer);

        // Action buttons.
        HBoxContainer actionButtons = new();
        actionButtons.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        actionButtons.MouseFilter = MouseFilterEnum.Pass;

        Foldable = new();
        Foldable.MouseFilter = MouseFilterEnum.Pass;
        Foldable.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Foldable.SizeFlagsVertical = SizeFlags.ShrinkCenter;
        Foldable.CustomMinimumSize = new(0, 31f); 
        Foldable.Text = "";
        Foldable.Pressed += () => content.Visible = Foldable.IsOpen;
        actionButtons.AddChild(Foldable);

        InsertButton = CreateActionButton("+\u2191", "Insert");
        InsertButton.MouseFilter = MouseFilterEnum.Pass;
        InsertButton.Pressed += () => PressedInsert?.Invoke(this);
        actionButtons.AddChild(InsertButton);

        DuplicateButton = CreateActionButton("\u29C9", "Duplicate");
        DuplicateButton.MouseFilter = MouseFilterEnum.Pass;
        DuplicateButton.Pressed += () => PressedDuplicate?.Invoke(this);
        actionButtons.AddChild(DuplicateButton);

        DeleteButton = CreateActionButton("\u00D7", "Delete");
        DeleteButton.MouseFilter = MouseFilterEnum.Pass;
        DeleteButton.Pressed += () => PressedDelete?.Invoke(this);
        actionButtons.AddChild(DeleteButton);

        // Contents.
        contentsContainer.AddChild(actionButtons);

        content.MouseFilter = MouseFilterEnum.Pass;
        content.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        content.SizeFlagsVertical = SizeFlags.ShrinkCenter;
        contentsContainer.AddChild(content);

        // Mouse events.
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

    public ListElement(string title, Control content) : this(content) => TitleText = title;

    /* Public methods. */
    public ListElement DuplicateElement()
    {
        ListElement copy = Content is IField field
            ? new(field.DuplicateField() as Control)
            : new(Content.Duplicate() as Control);
        copy.TitleText = TitleText;
        copy.FoldoutOpen = FoldoutOpen;
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

    private void OnMouseEntered()
    {
        InsertButton.Visible = true;
        DuplicateButton.Visible = true;
        DeleteButton.Visible = true;
    }

    private void OnMouseExited()
    {
        InsertButton.Visible = false;
        DuplicateButton.Visible = false;
        DeleteButton.Visible = false;
    }

    private void OnHandlePressed()
    {
        Dragging = true;
    }

    private void OnHandleReleased()
    {
        Dragging = false;
    }
}