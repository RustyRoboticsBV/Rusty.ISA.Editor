using System.Collections.Generic;
using Rusty.Cutscenes;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// Decompiles a list of cutscene instructions and outputs a compiler graph.
    /// </summary>
    public abstract class InstructionListDecompiler
    {
        /* Public methods. */
        /// <summary>
        /// Decompiles a list of cutscene instructions and outputs a compiler graph.
        /// </summary>
        public static CompilerGraph Decompile(InstructionSet set, List<InstructionInstance> instructions)
        {
            // Create node graph.
            CompilerGraph graph = new();
            Dictionary<string, CompilerNode> labelTable = new();
            int index = 0;
            while (index < instructions.Count)
            {
                // Create node or node hierarchy from instruction.
                switch (instructions[index].Opcode)
                {
                    case BuiltIn.NodeOpcode:
                    case BuiltIn.InstructorInspectorOpcode:
                    case BuiltIn.OptionRuleOpcode:
                    case BuiltIn.ChoiceRuleOpcode:
                    case BuiltIn.TupleRuleOpcode:
                    case BuiltIn.ListRuleOpcode:
                        graph.AddNode(HandleCollection(set, instructions, ref index));
                        break;
                    default:
                        graph.AddNode(CompilerNodeMaker.GetInstruction(set, instructions[index]));
                        index++;
                        break;
                }

                // Add to label table.
                try
                {
                    CompilerNode node = graph[^1] as CompilerNode;
                    string label = node.GetLabel().Data.GetArgument(BuiltIn.LabelNameId);
                    labelTable.Add(label, node);
                }
                catch { }

                index++;
            }

            // Connect nodes.
            for (int i = 0; i < graph.Count; i++)
            {
                CompilerNode node = graph[i] as CompilerNode;
                OutputData outputData = node.GetOutputData();

                // Connect default output.
                if (outputData.HasDefaultOutput && i < graph.Count - 1)
                    node.ConnectTo(graph[i + 1]);

                // Connect argument outputs.
                for (int j = 0; j < outputData.ArgumentCount; j++)
                {
                    // Get target node label.
                    string label = outputData.GetOutput(j).ArgumentValue;

                    // Find referenced node and connect to it.
                    try
                    {
                        node.ConnectTo(labelTable[label]);
                    }
                    catch { }
                }
            }

            return graph;
        }

        /* Private methods. */
        /// <summary>
        /// Convert a collection instruction into a compiler node.
        /// </summary>
        private static CompilerNode HandleCollection(InstructionSet set, List<InstructionInstance> instructions, ref int index)
        {
            // Handle header.
            CompilerNode node = CompilerNodeMaker.GetInstruction(set, instructions[index]);
            index++;

            // Handle subsequent nodes.
            while (index < instructions.Count)
            {
                switch (instructions[index].Opcode)
                {
                    case BuiltIn.NodeOpcode:
                    case BuiltIn.InstructorInspectorOpcode:
                    case BuiltIn.OptionRuleOpcode:
                    case BuiltIn.ChoiceRuleOpcode:
                    case BuiltIn.TupleRuleOpcode:
                    case BuiltIn.ListRuleOpcode:
                        node.AddChild(HandleCollection(set, instructions, ref index));
                        break;
                    case BuiltIn.EndOfBlockOpcode:
                        node.AddChild(CompilerNodeMaker.GetInstruction(set, instructions[index]));
                        return node;
                    default:
                        node.AddChild(CompilerNodeMaker.GetInstruction(set, instructions[index]));
                        break;
                }

                index++;
            }

            // This shouldn't happen.
            return node;
        }
    }
}