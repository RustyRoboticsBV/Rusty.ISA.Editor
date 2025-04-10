using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor.Definitions
{
    public partial class ParameterBox : VBoxContainer
    {
        /* Public properties. */
        public List<ParameterInspector> Parameters { get; set; } = new();

        /* Private properties. */
        private TabBar TabBar { get; set; }
        private VBoxContainer Contents { get; set; }
        private Button RemoveButton { get; set; }
        private Label NoneLabel { get; set; }

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
            Contents.AddChild(inspector);
            Parameters.Add(inspector);
        }

        public void RemoveAt(int index)
        {
            Contents.RemoveChild(Parameters[index]);
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

            BorderContainer border = new();
            AddChild(border);

            HBoxContainer header = new();
            border.AddToTop(header);

            TabBar = new();
            header.AddChild(TabBar);
            TabBar.ClipTabs = false;

            Button addButton = new Button();
            header.AddChild(addButton);
            addButton.Text = "       +       ";
            addButton.Pressed += OnAdd;

            Contents = new();
            border.AddChild(Contents);

            NoneLabel = new();
            Contents.AddChild(NoneLabel);
            NoneLabel.Text = "This instruction currently has no parameters.\nClick on the '+' button to add one.";

            RemoveButton = new();
            Contents.AddChild(RemoveButton);
            RemoveButton.Text = "Delete";
            RemoveButton.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
            RemoveButton.Pressed += OnRemove;
        }

        public override void _Process(double delta)
        {
            for (int i = 0; i < Parameters.Count; i++)
            {
                TabBar.SetTabTitle(i, $"{Parameters[i].ID.Value} ({Parameters[i].Types[Parameters[i].Type.Value]})");
                Parameters[i].Visible = TabBar.CurrentTab == i;
            }

            NoneLabel.Visible = Parameters.Count == 0;
            RemoveButton.Visible = Parameters.Count != 0;
        }

        /* Private methods. */
        private void OnAdd()
        {
            ParameterInspector inspector = new();
            inspector.ID.Value = "parameter " + TabBar.TabCount;
            TabBar.AddTab();
            Contents.AddChild(inspector);
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