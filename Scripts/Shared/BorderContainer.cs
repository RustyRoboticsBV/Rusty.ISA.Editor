using Godot;

namespace Rusty.ISA.Editor.Definitions
{
    /// <summary>
    /// A margin container with a border background.
    /// </summary>
    public partial class BorderContainer : MarginContainer
    {
        /* Constants. */
        private const int MarginSize = 8;

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
            AddThemeConstantOverride("margin_left", MarginSize);
            AddThemeConstantOverride("margin_right", MarginSize);
            AddThemeConstantOverride("margin_bottom", MarginSize);
            AddThemeConstantOverride("margin_top", MarginSize);

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
            TextureRect.Size = Anchor.Size + new Vector2(MarginSize * 2, MarginSize * 2);
            TextureRect.Position = new Vector2(-MarginSize, -MarginSize);

            if (TitleLabel.Text != "")
            {
                AddThemeConstantOverride("margin_top", MarginSize + 16);

                TitleColor.Size = TitleLabel.Size;
                TitleMargin.Position = new Vector2(20, -(20 + MarginSize));
                TitleMargin.Show();

                TextureRect.Position += new Vector2(0f, -MarginSize);
                TextureRect.Size += new Vector2(0f, MarginSize);
            }
            else
            {
                AddThemeConstantOverride("margin_top", MarginSize);
                TitleMargin.Hide();
            }
        }
    }
}