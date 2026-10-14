using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A sticky note graph element, used for annotating graphs with commentary.
/// </summary>
internal sealed partial class Memo : GraphElement
{
    /* Public properties. */
    public string Text
    {
        get => Label.Text;
        set => Label.Text = value;
    }
    public Color Color
    {
        get => Label.Modulate;
        set => Label.Modulate = value;
    }

    /* Private properties. */
    private Label Label { get; set; }

    /* Godot overrides. */
    public override void _EnterTree()
    {
        PanelContainer panel = new();
        panel.MouseFilter = MouseFilterEnum.Ignore;
        PanelUtility.SetPanelStyle(panel, PanelUtility.GetStyleBox(Color.FromHtml("1A1A1A"), 4, 4, 4, 4));
        AddChild(panel);

        MarginContainer margin = MarginUtility.Create(0, 16, 16, 0);
        panel.AddChild(margin);

        Label = new();
        Label.Text = "New Note";
        Label.Modulate = Colors.Green;
        margin.AddChild(Label);

        CustomMinimumSize = new(160f, 40f);
    }
}