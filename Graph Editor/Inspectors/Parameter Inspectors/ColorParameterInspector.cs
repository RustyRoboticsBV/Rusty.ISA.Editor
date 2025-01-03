using Godot;
using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A color parameter inspector.
    /// </summary>
    public partial class ColorParameterInspector : ParameterInspector
    {
        /* Public properties. */
        public Color Value
        {
            get => ColorField.Value;
            set => ColorField.Value = value;
        }

        /* Private properties. */
        private ColorField ColorField { get; set; }

        /* Constructors. */
        public ColorParameterInspector() : base() { }

        public ColorParameterInspector(InstructionSet instructionSet, ColorParameter parameter)
            : base(instructionSet, parameter)
        {
            ColorField.Value = parameter.DefaultValue;
        }

        public ColorParameterInspector(ColorParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new ColorParameterInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is ColorParameterInspector otherInspector)
            {
                Value = otherInspector.Value;
                return true;
            }
            return false;
        }

        /* Protected methods. */
        protected override void Init(ParameterDefinition resource)
        {
            // Base parameter inspector init.
            base.Init(resource);

            // Set name.
            Name = $"ColorParameter ({resource.Id})";

            // Add Color field.
            ColorField = new();
            ColorField.LabelText = resource.DisplayName;
            Add(ColorField);
        }
    }
}