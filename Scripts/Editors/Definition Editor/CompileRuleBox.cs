using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor.Definitions
{
    public partial class CompileRuleBox : VBoxContainer
    {
        /* Public properties. */
        public List<CompileRuleInspector> CompileRules { get; set; } = new();
        public bool MonoType { get; set; }

        /* Private properties. */
        private TabBar TabBar { get; set; }
        private VBoxContainer Margin { get; set; }
        private Button AddButton { get; set; }
        private Button RemoveButton { get; set; }

        /* Constructors. */
        public CompileRuleBox()
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill;

            HBoxContainer buttons = new();
            AddChild(buttons);

            AddButton = new Button() { Text = "Add Rule" };
            AddButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            buttons.AddChild(AddButton);
            AddButton.Pressed += OnAdd;

            RemoveButton = new Button() { Text = "Remove Rule" };
            RemoveButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            buttons.AddChild(RemoveButton);
            RemoveButton.Pressed += OnRemove;

            TabBar = new();
            AddChild(TabBar);

            Margin = new();
            AddChild(Margin);
            Margin.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        }

        /* Public methods. */
        public void Set(CompileRuleDescriptor rule)
        {
            Clear();
            Add(rule);
        }

        public void Set(List<CompileRuleDescriptor> rules)
        {
            Clear();
            foreach (var rule in rules)
            {
                Add(rule);
            }
        }

        public void Add(CompileRuleDescriptor rule)
        {
            CompileRuleInspector inspector = new();
            inspector.Value = rule;
            TabBar.AddTab();
            Margin.AddChild(inspector);
            CompileRules.Add(inspector);
        }

        public CompileRuleDescriptor GetFirst()
        {
            if (CompileRules.Count > 0)
                return CompileRules[0].Value;
            else
                return null;
        }

        public List<CompileRuleDescriptor> Get()
        {
            List<CompileRuleDescriptor> result = new();
            if (MonoType && CompileRules.Count > 0)
                result.Add(CompileRules[0].Value);
            else
            {
                foreach (var rule in CompileRules)
                {
                    result.Add(rule.Value);
                }
            }
            return result;
        }

        public void RemoveAt(int index)
        {
            Margin.RemoveChild(CompileRules[index]);
            CompileRules.RemoveAt(index);
            TabBar.RemoveTab(index);
        }

        public void Clear()
        {
            while (CompileRules.Count > 0)
            {
                RemoveAt(0);
            }
        }

        public override void _Process(double delta)
        {
            // If mono-type is enabled, hide tabs & buttons, and select tab 0. Also make sure there is at least one rule.
            if (MonoType && TabBar.TabCount > 1)
                TabBar.CurrentTab = 0;

            TabBar.Visible = !MonoType;
            AddButton.Visible = !MonoType;
            RemoveButton.Visible = !MonoType;

            if (MonoType && TabBar.TabCount == 0)
                OnAdd();

            // Set tab titles to match rule IDs.
            for (int i = 0; i < CompileRules.Count; i++)
            {
                TabBar.SetTabTitle(i, $"{CompileRules[i].ID.Value} ({CompileRules[i].Types[CompileRules[i].Type.Value]})");
                CompileRules[i].Visible = TabBar.CurrentTab == i;
            }

            // Set remove button visibility.
            if (!MonoType)
                RemoveButton.Visible = TabBar.TabCount != 0;
        }

        /* Private methods. */
        private void OnAdd()
        {
            CompileRuleInspector inspector = new();
            inspector.ID.Value = "rule " + TabBar.TabCount;
            TabBar.AddTab();
            Margin.AddChild(inspector);
            CompileRules.Add(inspector);
        }

        private void OnRemove()
        {
            if (TabBar.TabCount == 0)
                return;

            RemoveAt(TabBar.CurrentTab);
        }
    }
}