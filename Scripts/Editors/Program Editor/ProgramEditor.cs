#if TOOLS
using Godot;
using System.Collections.Generic;
using Rusty.ISA.Editor.Programs.Compiler;
using System.IO;

namespace Rusty.ISA.Editor.Programs
{
    /// <summary>
    /// The ISA editor window. Contains a set of file buttons, an inspector and a graph edit.
    /// </summary>
    [GlobalClass]
    public partial class ProgramEditor : VBoxContainer
    {
        /* Public properties. */
        [Export] public InstructionSet InstructionSet { get; private set; }
        [Export] public MenuButton FileButton { get; private set; }

        [Export] public VBoxContainer Inspector { get; private set; }

        [Export] public ProgramGraphEdit GraphEdit { get; private set; }

        /* Private properties. */
        private FileDialog Dialog { get; set; }

        /* Public methods. */
        public void UpdateInstructionSet(InstructionSet set)
        {
            InstructionSet = set;
            GraphEdit.UpdateInstructionSet(set);
        }

        /* Godot overrides. */
        public override void _EnterTree()
        {
            FileButton.GetPopup().IndexPressed += OnIndexPressed;
        }

        /* Private methods. */
        private string Compile()
        {
            // Compile editor into graph.
            CompilerGraph graph = GraphEditCompiler.Compile(GraphEdit);

            // Compile graph into code.
            return GraphCompiler.Compile(graph);
        }

        private void Decompile(string code)
        {
            // Decompile code into instructions list.
            List<InstructionInstance> instructions = CodeDecompiler.Decompile(InstructionSet, code);

            // Decompile into graph.
            CompilerGraph graph = InstructionListDecompiler.Decompile(InstructionSet, instructions);
            GD.Print("Decompiled graph:\n" + graph);

            // Spawn into editor.
            GraphDecompiler.Spawn(GraphEdit, graph);
        }


        private void OnIndexPressed(long index)
        {
            switch (index)
            {
                case 0:
                    OnSave();
                    break;
                case 1:
                    OnOpen();
                    break;
                case 2:
                    OnCopy();
                    break;
                case 3:
                    OnPaste();
                    break;
            }
        }

        private void OnSave()
        {
            Dialog = FileDialogMaker.GetSave("Save Program", PathUtility.GetPath(""), "Program", "csv");
            AddChild(Dialog);
            Dialog.Show();
            Dialog.FileSelected += OnSaveFileSelected;
        }

        private void OnSaveFileSelected(string filePath)
        {
            File.WriteAllText(filePath, Compile());
        }

        private void OnOpen()
        {
            Dialog = FileDialogMaker.GetOpen("Open Program", PathUtility.GetPath(""), "Program", "csv");
            AddChild(Dialog);
            Dialog.Show();
            Dialog.FileSelected += OnOpenFileSelected;
        }

        private void OnOpenFileSelected(string filePath)
        {
            Decompile(File.ReadAllText(filePath));
        }

        private void OnCopy()
        {
            DisplayServer.ClipboardSet(Compile());
        }

        private void OnPaste()
        {
            Decompile(DisplayServer.ClipboardGet());
        }
    }
}
#endif