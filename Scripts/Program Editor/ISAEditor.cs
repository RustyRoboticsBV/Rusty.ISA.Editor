#if TOOLS
using Godot;
using Rusty.ISA.Editor.Compiler;
using Rusty.ISA.Editor.InstructionSets;
using Rusty.ISA.Importer.InstructionSets;

namespace Rusty.ISA.Editor
{
    /// <summary>
    /// The ISA editor window. Contains a set of file buttons, an inspector and a graph edit.
    /// </summary>
    [GlobalClass]
    public partial class ISAEditor : VBoxContainer
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

        [Export] public ProgramGraphEdit GraphEdit { get; private set; }

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
            GeneratedSet = InstructionSetBuilder.Build(InstructionSet, "Definitions");
            for (int i = 0; i < GeneratedSet.Definitions.Length; i++)
            {
                GD.Print("Definition " + i + ": " + GeneratedSet.Definitions[i]);
            }
            GraphEdit.InstructionSet = GeneratedSet;
            Serializer.Serialize(GeneratedSet, "InstructionSet/InstructionSet.zip");
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
            CompilerGraph graph = GraphEditCompiler.Compile(GraphEdit);
            string str = GraphCompiler.Compile(graph);
            DisplayServer.ClipboardSet(str);
            GD.Print("Debug compilation result saved to clipboard!");
        }

        private void OnDebugDecompile()
        {
            InstructionSet set = GraphEdit.InstructionSet;
            string code = DisplayServer.ClipboardGet();
            CompilerGraph graph = InstructionListDecompiler.Decompile(set, CodeDecompiler.Decompile(set, code));

            GD.Print("Decompiled graph:\n" + graph);

            GraphDecompiler.Spawn(GraphEdit, graph);

            GD.Print("Debug decompilation input loaded from clipboard!");
        }
    }
}
#endif