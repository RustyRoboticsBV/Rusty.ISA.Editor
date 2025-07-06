using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A margin container with a border background.
/// </summary>
public partial class BorderContainer : MarginContainer
{
    /* Public properties. */
    public int MarginSize { get; set; } = 8;
    public int BorderThickness { get; set; } = 1;
    public bool HideBorderIfEmpty { get; set; } = true;
    public bool ForceHideBorder { get; set; }
    public bool HideHeader { get; set; }
    public bool HideContents { get; set; }
    public bool HideFooter { get; set; }
    public Color BackgroundColor { get; set; } = Colors.Transparent;
    public Color BorderColor { get; set; } = Colors.White;

    /* Private properties. */
    private ColorRect Background { get; set; }
    private Header Top { get; set; }
    private MarginContainer Middle { get; set; }
    private ColorRect Left { get; set; }
    private MarginContainer ContentsMargin { get; set; }
    private VBoxContainer Contents { get; set; }
    private ColorRect Right { get; set; }
    private Header Bottom { get; set; }
    private bool Initialized { get; set; }

    /* Constructors. */
    public BorderContainer()
    {
        Name = "BorderContainer";

        if (!Initialized)
            Initialize();
    }

    /* Godot overrides. */
    public override void _EnterTree()
    {
        if (!Initialized)
            Initialize();
    }

    public override void _Process(double delta)
    {
        // Apply margin size.
        int topOffset = (int)(Top.GetRect().Size.Y / 2f);
        int bottomOffset = (int)(Bottom.GetRect().Size.Y / 2f);

        Middle.AddThemeConstantOverride("margin_top", topOffset);
        Middle.AddThemeConstantOverride("margin_bottom", bottomOffset);

        ContentsMargin.AddThemeConstantOverride("margin_top", MarginSize + topOffset);
        ContentsMargin.AddThemeConstantOverride("margin_left", MarginSize + BorderThickness);
        ContentsMargin.AddThemeConstantOverride("margin_right", MarginSize + BorderThickness);
        ContentsMargin.AddThemeConstantOverride("margin_bottom", MarginSize + bottomOffset);

        // Apply colors.
        Background.Color = BackgroundColor;
        Top.BackgroundColor = BackgroundColor;
        Bottom.BackgroundColor = BackgroundColor;

        Top.LineColor = BorderColor;
        Left.Color = BorderColor;
        Right.Color = BorderColor;
        Bottom.LineColor = BorderColor;

        // Apply border thickness.
        Top.LineThickness = BorderThickness;
        Left.CustomMinimumSize = new(BorderThickness, CustomMinimumSize.Y);
        Right.CustomMinimumSize = new(BorderThickness, CustomMinimumSize.Y);
        Bottom.LineThickness = BorderThickness;

        // Hiding.
        Top.Contents.Visible = !HideHeader;
        ContentsMargin.Visible = !HideContents;
        Bottom.Contents.Visible = !HideFooter;

        bool visibleBorder = true;
        if (ForceHideBorder)
            visibleBorder = false;
        else if (HideBorderIfEmpty && !IsVisible(ContentsMargin))
            visibleBorder = false;

        Top.HideLine = !visibleBorder;
        Left.Visible = visibleBorder;
        Right.Visible = visibleBorder;
        Bottom.HideLine = !visibleBorder;
    }

    /* Public methods. */
    public void AddToContents(Control control)
    {
        Contents.AddChild(control);
    }

    public Control GetFromContents(int index)
    {
        return Contents.GetChild(index) as Control;
    }

    public int GetContentsCount() => Contents.GetChildCount();

    public void AddToHeader(Control control)
    {
        Top.Contents.AddChild(control);
        control.SizeFlagsVertical = SizeFlags.ShrinkCenter;
    }

    public void AddToFooter(Control control)
    {
        Bottom.Contents.AddChild(control);
        control.SizeFlagsVertical = SizeFlags.ShrinkCenter;
    }

    /* Protected methods. */
    protected virtual void Initialize()
    {
        Initialized = true;

        // Create middle.
        Middle = new();
        Middle.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Middle.SizeFlagsVertical = SizeFlags.ExpandFill;
        AddChild(Middle);
        Middle.Name = "Middle";

        // Create background.
        Background = new();
        Middle.AddChild(Background);
        Background.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Background.SizeFlagsVertical = SizeFlags.ExpandFill;
        Background.Color = Colors.Transparent;
        Background.Name = "Background";

        // Create left edge.
        Left = new();
        Middle.AddChild(Left);
        Left.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        Left.SizeFlagsVertical = SizeFlags.ExpandFill;
        Left.MouseFilter = MouseFilterEnum.Ignore;
        Left.CustomMinimumSize = new(BorderThickness, 1);
        Left.Name = "LeftEdge";

        // Create contents.
        ContentsMargin = new();
        Middle.AddChild(ContentsMargin);
        ContentsMargin.Name = "ContentsMargin";

        Contents = new();
        ContentsMargin.AddChild(Contents);
        Contents.Name = "Contents";

        // Create right edge.
        Right = new();
        Middle.AddChild(Right);
        Right.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
        Right.SizeFlagsVertical = SizeFlags.ExpandFill;
        Right.MouseFilter = MouseFilterEnum.Ignore;
        Right.CustomMinimumSize = new(BorderThickness, 1);
        Right.Name = "RightEdge";

        // Create top edge.
        Top = new();
        AddChild(Top);
        Top.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Top.SizeFlagsVertical = SizeFlags.ShrinkBegin;
        Top.Name = "TopEdge";

        // Create bottom edge.
        Bottom = new();
        AddChild(Bottom);
        Bottom.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Bottom.SizeFlagsVertical = SizeFlags.ShrinkEnd;
        Bottom.Name = "BottomEdge";

        /*Top.Contents.AddChild(new Label() { Text = "Top text!\nObtained e." });
        Contents.AddChild(new Label() { Text = "These are the\nmain contents." });
        Bottom.Contents.AddChild(new Label() { Text = "Bottom\ntext!" });*/
    }

    /* Private methods. */
    /// <summary>
    /// Get the vertical size of a container's contents. This assumes that all elements use the ShrinkCenter vertical size flag.
    /// </summary>
    private static float GetContentsSize(Container container)
    {
        int contentsSize = 0;
        for (int i = 0; i < container.GetChildCount(); i++)
        {
            Node child = container.GetChild(i);
            if (child is Control control && control.SizeFlagsVertical == SizeFlags.ShrinkCenter)
            {
                if (contentsSize < control.Size.Y)
                    contentsSize = Mathf.RoundToInt(control.Size.Y);
            }
        }
        return contentsSize;
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
}