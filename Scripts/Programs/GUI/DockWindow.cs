using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// The base class for window tabs.
/// </summary>
public abstract partial class DockWindow<T> : MarginContainer
    where T : Control
{
    /* Public methods. */
    public int ContentsCount => Contents.GetChildCount();
    public string TitleText
    {
        get => TitleLabel.Text;
        set => TitleLabel.Text = value;
    }

    /* Private properties. */
    private VBoxContainer Contents { get; set; }
    private Label TitleLabel { get; set; }

    /* Constructors. */
    public DockWindow()
    {
        // Add background.
        ColorRect bg = new();
        bg.Color = EditorColors.Dock;
        AddChild(bg);

        // Add vbox.
        VBoxContainer vbox = new();
        AddChild(vbox);

        // Add title elements.
        MarginContainer titleContainer = CreateMargin(false);
        vbox.AddChild(titleContainer);

        VBoxContainer titleVbox = new();
        titleContainer.AddChild(titleVbox);

        TitleLabel = new();
        TitleLabel.Text = "Window";
        TitleLabel.HorizontalAlignment = HorizontalAlignment.Center;
        titleVbox.AddChild(TitleLabel);
        TitleLabel.Name = "Title";

        ColorRect separator = new();
        separator.Color = EditorColors.InternalSeparator;
        separator.CustomMinimumSize = new(1, 1);
        titleVbox.AddChild(separator);
        separator.Name = "InternalSeparator";

        // Add scroll view.
        ScrollContainer scroll = new();
        scroll.SizeFlagsVertical = SizeFlags.ExpandFill;
        vbox.AddChild(scroll);

        // Add margin around contents.
        MarginContainer contentsMargin = CreateMargin();
        scroll.AddChild(contentsMargin);

        // Add contents.
        Contents = new();
        contentsMargin.AddChild(Contents);
    }

    /* Public methods. */
    public void Add(T element)
    {
        Contents.AddChild(element);
    }

    public void Remove(T element)
    {
        Contents.RemoveChild(element);
    }

    public T GetAt(int index)
    {
        return Contents.GetChild(index) as T;
    }

    public bool Contains(T element)
    {
        return element.GetParent() == Contents;
    }

    /* Godot overrides. */
    public override void _Ready()
    {
        CustomMinimumSize = new(GetViewportRect().Size.X * 0.25f, 0);
    }

    /* Private methods. */
    private static MarginContainer CreateMargin(bool expandFillY = true)
    {
        MarginContainer margin = new();
        margin.AddThemeConstantOverride("margin_left", 4);
        margin.AddThemeConstantOverride("margin_right", 4);
        margin.AddThemeConstantOverride("margin_bottom", 4);
        margin.AddThemeConstantOverride("margin_top", 4);
        margin.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        if (expandFillY)
            margin.SizeFlagsVertical = SizeFlags.ExpandFill;
        return margin;
    }
}