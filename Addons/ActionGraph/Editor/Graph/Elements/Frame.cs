using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A frame graph element, which can contain other elements.
/// </summary>
internal sealed partial class Frame : GraphElement
{
    /* Private properties. */
    private PanelContainer Panel { get; set; }
    private Label Label { get; set; }

    private StyleBoxFlat NormalStyle { get; set; }
    private StyleBoxFlat SelectedStyle { get; set; }

    /* Constructors. */
    public Frame()
    {
        Panel = new();
        Panel.MouseFilter = MouseFilterEnum.Ignore;
        Panel.AnchorRight = 1.0f;
        Panel.AnchorBottom = 1.0f;
        AddChild(Panel);

        NormalStyle = PanelUtility.GetStyleBox(
            new Color(0.15f, 0.15f, 0.15f), 4, 4, 4, 4,
            new Color(0.3f, 0.3f, 0.3f), 1, 1, 1, 1
        );
        SelectedStyle = PanelUtility.GetStyleBox(
            new Color(0.15f, 0.15f, 0.15f), 4, 4, 4, 4,
            Colors.DodgerBlue, 2, 2, 2, 2
        );
        PanelUtility.SetPanelStyle(Panel, NormalStyle);

        Label = new();
        Label.Text = "New ID";
        Panel.AddChild(Label);
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        PanelUtility.SetPanelStyle(Panel, Selected ? SelectedStyle : NormalStyle);
    }
}