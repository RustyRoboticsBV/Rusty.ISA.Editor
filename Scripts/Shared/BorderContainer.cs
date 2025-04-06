using Godot;

namespace Rusty.ISA.Editor.Definitions
{
    /// <summary>
    /// A margin container with a border background.
    /// </summary>
    public partial class BorderContainer : MarginContainer
    {
        /* Private properties. */
        private new Control Anchor { get; set; }
        private NinePatchRect TextureRect { get; set; }
        private MarginContainer TitleMargin { get; set; }
        private ColorRect TitleColor { get; set; }
        private Label TitleLabel { get; set; }

        /* Constructors. */
        public BorderContainer() : this(BorderMaker.Default) { }

        public BorderContainer(Color color) : this(BorderMaker.CreateBorderTexture(color, 128)) { }

        public BorderContainer(Texture2D texture)
        {
            // Set margins.
            AddThemeConstantOverride("margin_left", 4);
            AddThemeConstantOverride("margin_right", 4);
            AddThemeConstantOverride("margin_bottom", 4);
            AddThemeConstantOverride("margin_top", 4);

            // Create anchor.
            Anchor = new();
            AddChild(Anchor);
            Anchor.Name = "Anchor";

            // Create border.
            TextureRect = BorderMaker.CreateBorderRect(texture);
            Anchor.AddChild(TextureRect);

            // Create title element.
            TitleMargin = new();
            Anchor.AddChild(TitleMargin);

            TitleColor = new();
            TitleColor.Color = Color.FromHtml("383838");
            TitleMargin.AddChild(TitleColor);

            TitleLabel = new();
            TitleMargin.AddChild(TitleLabel);
            TitleLabel.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;

            TitleMargin.Hide();
        }

        /* Public methods. */
        public void SetTitle(string text)
        {
            if (text != "")
                text = $"  {text}  ";
            TitleLabel.Text = text;

            _Process(0.0);
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            TextureRect.Size = Anchor.Size + new Vector2(8, 8);
            TextureRect.Position = new Vector2(-4, -4);

            if (TitleLabel.Text != "")
            {
                AddThemeConstantOverride("margin_top", 20);

                TitleColor.Size = TitleLabel.Size;
                TitleMargin.Position = new Vector2(20, -24);
                TitleMargin.Show();

                TextureRect.Position += new Vector2(0f, -8f);
                TextureRect.Size += new Vector2(0f, 8f);
            }
            else
            {
                AddThemeConstantOverride("margin_top", 4);
                TitleMargin.Hide();
            }
        }
    }
}