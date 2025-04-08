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
        private Control Border { get; set; }
        private VBoxContainer LeftContainer { get; set; }
        private VSeparator Left { get; set; }
        private VBoxContainer RightContainer { get; set; }
        private VSeparator Right { get; set; }
        private HBoxContainer TopContainer { get; set; }
        private HSeparator TopLeft { get; set; }
        private MarginContainer TopContents { get; set; }
        private HSeparator TopRight { get; set; }
        private HBoxContainer BottomContainer { get; set; }
        private HSeparator BottomLeft { get; set; }
        private MarginContainer BottomContents { get; set; }
        private HSeparator BottomRight { get; set; }

        /* Constructors. */
        public BorderContainer()
        {
            // Set margins.
            AddThemeConstantOverride("margin_left", MarginSize);
            AddThemeConstantOverride("margin_right", MarginSize);
            AddThemeConstantOverride("margin_bottom", MarginSize);
            AddThemeConstantOverride("margin_top", MarginSize);

            // Create border.
            Border = new();
            AddChild(Border);
            Border.MouseFilter = MouseFilterEnum.Ignore;

            // Left edge.
            LeftContainer = new();
            Border.AddChild(LeftContainer);
            LeftContainer.MouseFilter = MouseFilterEnum.Ignore;

            Left = new();
            LeftContainer.AddChild(Left);
            Left.SizeFlagsVertical = SizeFlags.ExpandFill;
            Left.MouseFilter = MouseFilterEnum.Ignore;

            // Right edge.
            RightContainer = new();
            Border.AddChild(RightContainer);
            RightContainer.MouseFilter = MouseFilterEnum.Ignore;

            Right = new();
            RightContainer.AddChild(Right);
            Right.SizeFlagsVertical = SizeFlags.ExpandFill;
            Right.MouseFilter = MouseFilterEnum.Ignore;

            // Top edge.
            TopContainer = new();
            Border.AddChild(TopContainer);
            TopContainer.AddThemeConstantOverride("separation", 0);
            TopContainer.MouseFilter = MouseFilterEnum.Ignore;

            TopLeft = new();
            TopContainer.AddChild(TopLeft);
            TopLeft.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
            TopLeft.MouseFilter = MouseFilterEnum.Ignore;

            TopContents = new();
            TopContainer.AddChild(TopContents);
            TopContents.AddThemeConstantOverride("margin_left", 4);
            TopContents.AddThemeConstantOverride("margin_right", 4);
            TopContents.MouseFilter = MouseFilterEnum.Ignore;

            TopRight = new();
            TopContainer.AddChild(TopRight);
            TopRight.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            TopRight.MouseFilter = MouseFilterEnum.Ignore;

            // Bottom edge.
            BottomContainer = new();
            Border.AddChild(BottomContainer);
            BottomContainer.AddThemeConstantOverride("separation", 0);
            BottomContainer.MouseFilter = MouseFilterEnum.Ignore;

            BottomLeft = new();
            BottomContainer.AddChild(BottomLeft);
            BottomLeft.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
            BottomLeft.MouseFilter = MouseFilterEnum.Ignore;

            BottomContents = new();
            BottomContainer.AddChild(BottomContents);
            BottomContents.AddThemeConstantOverride("margin_left", 4);
            BottomContents.AddThemeConstantOverride("margin_right", 4);
            BottomContents.MouseFilter = MouseFilterEnum.Ignore;

            BottomRight = new();
            BottomContainer.AddChild(BottomRight);
            BottomRight.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            BottomRight.MouseFilter = MouseFilterEnum.Ignore;
        }

        /* Public methods. */
        public void AddToTop(Control element)
        {
            TopContents.AddChild(element);
        }

        public void AddToBottom(Control element)
        {
            BottomContents.AddChild(element);
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            int topMargin = GetMargin(TopContents);
            int bottomMargin = GetMargin(BottomContents);

            AddThemeConstantOverride("margin_top", topMargin);
            AddThemeConstantOverride("margin_bottom", bottomMargin);

            LeftContainer.SetAnchorAndOffset(Side.Left, 0, -2 - MarginSize);
            LeftContainer.SetAnchorAndOffset(Side.Right, 0, 3 - MarginSize);
            LeftContainer.SetAnchorAndOffset(Side.Top, 0, 1 - MarginSize);
            LeftContainer.SetAnchorAndOffset(Side.Bottom, 1, -1 + MarginSize);

            RightContainer.SetAnchorAndOffset(Side.Left, 1, -3 + MarginSize);
            RightContainer.SetAnchorAndOffset(Side.Right, 1, 2 + MarginSize);
            RightContainer.SetAnchorAndOffset(Side.Top, 0, 1 - MarginSize);
            RightContainer.SetAnchorAndOffset(Side.Bottom, 1, -1 + MarginSize);

            TopContainer.SetAnchorAndOffset(Side.Left, 0, 1 - MarginSize);
            TopContainer.SetAnchorAndOffset(Side.Right, 1, -1 + MarginSize);
            TopContainer.SetAnchorAndOffset(Side.Top, 0, -Border.Size.Y / 2 - MarginSize);
            TopContainer.SetAnchorAndOffset(Side.Bottom, 0, Border.Size.Y / 2 - MarginSize);
            TopContents.Visible = TopContents.GetChildCount() > 0;

            BottomContainer.SetAnchorAndOffset(Side.Left, 0, 1 - MarginSize);
            BottomContainer.SetAnchorAndOffset(Side.Right, 1, -1 + MarginSize);
            BottomContainer.SetAnchorAndOffset(Side.Top, 1, -Border.Size.Y / 2 + MarginSize);
            BottomContainer.SetAnchorAndOffset(Side.Bottom, 1, Border.Size.Y / 2 + MarginSize);
            BottomContents.Visible = BottomContents.GetChildCount() > 0;

        }

        /* Private methods. */
        private int GetMargin(Node node)
        {
            int contentsSize = 0;
            for (int i = 0; i < TopContents.GetChildCount(); i++)
            {
                Node child = TopContents.GetChild(i);
                if (child is Control control)
                {
                    if (contentsSize < control.Size.Y)
                        contentsSize = (int)control.Size.Y;
                }
            }

            return Mathf.RoundToInt(contentsSize / 2f) + MarginSize;
        }
    }
}