using Godot;

namespace Rusty.ActionGraph.Graphs;

public sealed partial class GraphNode : GraphElement
{
    /* Public properties. */
    public string HeaderText
    {
        get => TitleLabel.Text;
        set => TitleLabel.Text = value;
    }

    public Texture2D HeaderIcon
    {
        get => Icon.Texture;
        set => Icon.Texture = value;
    }

    /* Private properties. */
    private Label TitleLabel { get; set; }
    private TextureRect Icon { get; set; }
    private VBoxContainer Vbox { get; set; }

    /* Godot overrides. */
    public override void _EnterTree()
    {
        PanelContainer panel = new();
        panel.MouseFilter = MouseFilterEnum.Ignore;
        PanelUtility.SetPanelStyle(panel, PanelUtility.GetStyleBox(Color.FromHtml("1A1A1A"), 4, 4, 4, 4));
        AddChild(panel);

        Vbox = new();
        Vbox.MouseFilter = MouseFilterEnum.Ignore;
        panel.AddChild(Vbox);

        PanelContainer header = new();
        header.MouseFilter = MouseFilterEnum.Ignore;
        header.CustomMinimumSize = new(0f, 32f);
        PanelUtility.SetPanelStyle(header, PanelUtility.GetStyleBox(Color.FromHtml("0088FF"), 4, 4, 0, 0));
        Vbox.AddChild(header);

        MarginContainer headerMargin = MarginUtility.Create(0, 0, 16, 0);
        header.AddChild(headerMargin);

        HBoxContainer headerHbox = new();
        headerMargin.AddChild(headerHbox);

        MarginContainer iconMargin = MarginUtility.Create(4);
        headerHbox.AddChild(iconMargin);

        Icon = new();
        Icon.ExpandMode = TextureRect.ExpandModeEnum.FitWidth;
        Icon.CustomMinimumSize = new(22, 22);
        iconMargin.AddChild(Icon);

        TitleLabel = new();
        TitleLabel.Text = "New Node";
        headerHbox.AddChild(TitleLabel);

        Vbox.AddChild(new PortPair(true));

        CustomMinimumSize = new(80f, 60f);
    }
}