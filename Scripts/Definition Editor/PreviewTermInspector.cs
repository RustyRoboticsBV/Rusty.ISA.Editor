using Godot;
using System.Collections.Generic;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Definitions
{
    /// <summary>
    /// A generic parameter definition inspector.
    /// </summary>
    public partial class PreviewTermInspector : VBoxContainer
    {
        /* Public methods. */
        public OptionField Type { get; private set; }
        public LineField Text { get; private set; }
        public OptionField HideIf { get; private set; }

        public PreviewTermDescriptor Value
        {
            get
            {
                return new(Types[Type.Value], Text.Value, (HideIf)HideIf.Value);
            }
            set
            {
                Type.Value = Types.IndexOf(value.Type);
                Text.Value = value.Value;
                HideIf.Value = (int)value.HideIf;

                if (Type.Value < 0)
                    Type.Value = 0;
                if (HideIf.Value < 0)
                    HideIf.Value = 0;
            }
        }

        public List<string> Types { get; set; } = new(new string[] { "text", "arg", "rule" });
        public List<string> Labels { get; set; } = new(new string[] { "Text", "Argument", "Compile Rule" });
        public List<string> HideIfLabels { get; set; } = new(new string[] { "Never", "Previous Empty", "Next Empty",
            "Either Empty", "Both Sides Empty" });

        /* Constructors. */
        public PreviewTermInspector()
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill;

            Type = new() { LabelText = "Type", Options = Labels.ToArray() };
            AddChild(Type);

            Text = new() { LabelText = "Text" };
            AddChild(Text);

            HideIf = new() { LabelText = "Hide If", Options = HideIfLabels.ToArray() };
            AddChild(HideIf);
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            if (Type.Value == 0)
                Text.LabelText = "Text";
            else if (Type.Value == 1)
                Text.LabelText = "Parameter ID";
            else if (Type.Value == 2)
                Text.LabelText = "Rule ID";
        }
    }
}