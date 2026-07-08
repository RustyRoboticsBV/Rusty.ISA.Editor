using Godot;

namespace Rusty.ActionGraph.Graphs;

public partial class Port : Panel
{
    private const float EDGE_THICKNESS = 2;

    private StyleBoxFlat StyleBox { get; set; }

    public Color EdgeColor
    {
        get => StyleBox.BorderColor;
        set => StyleBox.BorderColor = value;
    }

    public override void _EnterTree()
    {
        StyleBox = new();
        StyleBox.BgColor = Color.FromHtml("131313");
        StyleBox.BorderColor = Colors.White;
        AddThemeStyleboxOverride("panel", StyleBox);
    }

    public override void _Process(double delta)
    {
        int cornerRadius = (int)(Mathf.Min(Size.X, Size.Y) / 2f);
        StyleBox.CornerRadiusTopLeft = cornerRadius;
        StyleBox.CornerRadiusTopRight = cornerRadius;
        StyleBox.CornerRadiusBottomLeft = cornerRadius;
        StyleBox.CornerRadiusBottomRight = cornerRadius;
        StyleBox.BorderWidthBottom = 2;
        StyleBox.BorderWidthLeft = 2;
        StyleBox.BorderWidthRight = 2;
        StyleBox.BorderWidthTop = 2;
    }
}
