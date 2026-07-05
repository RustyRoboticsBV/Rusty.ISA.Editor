using Godot;

namespace Rusty.ISA.Graphs;

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
        panel.MouseFilter = Control.MouseFilterEnum.Ignore;
        panel.AnchorRight = 1.0f;
        panel.AnchorBottom = 1.0f;
        AddChild(panel);

        normalStyle = new StyleBoxFlat
        {
            BgColor = new Color(0.15f, 0.15f, 0.15f),
            BorderWidthLeft = 1,
            BorderWidthTop = 1,
            BorderWidthRight = 1,
            BorderWidthBottom = 1,
            BorderColor = new Color(0.3f, 0.3f, 0.3f)
        };

        selectedStyle = new StyleBoxFlat
        {
            BgColor = new Color(0.15f, 0.15f, 0.15f),
            BorderWidthLeft = 2,
            BorderWidthTop = 2,
            BorderWidthRight = 2,
            BorderWidthBottom = 2,
            BorderColor = Colors.DodgerBlue
        };
        panel.AddThemeStyleboxOverride("panel", normalStyle);


        label = new();
        label.Text = "New ID";
        panel.AddChild(label);
    }

    public override void _Process(double delta)
    {
        panel.AddThemeStyleboxOverride(
            "panel",
            Selected ? selectedStyle : normalStyle);
    }
}