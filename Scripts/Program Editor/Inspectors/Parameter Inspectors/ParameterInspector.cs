using Godot;
using Rusty.EditorUI;
using System;

namespace Rusty.ISA.Editor
{
    /// <summary>
    /// A instruction parameter inspector.
    /// </summary>
    [GlobalClass]
    public abstract partial class ParameterInspector : Inspector
    {
        /* Public properties. */
        /// <summary>
        /// The parameter definition visualized by this inspector.
        /// </summary>
        public Parameter Definition
        {
            get => Resource as Parameter;
            set => Resource = value;
        }
        /// <summary>
        /// The value of this parameter.
        /// </summary>
        public abstract object ValueObj { get; set; }

        /// <summary>
        /// The root instruction inspector.
        /// </summary>
        public InstructionInspector Root { get; private set; }
        /// <summary>
        /// The preview generator for this inspector.
        /// </summary>
        public ParameterPreview Preview { get; private set; } = null;
        /// <summary>
        /// Whether or not the preview was updated this loop.
        /// </summary>
        public bool UpdatedPreview { get; private set; }

        /* Private properties. */
        private object LastValue { get; set; }

        /* Constructors. */
        public ParameterInspector(InstructionInspector root, Parameter parameter)
            : base(root.InstructionSet, parameter)
        {
            Root = root;
            Preview = new(this);
        }

        public ParameterInspector(ParameterInspector other) : base(other)
        {
            Preview = new(this);
        }

        /* Public methods. */
        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is ParameterInspector parameter)
            {
                Root = parameter.Root;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Create a parameter inspector of some type.
        /// </summary>
        public static ParameterInspector Create(InstructionInspector root, Parameter parameter)
        {
            switch (parameter)
            {
                case BoolParameter boolParameter:
                    return new BoolParameterInspector(root, boolParameter);
                case IntParameter intParameter:
                    return new IntParameterInspector(root, intParameter);
                case FloatParameter floatParameter:
                    return new FloatParameterInspector(root, floatParameter);
                case IntSliderParameter intSliderParameter:
                    return new IntSliderParameterInspector(root, intSliderParameter);
                case FloatSliderParameter floatSliderParameter:
                    return new FloatSliderParameterInspector(root, floatSliderParameter);
                case TextlineParameter TextlineParameter:
                    return new TextlineParameterInspector(root, TextlineParameter);
                case MultilineParameter multilineParameter:
                    return new MultilineParameterInspector(root, multilineParameter);
                case ColorParameter colorParameter:
                    return new ColorParameterInspector(root, colorParameter);
                case OutputParameter outputParameter:
                    return new OutputParameterInspector(root, outputParameter);
                default:
                    throw new ArgumentException($"Parameter '{parameter}' has an illegal type '{parameter.GetType().Name}'.");
            }
        }

        /// <summary>
        /// Set the inspector's argument value using a string.
        /// </summary>
        public virtual void SetValue(string str)
        {
            ValueObj = str;
        }

        /// <summary>
        /// Get the inspector's argument value as a string.
        /// </summary>
        public string GetValue()
        {
            if (ValueObj == null)
                return "";
            else
                return ValueObj.ToString();
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            base._Process(delta);

            // Check if we need to update the preview.
            GD.Print("Hit I'm parameter " + Definition);
            if (Preview == null)
                GD.Print("My preview is null, which shouldn't happen...");
            else
                GD.Print("My preview says " + Preview.Evaluate());
            UpdatedPreview = false;
            if (ValueObj == null && LastValue != null || ValueObj != null && !ValueObj.Equals(LastValue)
                || Root != null && Root.UpdatedPreview)
            {
                Preview = new(this);
                UpdatedPreview = true;
                LastValue = ValueObj;
            }
        }
    }
}