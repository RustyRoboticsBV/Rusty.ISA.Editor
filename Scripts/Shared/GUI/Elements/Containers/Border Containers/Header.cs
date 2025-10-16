using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A header element, used in border containers for the top and bottom edges.
/// </summary>
public partial class Header : HBoxContainer, IGuiElement
{
    /* Public properties. */
    public Color BackgroundColor
    {
        get => Background.Color;
        set => Background.Color = value;
    }

    public int LineThickness
    {
        get => (int)Left.CustomMinimumSize.Y;
        set
        {
            Left.CustomMinimumSize = new(Left.CustomMinimumSize.X, value);
            Right.CustomMinimumSize = new(Right.CustomMinimumSize.X, value);
        }
    }
    public Color LineColor
    {
        get => Left.Color;
        set
        {
            Left.Color = value;
            Right.Color = value;
        }
    }

    public bool HideLine
    {
        get => !Left.Visible;
        set
        {
            Left.Visible = !value;
            Right.Visible = !value;
        }
    }
    public bool HideContents
    {
        get => !Contents.Visible;
        set => Contents.Visible = !value;
    }
    public bool ShrinkRightEdge { get; set; }

    public new string TooltipText
    {
        get => base.TooltipText;
        set
        {
            base.TooltipText = value;
            for (int i = 0; i < Contents.GetContentsCount(); i++)
            {
                Contents.GetFromContents(i).TooltipText = value;
            }
        }
    }

    /* Private properties. */
    private ColorRect Background { get; set; }
    private ColorRect Left { get; set; }
    private MarginContainer ContentsMargin { get; set; }
    private HorizontalContainer Contents { get; set; }
    private ColorRect Right { get; set; }
    private bool Initialized { get; set; }

    /* Public events. */
    public event ChangedHandler Changed;

    /* Constructors. */
    public Header()
    {
        Initialize();
    }

    /* Public methods. */
    public IGuiElement Copy()
    {
        Header copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(IGuiElement other)
    {
        if (other is Header header)
        {
            BackgroundColor = header.BackgroundColor;
            LineThickness = header.LineThickness;
            LineColor = header.LineColor;
            HideLine = header.HideLine;
            HideContents = header.HideContents;
            ShrinkRightEdge = header.ShrinkRightEdge;
            Contents.CopyFrom(header.Contents);
        }
    }

    public void Add(IGuiElement element)
    {
        Contents.AddToContents(element);
        element.Changed += OnElementChanged;
    }

    public IGuiElement GetAt(int index)
    {
        return Contents.GetFromContents(index);
    }

    /* Godot overrides. */
    public override void _EnterTree()
    {
        Initialize();
    }

    public override void _Process(double delta)
    {
        // Make sure that the left part is not too small.
        Left.CustomMinimumSize = new(LineThickness + 8f, Left.CustomMinimumSize.Y);

        // Hide contents container if we have no contents.
        ContentsMargin.Visible = IsVisible(Contents);

        // Set margins.
        ContentsMargin.AddThemeConstantOverride("margin_left", Left.Visible ? 4 : 0);
        ContentsMargin.AddThemeConstantOverride("margin_right", Right.Visible ? 4 : 0);

        // Shrink/expand right edge.
        ContentsMargin.SizeFlagsHorizontal = ShrinkRightEdge ? SizeFlags.ExpandFill : SizeFlags.ShrinkBegin;
        Right.SizeFlagsHorizontal = ShrinkRightEdge ? SizeFlags.ShrinkEnd : SizeFlags.ExpandFill;
    }

    /* Private methods. */
    private void Initialize()
    {
        if (Initialized)
            return;

        Initialized = true;

        SizeFlagsHorizontal = SizeFlags.ExpandFill;
        SizeFlagsVertical = SizeFlags.ShrinkCenter;
        MouseFilter = MouseFilterEnum.Ignore;
        CustomMinimumSize = new(1f, 1f);
        AddThemeConstantOverride("separation", 0);

        Left = new();
        Left.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        Left.SizeFlagsVertical = SizeFlags.ShrinkCenter;
        Left.CustomMinimumSize = new(8f, 1f);
        AddChild(Left);
        Left.Name = "Left";

        ContentsMargin = new();
        ContentsMargin.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        ContentsMargin.SizeFlagsVertical = SizeFlags.Expand;
        ContentsMargin.AddThemeConstantOverride("margin_left", 4);
        ContentsMargin.AddThemeConstantOverride("margin_right", 4);
        AddChild(ContentsMargin);
        ContentsMargin.Name = "ContentsMargin";

        Background = new();
        Background.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Background.SizeFlagsVertical = SizeFlags.ExpandFill;
        Background.MouseFilter = MouseFilterEnum.Ignore;
        Background.Color = Colors.Transparent;
        ContentsMargin.AddChild(Background);
        Background.Name = "Background";

        Contents = new();
        Contents.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Contents.SizeFlagsVertical = SizeFlags.Expand;
        ContentsMargin.AddChild(Contents);
        Contents.Name = "Contents";

        Right = new();
        Right.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Right.SizeFlagsVertical = SizeFlags.ShrinkCenter;
        Right.CustomMinimumSize = new(8f, 1f);
        AddChild(Right);
        Right.Name = "Right";
    }

    private static bool IsVisible(Control control)
    {
        if (!control.Visible)
            return false;

        else if (control is Container container)
        {
            for (int i = 0; i < container.GetChildCount(); i++)
            {
                if (IsVisible(container.GetChild(i) as Control))
                    return true;
            }
            return false;
        }

        return true;
    }

    private void OnElementChanged()
    {
        Changed?.Invoke();
    }
}