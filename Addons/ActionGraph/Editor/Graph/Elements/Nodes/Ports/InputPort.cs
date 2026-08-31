using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A container for an input port and an associated label.
/// </summary>
internal partial class InputPort : HBoxContainer
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
            Input.TooltipText = value;
        }
    }

    /* Private properties. */
    private Label Label { get; set; }
    private Port Input { get; set; }

    /* Constructors. */
    public InputPort()
    {
        Control anchor = new();
        anchor.Name = "Input Port Anchor";
        anchor.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
        anchor.MouseFilter = MouseFilterEnum.Pass;
        AddChild(anchor);

        Input = new();
        Input.Name = "Input Port";
        Input.Position = new(-8f, 5f);
        Input.Size = new(14, 14);
        Input.EdgeColor = Colors.Azure;
        anchor.AddChild(Input);

        Control separator = new();
        separator.Name = "Space";
        AddChild(separator);

        Label = new();
        Label.Name = "Input Port Label";
        Label.Text = "In";
        Label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(Label);
    }
}