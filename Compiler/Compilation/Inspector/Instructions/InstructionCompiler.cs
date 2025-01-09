using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// A compiler that converts instruction inspectors to graph nodes,
    /// </summary>
    public abstract class InstructionCompiler : Compiler
    {
        /* Public methods. */
        public static SubNode<NodeData> Compile(InstructionInspector inspector)
        {
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = inspector.Definition;
            InstructionInstance instance = new(definition);

            // Pre-instruction group.
            SubNode<NodeData> preInstructionGroup = GetPreInstructionGroup(inspector);

            // Compile rules.
            for (int i = 0; i < definition.PreInstructions.Length; i++)
            {
                try
                {
                    Inspector ruleInspector = inspector.GetCompileRuleInspector(i);
                    preInstructionGroup.AddChild(RuleCompiler.Compile(ruleInspector));
                }
                catch { }
            }

            // Main instruction.
            for (int i = 0; i < definition.Parameters.Length; i++)
            {
                if (definition.Parameters[i] is OutputParameter)
                    continue;

                try
                {
                    instance.Arguments[i] = inspector.GetParameterInspector(i).ValueObj.ToString();
                }
                catch { }
            }

            preInstructionGroup.AddChild(new(instance.ToString(), new(set, definition, instance)));

            // End of pre-instruction group.
            preInstructionGroup.AddChild(GetEndOfBlock(inspector.InstructionSet));

            return preInstructionGroup;
        }

        /* Private methods. */
        private static SubNode<NodeData> GetPreInstructionGroup(Inspector inspector)
        {
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = set[BuiltIn.PreInstructionGroupOpcode];
            InstructionInstance instance = new(definition);

            return new(instance.ToString(), new(set, definition, instance));
        }
    }
}