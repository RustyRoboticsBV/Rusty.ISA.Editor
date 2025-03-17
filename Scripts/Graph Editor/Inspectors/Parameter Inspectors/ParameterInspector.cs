using Rusty.ISA;
using System;

namespace Rusty.ISA.Editor
{
    /// <summary>
    /// A instruction parameter inspector.
    /// </summary>
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

        /* Constructors. */
        public ParameterInspector() : base() { }

        public ParameterInspector(InstructionSet instructionSet, Parameter parameter)
            : base(instructionSet, parameter) { }

        public ParameterInspector(ParameterInspector other) : base(other) { }

        /* Public methods. */
        /// <summary>
        /// Create a parameter inspector of some type.
        /// </summary>
        public static ParameterInspector Create(InstructionSet instructionSet, Parameter parameter)
        {
            switch (parameter)
            {
                case BoolParameter boolParameter:
                    return new BoolParameterInspector(instructionSet, boolParameter);
                case IntParameter intParameter:
                    return new IntParameterInspector(instructionSet, intParameter);
                case FloatParameter floatParameter:
                    return new FloatParameterInspector(instructionSet, floatParameter);
                case IntSliderParameter intSliderParameter:
                    return new IntSliderParameterInspector(instructionSet, intSliderParameter);
                case FloatSliderParameter floatSliderParameter:
                    return new FloatSliderParameterInspector(instructionSet, floatSliderParameter);
                case TextParameter TextParameter:
                    return new TextParameterInspector(instructionSet, TextParameter);
                case MultilineParameter multilineParameter:
                    return new MultilineParameterInspector(instructionSet, multilineParameter);
                case ColorParameter colorParameter:
                    return new ColorParameterInspector(instructionSet, colorParameter);
                case OutputParameter outputParameter:
                    return new OutputParameterInspector(instructionSet, outputParameter);
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
    }
}