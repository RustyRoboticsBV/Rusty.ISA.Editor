using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.DefinitionEditor
{
    public partial class ParameterBox : VBoxContainer
    {
        /* Public properties. */
        public List<ParameterInspector> Parameters { get; set; } = new();

        /* Private properties. */
        private TabBar TabBar { get; set; }
        private VBoxContainer Margin { get; set; }
        private Button RemoveButton { get; set; }

        /* Public methods. */
        public void Set(List<ParameterDescriptor> parameters)
        {
            Clear();
            foreach (var parameter in parameters)
            {
                Add(parameter);
            }
        }

        public void Add(ParameterDescriptor parameter)
        {
            ParameterInspector inspector = new();
            inspector.Value = parameter;
            TabBar.AddTab();
            Margin.AddChild(inspector);
            Parameters.Add(inspector);
        }

        public void RemoveAt(int index)
        {
            Margin.RemoveChild(Parameters[index]);
            Parameters.RemoveAt(index);
            TabBar.RemoveTab(index);
        }

        public void Clear()
        {
            while (Parameters.Count > 0)
            {
                RemoveAt(0);
            }
        }

        /* Godot overrides. */
        public override void _Ready()
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill;

            HBoxContainer buttons = new();
            AddChild(buttons);

            Button addButton = new Button() { Text = "Add Parameter" };
            addButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            buttons.AddChild(addButton);
            addButton.Pressed += OnAdd;

            RemoveButton = new Button() { Text = "Remove Parameter" };
            RemoveButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            buttons.AddChild(RemoveButton);
            RemoveButton.Pressed += OnRemove;

            TabBar = new();
            AddChild(TabBar);

            Margin = new();
            AddChild(Margin);
        }

        public override void _Process(double delta)
        {
            for (int i = 0; i < Parameters.Count; i++)
            {
                TabBar.SetTabTitle(i, $"{Parameters[i].ID.Value} ({Parameters[i].Types[Parameters[i].Type.Value]})");
                Parameters[i].Visible = TabBar.CurrentTab == i;
            }

            RemoveButton.Visible = TabBar.TabCount != 0;
        }

        /* Private methods. */
        private void OnAdd()
        {
            ParameterInspector inspector = new();
            inspector.ID.Value = "parameter " + TabBar.TabCount;
            TabBar.AddTab();
            Margin.AddChild(inspector);
            Parameters.Add(inspector);
        }

        private void OnRemove()
        {
            if (TabBar.TabCount == 0)
                return;

            RemoveAt(TabBar.CurrentTab);
        }
    }
}