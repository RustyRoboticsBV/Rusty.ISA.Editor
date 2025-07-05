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
        margin.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        margin.AddThemeConstantOverride("margin_left", 8);
        margin.AddThemeConstantOverride("margin_right", 8);
        margin.MouseFilter = MouseFilterEnum.Ignore;
        AddChild(margin);

        Left = new();
        Left.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Left.MouseFilter = MouseFilterEnum.Ignore;
        margin.AddChild(Left);

        Right = new();
        Right.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Right.HorizontalAlignment = HorizontalAlignment.Right;
        Right.MouseFilter = MouseFilterEnum.Ignore;
        margin.AddChild(Right);
    }
}