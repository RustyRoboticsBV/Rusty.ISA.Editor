using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A container for an output port and an associated label.
/// </summary>
internal sealed partial class OutputPortContainer : HBoxContainer
{
    /* Public properties. */
    public string LabelText
    {
        get => Label.Text;
        set => Label.Text = value;
    }
    public new string TooltipText
    {
        get => base.TooltipText;
        set
        {
            base.TooltipText = value;
            Label.TooltipText = value;
            Output.TooltipText = value;
        }
    }

    /* Private properties. */
    private Label Label { get; set; }
    private Port Output { get; set; }

    /* Constructors. */
    public OutputPortContainer()
    {
        Label = new();
        Label.Name = "Output Port Label";
        Label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        Label.HorizontalAlignment = HorizontalAlignment.Right;
        Label.Text = "Out";
        AddChild(Label);

        Control separator = new();
        separator.Name = "Space";
        AddChild(separator);

        Control anchor = new();
        anchor.Name = "Output Port Anchor";
        anchor.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        anchor.MouseFilter = MouseFilterEnum.Pass;
        AddChild(anchor);

        Output = new();
        Output.Name = "Output Port";
        Output.Position = new(-8f, 5f);
        Output.Size = new(14, 14);
        Output.Color = Colors.Azure;
        anchor.AddChild(Output);
    }
}