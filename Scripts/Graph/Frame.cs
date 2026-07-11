using Godot;

namespace Rusty.ActionGraph.Graphs;

[GlobalClass]
public sealed partial class Frame : GraphElement
{
    private PanelContainer panel;
    private Label label;

    private StyleBoxFlat normalStyle;
    private StyleBoxFlat selectedStyle;

    public override void _Ready()
    {
        panel = new();
        panel.MouseFilter = MouseFilterEnum.Ignore;
        panel.AnchorRight = 1.0f;
        panel.AnchorBottom = 1.0f;
        AddChild(panel);

        normalStyle = PanelUtility.GetStyleBox(
            new Color(0.15f, 0.15f, 0.15f), 4, 4, 4, 4,
            new Color(0.3f, 0.3f, 0.3f), 1, 1, 1, 1
        );
        selectedStyle = PanelUtility.GetStyleBox(
            new Color(0.15f, 0.15f, 0.15f), 4, 4, 4, 4,
            Colors.DodgerBlue, 2, 2, 2, 2
        );
        PanelUtility.SetPanelStyle(panel, normalStyle);

        label = new();
        label.Text = "New ID";
        panel.AddChild(label);
    }

    public override void _Process(double delta)
    {
        PanelUtility.SetPanelStyle(panel, Selected ? selectedStyle : normalStyle);
    }
}