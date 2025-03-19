using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor.Definitions
{
    public partial class PreviewTermBox : VBoxContainer
    {
        /* Public properties. */
        public List<PreviewTermInspector> PreviewTerms { get; private set; } = new();
        public RichTextLabel Preview { get; set; }

        /* Private properties. */
        private GridContainer Margin { get; set; }
        private Button RemoveButton { get; set; }

        /* Public methods. */
        public void Set(List<PreviewTermDescriptor> previewTerms)
        {
            Clear();
            foreach (var term in previewTerms)
            {
                Add(term);
            }
        }

        public void Add(PreviewTermDescriptor previewTerm)
        {
            PreviewTermInspector inspector = new();
            inspector.Value = previewTerm;
            Margin.AddChild(inspector);
            PreviewTerms.Add(inspector);
        }

        public void Clear()
        {
            while (PreviewTerms.Count > 0)
            {
                RemoveAt(0);
            }
        }

        public void RemoveAt(int index)
        {
            Margin.RemoveChild(PreviewTerms[index]);
            PreviewTerms.RemoveAt(index);
        }

        /* Godot overrides. */
        public override void _Ready()
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill;

            AddChild(new Label() { Text = "Preview Terms" });

            Preview = new();
            Preview.CustomMinimumSize = new Vector2(100, 20);
            Preview.BbcodeEnabled = true;
            AddChild(Preview);

            Margin = new();
            Margin.AddThemeConstantOverride("h_separation", 4);
            AddChild(Margin);

            HBoxContainer buttons = new();
            AddChild(buttons);

            Button addButton = new Button() { Text = "Add Preview Term" };
            addButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            buttons.AddChild(addButton);
            addButton.Pressed += OnAdd;

            RemoveButton = new Button() { Text = "Remove Preview Term" };
            RemoveButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            buttons.AddChild(RemoveButton);
            RemoveButton.Pressed += OnRemove;
        }

        public override void _Process(double delta)
        {
            RemoveButton.Visible = PreviewTerms.Count > 0;

            Margin.Columns = 3;

            Preview.Text = "";
            for (int i = 0; i < PreviewTerms.Count; i++)
            {
                PreviewTermDescriptor descriptor = PreviewTerms[i].Value;
                switch (descriptor.Type)
                {
                    case "text":
                        Preview.Text += descriptor.Value;
                        break;
                    case "arg":
                        Preview.Text += $"[i]({descriptor.Value})[/i]";
                        break;
                    case "rule":
                        Preview.Text += $"[i][{descriptor.Value}][/i]";
                        break;
                }
            }
            Preview.Visible = Preview.Text.Length != 0;
        }

        /* Private methods. */
        private void OnAdd()
        {
            Add(new());
        }

        private void OnRemove()
        {
            RemoveAt(PreviewTerms.Count - 1);
        }
    }
}