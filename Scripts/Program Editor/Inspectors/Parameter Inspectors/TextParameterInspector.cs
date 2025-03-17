﻿using Rusty.ISA;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor
{
    /// <summary>
    /// A line parameter inspector.
    /// </summary>
    public partial class TextParameterInspector : ParameterInspector
    {
        /* Public properties. */
        /// <summary>
        /// The parameter definition visualized by this inspector.
        /// </summary>
        public new TextParameter Definition
        {
            get => base.Definition as TextParameter;
            set => base.Definition = value;
        }
        /// <summary>
        /// The value of the stored parameter.
        /// </summary>
        public string Value
        {
            get => LineField.Value;
            set => LineField.Value = value;
        }
        public override object ValueObj
        {
            get => Value;
            set => Value = (string)value;
        }

        /* Private properties. */
        private LineField LineField { get; set; }

        /* Constructors. */
        public TextParameterInspector() : base() { }

        public TextParameterInspector(InstructionSet instructionSet, TextParameter parameter)
            : base(instructionSet, parameter)
        {
            LineField.Value = parameter.DefaultValue;
        }

        public TextParameterInspector(TextParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new TextParameterInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is TextParameterInspector otherInspector)
            {
                LineField = GetAt(0) as LineField;
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
            Name = $"TextParameter ({Definition.ID})";

            // Add line field.
            LineField = new();
            LineField.LabelText = Definition.DisplayName;
            Add(LineField);
        }
    }
}