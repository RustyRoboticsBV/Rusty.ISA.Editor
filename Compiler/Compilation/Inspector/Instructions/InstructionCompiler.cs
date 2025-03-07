using System.Collections.Generic;
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
        /// <summary>
        /// Compile an instruction inspector into a compile node hierarchy.
        /// </summary>
        public static SubNode<NodeData>[] Compile(InstructionInspector inspector)
        {
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = inspector.Definition;
            InstructionInstance instance = new(definition);

            List<SubNode<NodeData>> results = new();

            // Pre-instructions.
            if (definition.PreInstructions.Length > 0)
            {
                SubNode<NodeData> group = CompilerNodeMaker.GetPreInstructionBlock(set);
                results.Add(group);

                for (int i = 0; i < definition.PreInstructions.Length; i++)
                {
                    try
                    {
                        Inspector ruleInspector = inspector.GetPreInstructionInspector(i);
                        group.AddChildren(RuleCompiler.Compile(ruleInspector));
                    }
                    catch { }
                }

                group.AddChild(CompilerNodeMaker.GetEndOfGroup(set));
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

            results.Add(CompilerNodeMaker.GetInstruction(set, instance));

            // Post-instructions.
            if (definition.PostInstructions.Length > 0)
            {
                SubNode<NodeData> group = CompilerNodeMaker.GetPostInstructionBlock(set);
                results.Add(group);

                for (int i = 0; i < definition.PostInstructions.Length; i++)
                {
                    try
                    {
                        Inspector ruleInspector = inspector.GetPostInstructionInspector(i);
                        group.AddChildren(RuleCompiler.Compile(ruleInspector));
                    }
                    catch { }
                }

                group.AddChild(CompilerNodeMaker.GetEndOfGroup(set));
            }

            // Return node(s).
            return results.ToArray();
        }
    }
}