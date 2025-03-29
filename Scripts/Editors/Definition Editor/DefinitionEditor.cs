using Godot;
using System.IO;
using System.Xml;
using Rusty.ISA.Editor.SetBuilder;

namespace Rusty.ISA.Editor.Definitions
{
    [GlobalClass]
    public partial class DefinitionEditor : MarginContainer
    {
        [Export] public MenuButton Menu { get; set; }
        [Export] public DefinitionInspector Inspector { get; set; }

        [Export] public InstructionSet BuiltInInstructions { get; set; }
        [Export] public string UserDefinedInstructions { get; set; } = "";

        private int Selected { get; set; } = -1;

        private FileDialog Dialog { get; set; }

        public override void _EnterTree()
        {
            Menu.GetPopup().IndexPressed += OnMenuPressed;
        }

        public override void _Process(double delta)
        {
            if (Dialog != null && !Dialog.Visible)
            {
                RemoveChild(Dialog);
                Dialog = null;
            }
        }

        private void OnMenuPressed(long index)
        {
            switch (index)
            {
                case 0:
                    OnExport();
                    break;
                case 1:
                    OnClear();
                    break;
                case 2:
                    OnSave();
                    break;
                case 3:
                    OnOpen();
                    break;
                case 4:
                    OnCopy();
                    break;
                case 5:
                    OnPaste();
                    break;
            }
        }

        private void OnExport()
        {
            string folderPath = PathUtility.GetPath(UserDefinedInstructions);

            Dialog = FileDialogMaker.GetSave("Export Instruction Set", folderPath, "InstructionSet", "zip");
            AddChild(Dialog);
            Dialog.Show();
            Dialog.FileSelected += OnSetSaved;
        }

        private void OnSetSaved(string path)
        {
            // Build instruction set.
            InstructionSet set = InstructionSetBuilder.Build(BuiltInInstructions, UserDefinedInstructions);

            // Save to file.
            File.WriteAllBytes(path, SetSerializer.Serialize(set));
        }

        private void OnClear()
        {
            Inspector.Clear();
        }

        private void OnSave()
        {
            string folderPath = PathUtility.GetPath(UserDefinedInstructions);

            InstructionDefinitionDescriptor desc = Inspector.Compile();
            string fileName = "Def" + desc.Opcode;

            FileDialog Dialog = FileDialogMaker.GetSave("Save Instruction Definition", folderPath, fileName, "xml");
            AddChild(Dialog);
            Dialog.Show();
            Dialog.FileSelected += OnDefinitionSaved;
        }

        private void OnDefinitionSaved(string path)
        {
            // Build instruction set.
            InstructionDefinitionDescriptor desc = Inspector.Compile();

            // Save to file.
            File.WriteAllText(path, desc.GetXml());
        }

        private void OnOpen()
        {
            string folderPath = PathUtility.GetPath(UserDefinedInstructions);

            Dialog = FileDialogMaker.GetOpen("Open Instruction Definition", folderPath, "", "xml");
            AddChild(Dialog);
            Dialog.Show();
            Dialog.FileSelected += OnDefinitionOpened;
        }

        private void OnDefinitionOpened(string path)
        {
            // Load XML.
            XmlDocument doc = new();
            doc.Load(path);

            // Create descriptor.
            InstructionDefinitionDescriptor desc = new(doc);

            // Load descriptor.
            Inspector.Load(desc);
        }

        private void OnCopy()
        {
            DisplayServer.ClipboardSet(Inspector.Compile().GetXml());
        }

        private void OnPaste()
        {
            // Read clipboard.
            string xml = DisplayServer.ClipboardGet();

            // Clear if the string is empty.
            if (xml == "")
            {
                OnClear();
                return;
            }

            // Load XML.
            XmlDocument doc = new();
            doc.LoadXml(xml);

            // Create descriptor.
            InstructionDefinitionDescriptor desc = new(doc);

            // Load descriptor.
            Inspector.Load(desc);
        }
    }
}
