using Godot;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A cutscene graph comment node.
    /// </summary>
    public partial class CutsceneGraphComment : GraphNode
    {
        public LineEdit Text { get; private set; }

        public override void _Ready()
        {
            Color textColor = new(0.3f, 0.75f, 0.3f);
            Color bgColor = new(0.1f, 0.1f, 0.1f, 0.9f);
            Color selectedBgColor = new(0.1f, 0.2f, 0.1f, 0.9f);

            Label label = new()
            {
                Text = "#"
            };
            label.AddThemeColorOverride("font_color", textColor);

            Text = new LineEdit()
            {
                Name = "Text",
                Text = "Comment text.",
                CustomMinimumSize = new Vector2(200f, 32f),
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                ExpandToTextLength = true,
            };
            Text.AddThemeColorOverride("font_color", textColor);

            HBoxContainer contents = new()
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            contents.AddChild(label);
            contents.AddChild(Text);

            MarginContainer margin = new();
            margin.AddThemeConstantOverride("margin_left", 4);
            margin.AddChild(contents);
            AddChild(margin);

            HBoxContainer titleBox = GetTitlebarHBox();
            titleBox.RemoveChild(titleBox.GetChild(0, true));
            titleBox.CustomMinimumSize = new Vector2(0f, 4f);
            titleBox.Size = Vector2.Zero;

            AddThemeStyleboxOverride("panel", new StyleBoxFlat()
            {
                BgColor = bgColor
            });
            AddThemeStyleboxOverride("titlebar", new StyleBoxFlat()
            {
                BgColor = bgColor
            });
            AddThemeStyleboxOverride("panel_selected", new StyleBoxFlat()
            {
                BgColor = selectedBgColor
            });
            AddThemeStyleboxOverride("titlebar_selected", new StyleBoxFlat()
            {
                BgColor = selectedBgColor
            });
        }

        public override void _Process(double delta)
        {
            Text.Size = Vector2.Zero;
            Size = Vector2.Zero;
            Size += new Vector2(10f, 10f);
        }
    }
}