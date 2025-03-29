using Godot;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Definitions
{
    public partial class IconInspector : ElementVBox
    {
        /* Public methods. */
        public LineField FilePath { get; private set; }
        public LabeledIcon Preview { get; private set; }

        /* Private properties. */
        private string LastFilePath { get; set; }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            if (LastFilePath != FilePath.Value)
            {
                string globalPath = PathUtility.GetPath("Definitions/" + FilePath.Value);
                Texture2D texture = IconLoader.Load(globalPath);
                if (texture != null)
                    Preview.Value = texture;
                else
                    Preview.Value = new ImageTexture();

                LastFilePath = FilePath.Value;
            }
        }

        /* Protected methods. */
        protected override void Init()
        {
            base.Init();

            FilePath = new()
            {
                Name = "FilePath",
                LabelText = "Icon Path"
            };
            Add(FilePath);

            Preview = new()
            {
                Name = "Preview",
                LabelText = ""
            };
            Add(Preview);
        }
    }
}