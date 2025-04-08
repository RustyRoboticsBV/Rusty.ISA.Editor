using Godot;

namespace Rusty.ISA.Editor.Definitions
{
    public partial class ImplementationBox : VBoxContainer
    {
        /* Public properties. */
        public CheckBox Enabled { get; private set; }
        public DependenciesBox Dependencies { get; private set; } = new();
        public TextEdit Members { get; private set; }
        public TextEdit Initialize { get; private set; }
        public TextEdit Execute { get; private set; }

        public ImplementationDescriptor Value
        {
            get
            {
                if (Enabled.ButtonPressed)
                {
                    return new(Dependencies.Get(), Members.Text, Initialize.Text, Execute.Text);
                }
                else
                    return null;
            }
            set
            {
                if (value != null)
                {
                    Enabled.ButtonPressed = true;
                    Dependencies.Set(value.Dependencies.ToArray());
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
        private BorderContainer Border { get; set; }
        private Button AddButton { get; set; }

        /* Godot overrides. */
        public override void _Ready()
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill;

            HBoxContainer header = new();
            AddChild(header);

            HBoxContainer EnableHBox = new();
            header.AddChild(EnableHBox);
            Enabled = new();
            EnableHBox.AddChild(Enabled);
            EnableHBox.AddChild(new Label() { Text = "Enabled?  " });

            TabBar = new();
            TabBar.AddTab("Dependencies");
            TabBar.AddTab("Members");
            TabBar.AddTab("Initialize");
            TabBar.AddTab("Execute");
            header.AddChild(TabBar);
            TabBar.CurrentTab = 3;
            TabBar.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            Border = new();
            AddChild(Border);

            VBoxContainer contents = new();
            Border.AddChild(contents);

            Dependencies = new();
            contents.AddChild(Dependencies);

            Members = new() { CustomMinimumSize = new(0, 128f) };
            contents.AddChild(Members);

            Initialize = new() { CustomMinimumSize = new(0, 128f) };
            contents.AddChild(Initialize);

            Execute = new() { CustomMinimumSize = new(0, 128f) };
            contents.AddChild(Execute);
        }

        public override void _Process(double delta)
        {
            TabBar.Visible = Enabled.ButtonPressed;
            Border.Visible = Enabled.ButtonPressed;
            Dependencies.Visible = TabBar.Visible && TabBar.CurrentTab == 0;
            Members.Visible = TabBar.Visible && TabBar.CurrentTab == 1;
            Initialize.Visible = TabBar.Visible && TabBar.CurrentTab == 2;
            Execute.Visible = TabBar.Visible && TabBar.CurrentTab == 3;
        }
    }
}