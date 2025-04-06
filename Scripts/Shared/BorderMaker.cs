using Godot;

namespace Rusty.ISA.Editor
{
	/// <summary>
	/// An utility for creating border textures.
	/// </summary>
	public static class BorderMaker
	{
		/* Fields. */
		private static Texture2D @default;

		/* Public properties. */
		/// <summary>
		/// The default border texture. Only gets created once.
		/// </summary>
		public static Texture2D Default
		{
			get
			{
				if (@default == null)
					@default = CreateBorderTexture();
				return @default;
			}
		}

		/* Public methods. */
		/// <summary>
		/// Create a gray border texture. The border always has a thickness of 1.
		/// </summary>
		public static Texture2D CreateBorderTexture()
		{
			return CreateBorderTexture(Color.FromHtml("898989"), 64);
		}

		/// <summary>
		/// Create a border texture of some size and color. The border always has a thickness of 1.
		/// </summary>
		public static Texture2D CreateBorderTexture(Color color, int size)
		{
			// Create image.
			Image image = Image.CreateEmpty(size, size, false, Image.Format.Rgba8);

			// Set vertical border.
			for (int i = 0; i < size; i++)
            {
                image.SetPixel(0, i, color);
				image.SetPixel(size - 1, i, color);
            }

			// Set horizontal border.
			for (int i = 1; i < size - 1; i++)
			{
				image.SetPixel(i, 0, color);
				image.SetPixel(i, size - 1, color);
			}

			// Create texture.
			ImageTexture texture = new();
			texture.SetImage(image);
			return texture;
        }

		/// <summary>
		/// Create a border nine-patch rect.
		/// </summary>
		public static NinePatchRect CreateBorderRect(Texture2D texture)
		{
			int patchMargin = Mathf.FloorToInt(texture.GetSize().X / 3f);

			NinePatchRect rect = new();
			rect.Texture = texture;
			rect.PatchMarginLeft = patchMargin;
            rect.PatchMarginRight = patchMargin;
            rect.PatchMarginBottom = patchMargin;
            rect.PatchMarginTop = patchMargin;
			return rect;
        }

        /// <summary>
        /// Create a border nine-patch rect, using the default texture.
        /// </summary>
        public static NinePatchRect CreateBorderRect()
		{
			return CreateBorderRect(Default);
		}

		/// <summary>
		/// Create a margin container with a border nine-patch rect inside of it.
		/// </summary>
		public static MarginContainer CreateBorderContainer(Texture2D texture)
		{
			MarginContainer container = new();
			container.AddChild(CreateBorderRect(texture));
			return container;
        }

        /// <summary>
        /// Create a margin container with a border nine-patch rect inside of it, using the default texture.
        /// </summary>
        public static MarginContainer CreateBorderContainer()
        {
            MarginContainer container = new();
            container.AddChild(CreateBorderRect());
            return container;
        }
    }
}