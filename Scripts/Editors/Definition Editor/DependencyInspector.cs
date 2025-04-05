using Godot;
using Rusty.ISA.Editor.Programs;
using System;

namespace Rusty.ISA.Editor.Definitions
{
    /// <summary>
    /// A dependency inspector.
    /// </summary>
    public partial class DependencyInspector : HBoxContainer
    {
        /* Public properties. */
        public LineEdit TypeName { get; private set; }
        public Button Delete { get; private set; }

        public DependencyDescriptor Value
        {
            get => new(TypeName.Text);
            set
            {
                if (value != null)
                {
                    TypeName.Text = value.Name;
                }
            }
        }

        /* Public events. */
        public event Action<DependencyInspector> Deleted;

        /* Godot overrides. */
        public override void _Ready()
        {
            TypeName = new();
            AddChild(TypeName);
            TypeName.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            Delete = new();
            AddChild(Delete);
            Delete.Text = "Delete";
            Delete.Pressed += OnDelete;
        }

        /* Private methods. */
        private void OnDelete()
        {
            Deleted?.Invoke(this);
        }
    }
}