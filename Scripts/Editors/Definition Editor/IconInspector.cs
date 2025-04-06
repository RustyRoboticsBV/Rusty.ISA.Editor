using Godot;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Definitions
{
    public partial class IconInspector : HBoxContainer
    {
        /* Public methods. */
        public LineField FilePath { get; private set; }
        public TextureRect Preview { get; private set; }

        /* Private properties. */
        private static string StandardPath => PathUtility.GetPath("Definitions/");

        private string LastFilePath { get; set; }
        private FileDialog OpenIconDialog { get; set; }

        /* Constructors. */
        public IconInspector()
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill;

            FilePath = new();
            AddChild(FilePath);
            FilePath.Name = "FilePath";
            FilePath.LabelText = "Icon Path";
            FilePath.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            Button OpenButton = new();
            AddChild(OpenButton);
            OpenButton.Text = "Open";
            OpenButton.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
            OpenButton.Pressed += OnOpen;

            Preview = new();
            AddChild(Preview);
            Preview.Name = "Preview";
            Preview.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;

            OpenIconDialog = FileDialogMaker.GetOpen("Open Icon", StandardPath, "", "bmp");
            OpenIconDialog.FileSelected += OnIconSelected;
            AddChild(OpenIconDialog);
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            if (LastFilePath != FilePath.Value)
            {
                // Get global path.
                string globalPath = FilePath.Value;
                if (!FilePath.Value.StartsWith("C:\\"))
                    globalPath = StandardPath + globalPath;

                // Update preview.
                Texture2D texture = IconLoader.Load(globalPath);
                if (texture != null)
                {
                    Preview.Texture = texture;
                    Preview.Show();
                }
                else
                {
                    Preview.Texture = ImageTexture.CreateFromImage(Image.CreateEmpty(32, 32, false, Image.Format.Rgba8));
                    Preview.Hide();
                }

                LastFilePath = FilePath.Value;
            }
        }

        /* Private methods. */
        private void OnOpen()
        {
            OpenIconDialog.Show();
        }

        private void OnIconSelected(string path)
        {
            if (path.StartsWith(StandardPath))
                path = path.Substring(StandardPath.Length);
            FilePath.Value = path;
        }
    }
}