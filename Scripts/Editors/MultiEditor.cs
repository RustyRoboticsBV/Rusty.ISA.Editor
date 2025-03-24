using Godot;

namespace Rusty.ISA.Editor
{
    [GlobalClass]
    public partial class MultiEditor : VBoxContainer
    {
        [Export] public TabBar Tabs { get; set; }
        [Export] public Control ProgramEditor { get; private set; }
        [Export] public Control DefinitionEditor { get; private set; }

        public override void _Process(double delta)
        {
            ProgramEditor.Visible = Tabs.CurrentTab == 0;
            DefinitionEditor.Visible = Tabs.CurrentTab == 1;
        }
    }
}