#if TOOLS
using Godot;
using Rusty.Cutscenes;
using Rusty.CutsceneEditor.Compiler;

namespace Rusty.CutsceneEditor
{
    [GlobalClass]
    public partial class CutsceneEditor : VBoxContainer
    {
        /* Public properties. */
        [Export] public InstructionSet InstructionSet { get; set; }

        [Export] public Button OpenButton { get; private set; }
        [Export] public FileDialog OpenDialog { get; private set; }
        [Export] public Button SaveButton { get; private set; }
        [Export] public FileDialog SaveDialog { get; private set; }
        [Export] public Button DebugCompileButton { get; private set; }
        [Export] public Button DebugDecompileButton { get; private set; }

        [Export] public CutsceneGraphEdit GraphEdit { get; private set; }

        /* Godot overrides. */
        public override void _EnterTree()
        {
            OpenButton.Pressed += OnOpen;
            OpenDialog.FileSelected += OnOpenFileSelected;
            SaveButton.Pressed += OnSave;
            SaveDialog.FileSelected += OnSaveFileSelected;
            DebugCompileButton.Pressed += OnDebugCompile;
            DebugDecompileButton.Pressed += OnDebugDecompile;
        }

        /* Private methods. */
        private void OnOpen()
        {
            OpenDialog.Popup();
        }

        private void OnOpenFileSelected(string filePath)
        {
            GD.PrintErr("OnFileSelected hasn't been implemented yet!");
        }

        private void OnSave()
        {
            SaveDialog.Popup();
        }

        private void OnSaveFileSelected(string filePath)
        {
            GD.PrintErr("OnFileDeselected hasn't been implemented yet!");
        }

        private void OnDebugCompile()
        {
            string str = "";
            for (int i = 0; i < GraphEdit.GetChildCount(); i++)
            {
                Node node = GraphEdit.GetChild(i);
                if (!(node is CutsceneGraphNode graphNode))
                    continue;

                try
                {
                    if (str != "")
                        str += "\n";
                    str += GraphNodeCompiler.GetInstruction(graphNode).ToString();
                }
                catch (System.Exception ex)
                {
                    GD.PrintErr("COULD NOT COMPILE DUE TO ERROR:\n" + ex);
                }
            }
            DisplayServer.ClipboardSet(str);
            GD.Print("Debug compilation result saved to clipboard!");
        }

        private void OnDebugDecompile()
        {
            string str = "";
            DisplayServer.ClipboardSet(str);
            GD.Print("Debug decompilation result saved to clipboard!");
        }
    }
}
#endif