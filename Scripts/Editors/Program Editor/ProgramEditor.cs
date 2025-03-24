#if TOOLS
using Godot;
using System.Collections.Generic;
using Rusty.ISA.Editor.Programs.Compiler;

namespace Rusty.ISA.Editor.Programs
{
    /// <summary>
    /// The ISA editor window. Contains a set of file buttons, an inspector and a graph edit.
    /// </summary>
    [GlobalClass]
    public partial class ProgramEditor : VBoxContainer
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
            // Compile editor into graph.
            CompilerGraph graph = GraphEditCompiler.Compile(GraphEdit);

            // Compile graph into code.
            string str = GraphCompiler.Compile(graph);
            DisplayServer.ClipboardSet(str);
            GD.Print("Debug compilation result saved to clipboard!");
        }

        private void OnDebugDecompile()
        {
            // Decompile code into instructions list.
            string code = DisplayServer.ClipboardGet();
            List<InstructionInstance> instructions = CodeDecompiler.Decompile(InstructionSet, code);

            // Decompile into graph.
            CompilerGraph graph = InstructionListDecompiler.Decompile(InstructionSet, instructions);
            GD.Print("Decompiled graph:\n" + graph);

            // Spawn into editor.
            GraphDecompiler.Spawn(GraphEdit, graph);
            GD.Print("Debug decompilation input loaded from clipboard!");
        }
    }
}
#endif