﻿using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A string parameter inspector.
    /// </summary>
    public partial class MultilineParameterInspector : ParameterInspector
    {
        /* Public properties. */
        /// <summary>
        /// The parameter definition visualized by this inspector.
        /// </summary>
        public new MultilineParameter Definition
        {
            get => base.Definition as MultilineParameter;
            set => base.Definition = value;
        }
        /// <summary>
        /// The value of the stored parameter.
        /// </summary>
        public string Value
        {
            get => MultilineField.Value;
            set => MultilineField.Value = value;
        }

        /* Private properties. */
        private MultilineField MultilineField { get; set; }

        /* Constructors. */
        public MultilineParameterInspector() : base() { }

        public MultilineParameterInspector(InstructionSet instructionSet, MultilineParameter parameter)
            : base(instructionSet, parameter)
        {
            MultilineField.Value = parameter.DefaultValue;
        }

        public MultilineParameterInspector(MultilineParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new MultilineParameterInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is MultilineParameterInspector otherInspector)
            {
                Value = otherInspector.Value;
                return true;
            }
            return false;
        }

        /* Protected methods. */
        protected override void Init()
        {
            // Base parameter inspector init.
            base.Init();

            // Set name.
            Name = $"MultilineParameter ({Definition.Id})";

            // Add multiline field.
            MultilineField = new();
            MultilineField.LabelText = Definition.DisplayName;
            MultilineField.Height = 128f;
            Add(MultilineField);
        }
    }
}