using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    public abstract class InstructionCompiler : Compiler
    {
        /* Public methods. */
        public static SubNode<NodeData> Compile(InstructionInspector inspector)
        {
            // Root instruction.
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = inspector.Definition;
            InstructionInstance instance = new(definition);

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

            SubNode<NodeData> node = new(instance.ToString(), new(set, definition, instance));

            // Compile rules.
            for (int i = 0; i < definition.PreInstructions.Length; i++)
            {
                try
                {
                    Inspector ruleInspector = inspector.GetCompileRuleInspector(i);
                    node.AddChild(RuleCompiler.Compile(ruleInspector));
                }
                catch { }
            }

            return node;
        }
    }
}