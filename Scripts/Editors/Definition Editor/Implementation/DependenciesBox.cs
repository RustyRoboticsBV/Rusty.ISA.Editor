using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor.Definitions
{
    /// <summary>
    /// A dependency list inspector.
    /// </summary>
    public partial class DependenciesBox : VBoxContainer
    {
        /* Private properties. */
        private List<DependencyInspector> Inspectors { get; } = new();
        private VBoxContainer Contents { get; } = new();

        /* Constructors. */
        public DependenciesBox()
        {
            Contents = new();
            AddChild(Contents);
            Contents.Name = "Contents";

            Button addButton = new();
            AddChild(addButton);
            addButton.Name = "AddButton";
            addButton.Text = "Add Dependency";
            addButton.Pressed += OnAdd;
        }

        /* Public methods. */
        public void Set(DependencyDescriptor[] dependencies)
        {
            Clear();
            foreach (var dependency in dependencies)
            {
                Add(dependency);
            }
        }

        public DependencyDescriptor[] Get()
        {
            List<DependencyDescriptor> result = new();
            foreach (var dependency in Inspectors)
            {
                result.Add(dependency.Value);
            }
            return result.ToArray();
        }

        public void Add(DependencyDescriptor dependency)
        {
            DependencyInspector inspector = new();
            Contents.AddChild(inspector);
            Inspectors.Add(inspector);
            inspector.Value = dependency;
            inspector.Deleted += OnDeleted;
        }

        public void Clear()
        {
            while (Inspectors.Count > 0)
            {
                Contents.RemoveChild(Inspectors[0]);
                Inspectors.RemoveAt(0);
            }
        }

        /* Private methods. */
        private void OnDeleted(DependencyInspector inspector)
        {
            Inspectors.Remove(inspector);
            Contents.RemoveChild(inspector);
        }

        private void OnAdd()
        {
            Add(new());
        }
    }
}