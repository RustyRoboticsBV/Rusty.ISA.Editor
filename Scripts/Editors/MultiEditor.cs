using Godot;
using Rusty.ISA.Editor.Definitions;
using Rusty.ISA.Editor.Programs;
using Rusty.ISA.Editor.SetBuilder;

namespace Rusty.ISA.Editor
{
    [GlobalClass]
    public partial class MultiEditor : VBoxContainer
    {
        [Export] public TabBar Tabs { get; set; }
        [Export] public ProgramEditor ProgramEditor { get; private set; }
        [Export] public DefinitionEditor DefinitionEditor { get; private set; }

        [Export] public InstructionSet BuiltInInstructions { get; private set; }
        [Export] public string UserDefinedInstructions { get; private set; } = "";

        public override void _EnterTree()
        {
            InstructionSet set = InstructionSetBuilder.Build(BuiltInInstructions, UserDefinedInstructions);
            ProgramEditor.UpdateInstructionSet(set);
            DefinitionEditor.BuiltInInstructions = BuiltInInstructions;
            DefinitionEditor.UserDefinedInstructions = UserDefinedInstructions;
        }

        public override void _Process(double delta)
        {
            ProgramEditor.Visible = Tabs.CurrentTab == 0;
            DefinitionEditor.Visible = Tabs.CurrentTab == 1;
        }
    }
}