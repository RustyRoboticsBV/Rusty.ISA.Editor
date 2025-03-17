using Rusty.ISA;
using Rusty.Graphs;

namespace Rusty.ISA.Editor.Compiler
{
    /// <summary>
    /// A compiler that converts instruction inspectors to compiler nodes,
    /// </summary>
    public abstract class InspectorCompiler
    {
        /* Public methods. */
        /// <summary>
        /// Compile an instruction inspector into a compile node hierarchy.
        /// </summary>
        public static SubNode<NodeData> Compile(InstructionInspector inspector)
        {
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = inspector.Definition;

            // Create instruction instance.
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

            // Create inspector node.
            SubNode<NodeData> node = CompilerNodeMaker.GetInspector(set);

            // Add pre-instructions.
            if (inspector.PreInstructions.Inspectors.Count > 0)
                node.AddChild(PreInstructionsCompiler.Compile(inspector.PreInstructions));

            // Create instruction node.
            node.AddChild(CompilerNodeMaker.GetAny(set, instance));

            // Create post-instructions.
            if (inspector.PostInstructions.Inspectors.Count > 0)
                node.AddChild(PostInstructionsCompiler.Compile(inspector.PostInstructions));

            // Add end-of-group.
            node.AddChild(CompilerNodeMaker.GetEndOfGroup(set));

            // Return finished inspector node.
            return node;
        }
    }
}