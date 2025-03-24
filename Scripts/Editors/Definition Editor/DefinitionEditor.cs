using Godot;
using System;
using System.IO;
using System.IO.Compression;

namespace Rusty.ISA.Editor.Definitions
{
    [GlobalClass]
    public partial class DefinitionEditor : MarginContainer
    {
        [Export] public InstructionSet InstructionSet { get; set; }
        [Export] public DefinitionInspector Inspector { get; set; }
        [Export] public Button ExportButton { get; set; }
        [Export] public Button ReloadButton { get; set; }
        [Export] public OptionButton Instructions { get; set; }

        public event Action Reloaded;

        private int Selected { get; set; } = -1;

        public override void _EnterTree()
        {
            // Add buttons.
            ExportButton.Pressed += OnExport;
            ReloadButton.Pressed += OnReload;
        }

        public override void _Process(double delta)
        {
            if (InstructionSet == null)
                Instructions.Clear();
            else if (Instructions.ItemCount != InstructionSet.Count)
            {
                Instructions.Clear();
                for (int i = 0; i < InstructionSet.Count; i++)
                {
                    Instructions.AddIconItem(InstructionSet[i].Icon, InstructionSet[i].DisplayName);
                }
            }

            if (Selected != Instructions.Selected)
            {
                Selected = Instructions.Selected;
                Inspector.Load(InstructionSet[Selected]);
            }
        }

        private void OnExport()
        {
            string path = ProjectSettings.GlobalizePath("res://.godot/Exported.zip");
            File.WriteAllBytes(path, SetSerializer.Serialize(InstructionSet));
        }

        private void OnReload()
        {
            Instructions.Clear();
            Reloaded?.Invoke();
        }
    }
}
