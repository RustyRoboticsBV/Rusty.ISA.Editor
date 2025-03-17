using System.Collections.Generic;
using Rusty.ISA;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor
{
    public abstract partial class SecondaryInstructionsInspector : Inspector
    {
        /* Public properties. */
        public List<Inspector> Inspectors { get; set; } = new();

        /* Constructors. */
        public SecondaryInstructionsInspector() : base() { }

        public SecondaryInstructionsInspector(InstructionSet instructionSet, InstructionDefinition definition)
            : base(instructionSet, definition) { }

        /* Public methods. */
        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is SecondaryInstructionsInspector otherInspector)
            {
                // Clear current refences.
                Inspectors.Clear();

                // Find inspectors.
                for (int i = 0; i < Count; i++)
                {
                    if (this[i] is CompileRuleInspector container)
                        Inspectors.Add(container);
                    else if (this[i] is InstructionRuleInspector instruction)
                        Inspectors.Add(instruction);
                }

                // Hide or show based on if we have contents or not.
                if (Inspectors.Count == 0)
                    Hide();
                else
                    Show();

                return true;
            }
            return false;
        }

        /// <summary>
        /// Fill this secondary instructions inspector with sub-inspectors.
        /// </summary>
        public void Populate(CompileRule[] rules)
        {
            foreach (CompileRule compileRule in rules)
            {
                Inspector inspector = CompileRuleInspector.Create(InstructionSet, compileRule);
                Inspectors.Add(inspector);
                Add(inspector);
            }

            if (rules.Length == 0)
                Hide();
            else
                Show();
        }

        /* Protected methods. */
        protected override void Init()
        {
            base.Init();

            Name = "Secondary Instructions";
        }
    }
}