using System.Collections.Generic;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor
{
    public abstract partial class SecondaryInstructionsInspector : Inspector
    {
        /* Public properties. */
        public InstructionInspector Root { get; private set; }
        public List<CompileRuleInspector> Inspectors { get; set; } = new();

        /* Constructors. */
        public SecondaryInstructionsInspector() : base() { }

        public SecondaryInstructionsInspector(InstructionInspector root, InstructionDefinition definition)
            : base(root.InstructionSet, definition)
        {
            Root = root;
        }

        /* Public methods. */
        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is SecondaryInstructionsInspector otherInspector)
            {
                // Copy root.
                Root = otherInspector.Root;

                // Clear current refences.
                Inspectors.Clear();

                // Find inspectors.
                for (int i = 0; i < Count; i++)
                {
                    Inspectors.Add(this[i] as CompileRuleInspector);
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
                CompileRuleInspector inspector = CompileRuleInspector.Create(Root, compileRule);
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