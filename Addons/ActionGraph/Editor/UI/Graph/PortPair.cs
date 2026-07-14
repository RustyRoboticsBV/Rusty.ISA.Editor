using Godot;

namespace Rusty.ActionGraph.Graphs;

public sealed partial class PortPair : HBoxContainer
{
    public Port Input { get; private set; }
    public Label LabelLeft { get; private set; }
    public Port Output { get; private set; }
    public Label LabelRight { get; private set; }

    private HBoxContainer Left { get; set; }

    public PortPair(bool input = false)
    {
        if (input)
        {
            Left = new();
            Left.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            AddChild(Left);

            Control anchorLeft = new();
            anchorLeft.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
            Left.AddChild(anchorLeft);

            Input = new();
            Input.Size = new(14, 14);
            Input.Position = new(-8f, 5f);
            Input.EdgeColor = Colors.Azure;
            anchorLeft.AddChild(Input);

            MarginContainer marginLeft = new();
            marginLeft.AddThemeConstantOverride("margin_left", 5);
            marginLeft.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            Left.AddChild(marginLeft);

            LabelLeft = new();
            LabelLeft.Text = "In";
            LabelLeft.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
            marginLeft.AddChild(LabelLeft);
        }

        HBoxContainer right = new();
        right.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(right);

        MarginContainer marginRight = new();
        marginRight.AddThemeConstantOverride("margin_right", 6);
        marginRight.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        right.AddChild(marginRight);

        LabelRight = new();
        LabelRight.Text = "Out";
        LabelRight.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
        marginRight.AddChild(LabelRight);

        Control anchorRight = new();
        anchorRight.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
        right.AddChild(anchorRight);

        Output = new();
        Output.Position = new(-8f, 5f);
        Output.Size = new(14, 14);
        Output.EdgeColor = Colors.Azure;
        anchorRight.AddChild(Output);
    }
}
