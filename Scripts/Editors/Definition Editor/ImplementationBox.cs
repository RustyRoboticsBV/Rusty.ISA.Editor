using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor.Definitions
{
    public partial class ImplementationBox : VBoxContainer
    {
        /* Public properties. */
        public CheckBox Enabled { get; private set; }
        public List<DependencyInspector> Dependencies { get; private set; } = new();
        public TextEdit Members { get; private set; }
        public TextEdit Initialize { get; private set; }
        public TextEdit Execute { get; private set; }

        public ImplementationDescriptor Value
        {
            get
            {
                if (Enabled.ButtonPressed)
                {
                    DependencyDescriptor[] dependencies = new DependencyDescriptor[Dependencies.Count];
                    for (int i = 0; i < dependencies.Length; i++)
                    {
                        dependencies[i] = Dependencies[i].Value;
                    }
                    return new(dependencies, Members.Text, Initialize.Text, Execute.Text);
                }
                else
                    return null;
            }
            set
            {
                Dependencies.Clear();
                while (DependencyBox.GetChildCount() > 0)
                {
                    DependencyBox.RemoveChild(DependencyBox.GetChild(0));
                }

                if (value != null)
                {
                    Enabled.ButtonPressed = true;
                    GD.Print("we have " + value.Dependencies.Count + " deps");
                    foreach (DependencyDescriptor dependency in value.Dependencies)
                    {
                        AddDependency(dependency);
                    }
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
        private VBoxContainer DependencyBox { get; set; }
        private Button AddButton { get; set; }

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
            DependencyBox = new();
            AddChild(DependencyBox);
            AddButton = new();
            AddButton.Text = "Add Dependency";
            AddButton.Pressed += OnAddDependency;
            AddChild(AddButton);
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
            foreach (DependencyInspector dependency in Dependencies)
            {
                dependency.Visible = TabBar.Visible && TabBar.CurrentTab == 0;
            }
            AddButton.Visible = TabBar.Visible && TabBar.CurrentTab == 0;
            Members.Visible = TabBar.Visible && TabBar.CurrentTab == 1;
            Initialize.Visible = TabBar.Visible && TabBar.CurrentTab == 2;
            Execute.Visible = TabBar.Visible && TabBar.CurrentTab == 3;
        }

        /* Private methods. */
        private void AddDependency(DependencyDescriptor dependency)
        {
            DependencyInspector inspector = new();
            DependencyBox.AddChild(inspector);
            Dependencies.Add(inspector);
            inspector.Value = dependency;
            inspector.Deleted += OnDeleteDependency;
        }


        private void OnAddDependency()
        {
            AddDependency(new());
        }

        private void OnDeleteDependency(DependencyInspector inspector)
        {
            Dependencies.Remove(inspector);
            DependencyBox.RemoveChild(inspector);
        }
    }
}