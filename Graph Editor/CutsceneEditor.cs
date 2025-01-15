#if TOOLS
using Godot;
using Rusty.Cutscenes;
using Rusty.CutsceneEditor.Compiler;
using Rusty.CutsceneEditor.InstructionSets;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// The cutscene editor window. Contains a set of file buttons, an inspector and a graph edit.
    /// </summary>
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

        [Export] public VBoxContainer Inspector { get;  private set; }

        [Export] public CutsceneGraphEdit GraphEdit { get; private set; }

        /* Private methods. */
        private InstructionSet GeneratedSet { get; set; }

        /* Godot overrides. */
        public override void _EnterTree()
        {
            OpenButton.Pressed += OnOpen;
            OpenDialog.FileSelected += OnOpenFileSelected;
            SaveButton.Pressed += OnSave;
            SaveDialog.FileSelected += OnSaveFileSelected;
            DebugCompileButton.Pressed += OnDebugCompile;
            DebugDecompileButton.Pressed += OnDebugDecompile;

            // Build instruction set.
            GeneratedSet = InstructionSetBuilder.Build(InstructionSet, ".godot/Definitions");
            for (int i = 0; i < GeneratedSet.Definitions.Length; i++)
            {
                GD.Print("Definition " + i + ": " + GeneratedSet.Definitions[i]);
            }
            GraphEdit.InstructionSet = GeneratedSet;
            InstructionSetCompiler.Compile(GeneratedSet, ".godot/Definitions/InstructionSet.zip");
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
            string str = GraphEditCompiler.Compile(GraphEdit);
            DisplayServer.ClipboardSet(str);
            GD.Print("Debug compilation result saved to clipboard!");
        }

        private void OnDebugDecompile()
        {
            InstructionSet set = GraphEdit.InstructionSet;
            string code = DisplayServer.ClipboardGet();
            CompilerGraph graph = InstructionListDecompiler.Decompile(set, CodeDecompiler.Decompile(set, code));
            GraphDecompiler.Spawn(GraphEdit, graph);

            GD.Print("Compiler graph:\n" + graph);
            GD.Print("Debug decompilation input loaded from clipboard!");
        }
    }
}
#endif