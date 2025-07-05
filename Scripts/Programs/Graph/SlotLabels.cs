using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A graph node label pair for an input and output slot pair.
/// </summary>
public partial class SlotLabels : HBoxContainer
{
    /* Public properties. */
    public string InputText
    {
        get => Left.Text;
        set => Left.Text = value;
    }
    public string OutputText
    {
        get => Right.Text;
        set => Right.Text = value;
    }

    /* Private properties. */
    private Label Left { get; set; }
    private Label Right { get; set; }

    /* Constructors. */
    public SlotLabels()
    {
        MarginContainer margin = new();
        margin.AddThemeConstantOverride("margin_left", 8);
        margin.AddThemeConstantOverride("margin_right", 8);
        margin.MouseFilter = MouseFilterEnum.Ignore;
        margin.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(margin);

        HBoxContainer hbox = new();
        hbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        margin.AddChild(hbox);

        Left = new();
        Left.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Left.MouseFilter = MouseFilterEnum.Ignore;
        hbox.AddChild(Left);

        Right = new();
        Right.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Right.HorizontalAlignment = HorizontalAlignment.Right;
        Right.MouseFilter = MouseFilterEnum.Ignore;
        hbox.AddChild(Right);
    }
}