using Godot;
using System;

namespace Rusty.ISA.Editor.Definitions
{
    /// <summary>
    /// A dependency inspector.
    /// </summary>
    public partial class DependencyInspector : HBoxContainer
    {
        /* Public properties. */
        public Label IndexLabel { get; private set; }
        public LineEdit TypeName { get; private set; }
        public Button Delete { get; private set; }

        public int Index
        {
            set => IndexLabel.Text = $"#{value}";
        }
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
            IndexLabel = new();
            AddChild(IndexLabel);
            IndexLabel.CustomMinimumSize = new(32f, 0f);
            IndexLabel.Text = "#0";

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