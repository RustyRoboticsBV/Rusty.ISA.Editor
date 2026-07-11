using Godot;

namespace Rusty.ActionGraph.Graphs;

public sealed partial class GraphNode : GraphElement
{
    public override void _EnterTree()
    {
        PanelContainer panel = new();
        panel.MouseFilter = MouseFilterEnum.Ignore;
        PanelUtility.SetPanelStyle(panel, PanelUtility.GetStyleBox(Color.FromHtml("1A1A1A"), 4, 4, 4, 4));
        AddChild(panel);

        VBoxContainer vbox = new();
        vbox.MouseFilter = MouseFilterEnum.Ignore;
        panel.AddChild(vbox);

        PanelContainer header = new();
        header.MouseFilter = MouseFilterEnum.Ignore;
        header.CustomMinimumSize = new(0f, 32f);
        PanelUtility.SetPanelStyle(header, PanelUtility.GetStyleBox(Color.FromHtml("0088FF"), 4, 4, 0, 0));
        vbox.AddChild(header);

        MarginContainer headerMargin = MarginUtility.Create(0, 0, 16, 0);
        header.AddChild(headerMargin);

        HBoxContainer headerHbox = new();
        headerMargin.AddChild(headerHbox);

        MarginContainer iconMargin = MarginUtility.Create(4);
        headerHbox.AddChild(iconMargin);

        TextureRect icon = new();
        icon.ExpandMode = TextureRect.ExpandModeEnum.FitWidth;
        icon.CustomMinimumSize = new(22, 22);
        iconMargin.AddChild(icon);

        Label titleLabel = new();
        titleLabel.Text = "New Node";
        headerHbox.AddChild(titleLabel);

        CustomMinimumSize = new(80f, 60f);
    }
}
