using System;
using System.Collections.Generic;
using Godot;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor
{
    /// <summary>
    /// A instruction compile rule inspector.
    /// </summary>
    public abstract partial class CompileRuleInspector : Inspector
    {
        /* Public properties. */
        public CompileRule Definition => Resource as CompileRule;

        /// <summary>
        /// The root instruction inspector.
        /// </summary>
        public InstructionInspector Root { get; private set; }
        /// <summary>
        /// The preview generator for this inspector.
        /// </summary>
        public CompileRulePreview Preview { get; private set; } = new();
        /// <summary>
        /// Whether or not the preview was updated this loop.
        /// </summary>
        public bool UpdatedPreview { get; protected set; }

        /* Private properties. */
        private int LastLength { get; set; }

        /* Constructors. */
        public CompileRuleInspector(InstructionInspector root, CompileRule compileRule)
            : base(root.InstructionSet, compileRule)
        {
            Root = root;
            Init();
        }

        public CompileRuleInspector(CompileRuleInspector other)
            : this(other.Root, other.Definition)
        {
            CopyStateFrom(other);
        }

        /* Public methods. */
        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is CompileRuleInspector rule)
            {
                Root = rule.Root;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the child rule inspector(s) that are currently active, if any.
        /// </summary>
        public abstract CompileRuleInspector[] GetActiveSubInspectors();

        /// <summary>
        /// Create a compile rule inspector of some type.
        /// </summary>
        public static CompileRuleInspector Create(InstructionInspector root, CompileRule compileRule)
        {
            switch (compileRule)
            {
                case InstructionRule instructionRule:
                    return new InstructionRuleInspector(root, instructionRule);
                case OptionRule optionRule:
                    return new OptionRuleInspector(root, optionRule);
                case ChoiceRule choiceRule:
                    return new ChoiceRuleInspector(root, choiceRule);
                case TupleRule tupleRule:
                    return new TupleRuleInspector(root, tupleRule);
                case ListRule listRule:
                    return new ListRuleInspector(root, listRule);
                default:
                    throw new ArgumentException($"Compile rule '{compileRule}' has an illegal type '{compileRule.GetType().Name}'.");
            }
        }

        /// <summary>
        /// Get all output parameter inspectors associated with this compile rule inspector and its sub-inspectors.
        /// </summary>
        public virtual List<ParameterInspector> GetOutputs()
        {
            List<ParameterInspector> Outputs = new();
            CompileRuleInspector[] subInspectors = GetActiveSubInspectors();
            foreach (CompileRuleInspector rule in subInspectors)
            {
                List<ParameterInspector> subOutputs = rule.GetOutputs();
                foreach (ParameterInspector output in subOutputs)
                {
                    Outputs.Add(output);
                }
            }
            return Outputs;
        }

        /// <summary>
        /// Force-update the preview. This will not set UpdatedPreview to true.
        /// </summary>
        public void ForcePreviewUpdate()
        {
            Preview = new(this);
            GD.Print("   I'm compile rule " + Definition + " and my preview was force-updated to " + Preview.Evaluate());
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            base._Process(delta);

            // Check if we need to update the preview.
            UpdatedPreview = false;
            CompileRuleInspector[] subInspectors = GetActiveSubInspectors();

            // ... Either if the number of sub-inspectors has changed...
            if (subInspectors.Length != LastLength)
            {
                UpdatedPreview = true;
                LastLength = subInspectors.Length;
            }

            /// ... Or if one of the sub-inspectors was updated.
            for (int i = 0; i < subInspectors.Length; i++)
            {
                if (UpdatedPreview)
                    break;
                if (subInspectors[i].UpdatedPreview)
                {
                    UpdatedPreview = true;
                    break;
                }
            }

            // Update the preview if necessary.
            if (UpdatedPreview)
            {
                GD.Print("Hi I'm compile rule " + Definition + " and I'm updating my preview.");
                ForcePreviewUpdate();
            }
        }

        /* Protected methods. */
        protected new virtual void Init() { }
    }
}