using Godot;

namespace Rusty.ISA.Editor;

[GlobalClass]
/// <summary>
/// A margin container with a border background.
/// </summary>
public partial class BorderContainer : MarginContainer, IContainer
{
    /* Public properties. */
    public int MarginSize { get; set; } = 8;
    public int BorderThickness { get; set; } = 1;
    public Color BorderColor { get; set; } = Colors.White;
    public Color BackgroundColor { get; set; } = Colors.Transparent;
    public bool HideBorderIfEmpty { get; set; } = true;
    public bool ForceHideBorder { get; set; }
    public bool HideHeader { get; set; }
    public bool HideContents { get; set; }
    public bool HideFooter { get; set; }
    public new virtual string TooltipText
    {
        get => base.TooltipText;
        set
        {
            base.TooltipText = value;
            Background.TooltipText = value;
            Contents.TooltipText = value;
            Top.TooltipText = value;
            Bottom.TooltipText = value;
        }
    }

    /* Protected properties. */
    protected Header Top { get; private set; }
    protected Header Bottom { get; private set; }

    /* Private properties. */
    private ColorRect Background { get; set; }
    private MarginContainer Middle { get; set; }
    private ColorRect Left { get; set; }
    private MarginContainer ContentsMargin { get; set; }
    private VerticalContainer Contents { get; set; }
    private ColorRect Right { get; set; }
    private bool Initialized { get; set; }

    /* Public events. */
    public event ChangedHandler Changed;

    /* Constructors. */
    public BorderContainer()
    {
        Name = "BorderContainer";

        if (!Initialized)
            Initialize();
    }

    /* Public methods. */
    public virtual IGuiElement Copy()
    {
        BorderContainer copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public virtual void CopyFrom(IGuiElement other)
    {
        if (other is BorderContainer border)
        {
            MarginSize = border.MarginSize;
            BorderThickness = border.BorderThickness;
            BorderColor = border.BorderColor;
            BackgroundColor = border.BackgroundColor;
            HideBorderIfEmpty = border.HideBorderIfEmpty;
            ForceHideBorder = border.ForceHideBorder;
            HideHeader = border.HideHeader;
            HideContents = border.HideContents;
            HideFooter = border.HideFooter;
            TooltipText = border.TooltipText;

            Top.CopyFrom(border.Top);
            Contents.CopyFrom(border.Contents);
            Bottom.CopyFrom(border.Bottom);
        }
    }


    public int GetContentsCount() => Contents.GetChildCount();

    public IGuiElement GetFromContents(int index)
    {
        return Contents.GetFromContents(index);
    }

    public void AddToContents(IGuiElement element)
    {
        Contents.AddToContents(element);
        Godot.GD.Print("Added " + element.Name + " to border container.");
        Changed?.Invoke();
    }

    public void RemoveFromContents(IGuiElement element)
    {
        Contents.RemoveFromContents(element);
        Changed?.Invoke();
    }

    public void ClearContents()
    {
        Contents.ClearContents();
        Changed?.Invoke();
    }


    public void AddToHeader(IGuiElement element)
    {
        Top.Add(element);
        ((Control)element).SizeFlagsVertical = SizeFlags.ShrinkCenter;
    }

    public IGuiElement GetFromHeader(int index)
    {
        return Top.GetAt(index);
    }


    public void AddToFooter(IGuiElement element)
    {
        Bottom.Add(element);
        ((Control)element).SizeFlagsVertical = SizeFlags.ShrinkCenter;
    }

    public IGuiElement GetFromFooter(int index)
    {
        return Bottom.GetAt(index);
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
        Top.HideContents = HideHeader;
        ContentsMargin.Visible = !HideContents;
        Bottom.HideContents = HideFooter;

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

        Contents.Changed += OnContentsChanged;

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

        Top.Changed += OnContentsChanged;

        // Create bottom edge.
        Bottom = new();
        AddChild(Bottom);
        Bottom.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Bottom.SizeFlagsVertical = SizeFlags.ShrinkEnd;
        Bottom.Name = "BottomEdge";

        Bottom.Changed += OnContentsChanged;
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

    private void OnContentsChanged()
    {
        Godot.GD.Print("Border container: the contents were changed.");
        Changed?.Invoke();
    }
}