using Godot;

namespace Rusty.ISA.DefinitionEditor
{
    public partial class ImplementationBox : VBoxContainer
    {
        /* Public properties. */
        public CheckBox Enabled { get; private set; }
        public LineEdit Dependencies { get; private set; }
        public TextEdit Members { get; private set; }
        public TextEdit Initialize { get; private set; }
        public TextEdit Execute { get; private set; }

        public ImplementationDescriptor Value
        {
            get
            {
                if (Enabled.ButtonPressed)
                    return new(Dependencies.Text.Split(','), Members.Text, Initialize.Text, Execute.Text);
                else
                    return null;
            }
            set
            {
                if (value != null)
                {
                    string dependencies = "";
                    foreach (string dependency in value.Dependencies)
                    {
                        if (dependencies != "")
                            dependencies += ",";
                        dependencies += dependency;
                    }

                    Enabled.ButtonPressed = true;
                    Dependencies.Text = dependencies;
                    Members.Text = value.Members;
                    Initialize.Text = value.Initialize;
                    Execute.Text = value.Execute;
                }
                else
                    Enabled.ButtonPressed = false;
            }
        }
        /* Private properties. */
        private TabBar TabBar { get; set; }
        private VBoxContainer Margin { get; set; }
        private Button RemoveButton { get; set; }

        /* Godot overrides. */
        public override void _Ready()
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill;

            HBoxContainer EnableHBox = new();
            AddChild(EnableHBox);
            Enabled = new();
            EnableHBox.AddChild(Enabled);
            EnableHBox.AddChild(new Label() { Text = "Enabled?" });

            TabBar = new();
            TabBar.AddTab("Dependencies");
            TabBar.AddTab("Members");
            TabBar.AddTab("Initialize");
            TabBar.AddTab("Execute");
            AddChild(TabBar);
            TabBar.CurrentTab = 3;

            Dependencies = new();
            AddChild(Dependencies);
            Members = new() { CustomMinimumSize = new(0, 128f) };
            AddChild(Members);
            Initialize = new() { CustomMinimumSize = new(0, 128f) };
            AddChild(Initialize);
            Execute = new() { CustomMinimumSize = new(0, 128f) };
            AddChild(Execute);
        }

        public override void _Process(double delta)
        {
            TabBar.Visible = Enabled.ButtonPressed;
            Dependencies.Visible = TabBar.Visible && TabBar.CurrentTab == 0;
            Members.Visible = TabBar.Visible && TabBar.CurrentTab == 1;
            Initialize.Visible = TabBar.Visible && TabBar.CurrentTab == 2;
            Execute.Visible = TabBar.Visible && TabBar.CurrentTab == 3;
        }
    }
}