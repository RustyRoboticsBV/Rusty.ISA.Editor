using System.Collections.Generic;
using Godot;
using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A pre-instruction inspector.
    /// </summary>
    public partial class PreInstructionInspector : InstructionInspector
    {
        /* Constructors. */
        public PreInstructionInspector() : base() { }

        public PreInstructionInspector(InstructionSet instructionSet, InstructionDefinition resource)
            : base(instructionSet, resource) { }

        public PreInstructionInspector(PreInstructionInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new PreInstructionInspector(this);
        }
    }
}