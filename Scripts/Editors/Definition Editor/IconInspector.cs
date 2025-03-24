using Godot;
using System.IO;
using Rusty.EditorUI;

namespace Rusty.ISA.DefinitionEditor
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
                string globalPath = PathUtility.GetPath(FilePath.Value);

                if (File.Exists(globalPath))
                {
                    byte[] bytes = File.ReadAllBytes(globalPath);

                    Image image = new();
                    if (globalPath.EndsWith(".bmp"))
                        image.LoadBmpFromBuffer(bytes);
                    else if (globalPath.EndsWith(".png"))
                        image.LoadPngFromBuffer(bytes);
                    else if (globalPath.EndsWith(".jpg") || globalPath.EndsWith(".jpeg"))
                        image.LoadJpgFromBuffer(bytes);
                    else if (globalPath.EndsWith(".tga"))
                        image.LoadTgaFromBuffer(bytes);
                    else if (globalPath.EndsWith(".svg"))
                        image.LoadSvgFromBuffer(bytes);
                    else if (globalPath.EndsWith(".webp"))
                        image.LoadWebpFromBuffer(bytes);
                    else if (globalPath.EndsWith(".ktx"))
                        image.LoadKtxFromBuffer(bytes);

                    ImageTexture imageTexture = new ImageTexture();
                    imageTexture.SetImage(image);

                    Preview.Value = imageTexture;
                }

                else
                {
                    ImageTexture imageTexture = new ImageTexture();

                    Preview.Value = imageTexture;
                }

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