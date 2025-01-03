using Rusty.Cutscenes;
using System;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A cutscene instruction parameter inspector.
    /// </summary>
    public abstract partial class ParameterInspector : Inspector
    {
        /* Public properties. */
        /// <summary>
        /// The parameter definition visualized by this inspector.
        /// </summary>
        public ParameterDefinition Definition
        {
            get => Resource as ParameterDefinition;
            set => Resource = value;
        }

        /* Constructors. */
        public ParameterInspector() : base() { }

        public ParameterInspector(InstructionSet instructionSet, ParameterDefinition parameter)
            : base(instructionSet, parameter) { }

        public ParameterInspector(ParameterInspector other) : base(other) { }

        /* Public methods. */
        /// <summary>
        /// Create a parameter inspector of some type.
        /// </summary>
        public static ParameterInspector Create(InstructionSet instructionSet, ParameterDefinition parameter)
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
                case LineParameter lineParameter:
                    return new LineParameterInspector(instructionSet, lineParameter);
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
    }
}