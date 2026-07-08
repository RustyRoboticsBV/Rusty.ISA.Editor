using Godot;

public sealed partial class GraphNode : GraphElement
{
    public override void _EnterTree()
    {
        PanelContainer panel = new();
        panel.MouseFilter = MouseFilterEnum.Ignore;
        panel.AddThemeStyleboxOverride("panel", GetPanel(Color.FromHtml("1A1A1A"), 4, 4));
        AddChild(panel);

        VBoxContainer vbox = new();
        vbox.MouseFilter = MouseFilterEnum.Ignore;
        panel.AddChild(vbox);

        PanelContainer header = new();
        header.MouseFilter = MouseFilterEnum.Ignore;
        header.CustomMinimumSize = new(0f, 32f);
        header.AddThemeStyleboxOverride("panel", GetPanel(Color.FromHtml("0088FF"), 4, 1));
        vbox.AddChild(header);

        CustomMinimumSize = new(64f, 64f);
    }

    private static StyleBoxFlat GetPanel(Color color, int topRounding, int bottomRounding)
    {
        StyleBoxFlat stylebox = new();
        stylebox.BgColor = color;
        stylebox.CornerRadiusTopLeft = topRounding;
        stylebox.CornerRadiusTopRight = topRounding;
        stylebox.CornerRadiusBottomLeft = bottomRounding;
        stylebox.CornerRadiusBottomRight = bottomRounding;
        return stylebox;
    }
}
