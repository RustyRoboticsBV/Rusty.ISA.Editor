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

        /* Public properties. */
        public bool HideBorderIfEmpty { get; set; } = true;

        /* Private properties. */
        private Control Border { get; set; }
        private ColorRect BackgroundRect { get; set; }
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
            Name = "BorderContainer";

            // Set margins.
            AddThemeConstantOverride("margin_left", MarginSize);
            AddThemeConstantOverride("margin_right", MarginSize);
            AddThemeConstantOverride("margin_bottom", MarginSize);
            AddThemeConstantOverride("margin_top", MarginSize);

            // Create border.
            Border = new();
            AddChild(Border, false, InternalMode.Front);
            Border.Name = "Border";
            Border.MouseFilter = MouseFilterEnum.Ignore;

            BackgroundRect = new();
            Border.AddChild(BackgroundRect);
            BackgroundRect.Color = Colors.Transparent;

            // Left edge.
            LeftContainer = new();
            Border.AddChild(LeftContainer);
            LeftContainer.Name = "LeftContainer";
            LeftContainer.MouseFilter = MouseFilterEnum.Ignore;

            Left = new();
            LeftContainer.AddChild(Left);
            Left.SizeFlagsVertical = SizeFlags.ExpandFill;
            Left.MouseFilter = MouseFilterEnum.Ignore;

            // Right edge.
            RightContainer = new();
            Border.AddChild(RightContainer);
            RightContainer.Name = "RightContainer";
            RightContainer.MouseFilter = MouseFilterEnum.Ignore;

            Right = new();
            RightContainer.AddChild(Right);
            Right.SizeFlagsVertical = SizeFlags.ExpandFill;
            Right.MouseFilter = MouseFilterEnum.Ignore;

            // Top edge.
            TopContainer = new();
            Border.AddChild(TopContainer);
            TopContainer.Name = "TopContainer";
            TopContainer.AddThemeConstantOverride("separation", 0);
            TopContainer.MouseFilter = MouseFilterEnum.Ignore;

            TopLeft = new();
            TopContainer.AddChild(TopLeft);
            TopLeft.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
            TopLeft.SizeFlagsVertical = SizeFlags.ShrinkCenter;
            TopLeft.MouseFilter = MouseFilterEnum.Ignore;

            TopContents = new();
            TopContainer.AddChild(TopContents);
            TopContents.AddThemeConstantOverride("margin_left", 4);
            TopContents.AddThemeConstantOverride("margin_right", 4);
            TopContents.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
            TopContents.SizeFlagsVertical = SizeFlags.ShrinkCenter;
            TopContents.MouseFilter = MouseFilterEnum.Ignore;

            TopRight = new();
            TopContainer.AddChild(TopRight);
            TopRight.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            TopRight.SizeFlagsVertical = SizeFlags.ShrinkCenter;
            TopRight.MouseFilter = MouseFilterEnum.Ignore;

            // Bottom edge.
            BottomContainer = new();
            Border.AddChild(BottomContainer);
            BottomContainer.Name = "BottomContainer";
            BottomContainer.AddThemeConstantOverride("separation", 0);
            BottomContainer.MouseFilter = MouseFilterEnum.Ignore;

            BottomLeft = new();
            BottomContainer.AddChild(BottomLeft);
            BottomLeft.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
            BottomLeft.SizeFlagsVertical = SizeFlags.ShrinkCenter;
            BottomLeft.MouseFilter = MouseFilterEnum.Ignore;

            BottomContents = new();
            BottomContainer.AddChild(BottomContents);
            BottomContents.AddThemeConstantOverride("margin_left", 4);
            BottomContents.AddThemeConstantOverride("margin_right", 4);
            BottomContents.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
            BottomContents.SizeFlagsVertical = SizeFlags.ShrinkCenter;
            BottomContents.MouseFilter = MouseFilterEnum.Ignore;

            BottomRight = new();
            BottomContainer.AddChild(BottomRight);
            BottomRight.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            BottomRight.SizeFlagsVertical = SizeFlags.ShrinkCenter;
            BottomRight.MouseFilter = MouseFilterEnum.Ignore;
        }

        /* Public methods. */
        public void SetBackgroundColor(Color color)
        {
            BackgroundRect.Color = color;
        }

        public void AddToTop(Control element)
        {
            TopContents.AddChild(element);
            element.SizeFlagsVertical = SizeFlags.ShrinkCenter;
        }

        public void AddToBottom(Control element)
        {
            BottomContents.AddChild(element);
            element.SizeFlagsVertical = SizeFlags.ShrinkCenter;
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            BackgroundRect.Size = Border.Size;

            int visibleChildCount = GetVisibleChildCount(this);

            if (HideBorderIfEmpty)
            {
                BackgroundRect.Visible = visibleChildCount > 0;
                LeftContainer.Visible = visibleChildCount > 0;
                RightContainer.Visible = visibleChildCount > 0;
                TopLeft.Visible = visibleChildCount > 0;
                TopRight.Visible = visibleChildCount > 0;
                BottomLeft.Visible = visibleChildCount > 0;
                BottomRight.Visible = visibleChildCount > 0;
            }

            float topContentsSize = GetContentsSize(TopContainer);
            int topMargin = (int)topContentsSize + MarginSize;
            int topBorderMargin = (int)(topContentsSize / 2) + MarginSize;
            if (visibleChildCount == 0)
                topBorderMargin = topMargin;

            float bottomContentsSize = GetContentsSize(BottomContainer);
            int bottomMargin = (int)bottomContentsSize + MarginSize;
            int bottomBorderMargin = (int)(bottomContentsSize / 2) + MarginSize;
            if (visibleChildCount == 0)
                bottomBorderMargin = bottomMargin;

            AddThemeConstantOverride("margin_top", topMargin);
            AddThemeConstantOverride("margin_bottom", bottomMargin);

            LeftContainer.SetAnchorAndOffset(Side.Left, 0, -2 - MarginSize);
            LeftContainer.SetAnchorAndOffset(Side.Right, 0, 3 - MarginSize);
            LeftContainer.SetAnchorAndOffset(Side.Top, 0, 5 - topBorderMargin);
            LeftContainer.SetAnchorAndOffset(Side.Bottom, 1, -3 + bottomBorderMargin);

            RightContainer.SetAnchorAndOffset(Side.Left, 1, -3 + MarginSize);
            RightContainer.SetAnchorAndOffset(Side.Right, 1, 2 + MarginSize);
            RightContainer.SetAnchorAndOffset(Side.Top, 0, 5 - topBorderMargin);
            RightContainer.SetAnchorAndOffset(Side.Bottom, 1, -3 + bottomBorderMargin);

            TopContainer.SetAnchorAndOffset(Side.Left, 0, 1 - MarginSize);
            TopContainer.SetAnchorAndOffset(Side.Right, 1, -1 + MarginSize);
            TopContainer.SetAnchorAndOffset(Side.Top, 0, 4 - Border.Size.Y / 2 - topBorderMargin);
            TopContainer.SetAnchorAndOffset(Side.Bottom, 0, 4 + Border.Size.Y / 2 - topBorderMargin);
            TopContents.Visible = TopContents.GetChildCount() > 0;

            BottomContainer.SetAnchorAndOffset(Side.Left, 0, 1 - MarginSize);
            BottomContainer.SetAnchorAndOffset(Side.Right, 1, -1 + MarginSize);
            BottomContainer.SetAnchorAndOffset(Side.Top, 1, 0 - Border.Size.Y / 2 + bottomBorderMargin);
            BottomContainer.SetAnchorAndOffset(Side.Bottom, 1, -5 + Border.Size.Y / 2 + bottomBorderMargin);
            BottomContents.Visible = BottomContents.GetChildCount() > 0;
        }

        /* Private methods. */
        /// <summary>
        /// Get the vertical size of a container's contents. This assumes that all elements use the ShrinkCenter vertical size flag.
        /// </summary>
        private static float GetContentsSize(Container container)
        {
            int contentsSize = 0;
            for (int i = 0; i < container.GetChildCount(); i++)
            {
                Node child = container.GetChild(i);
                if (child is Control control && control.SizeFlagsVertical == SizeFlags.ShrinkCenter)
                {
                    if (contentsSize < control.Size.Y)
                        contentsSize = Mathf.RoundToInt(control.Size.Y);
                }
            }
            return contentsSize;
        }

        /// <summary>
        /// Get the number of visible children.
        /// </summary>
        private int GetVisibleChildCount(Container container)
        {
            int visible = 0;
            for (int i = 0; i < container.GetChildCount(); i++)
            {
                if (container.GetChild(i) is Control control && control.Visible)
                {
                    if (control is Container childContainer)
                    {
                        if (control is BorderContainer borderContainer)
                        {
                            visible += GetVisibleChildCount(borderContainer.TopContents);
                            visible += GetVisibleChildCount(borderContainer.BottomContents);
                        }
                        visible += GetVisibleChildCount(childContainer);
                    }
                    else
                        visible++;
                }
            }
            return visible;
        }
    }
}