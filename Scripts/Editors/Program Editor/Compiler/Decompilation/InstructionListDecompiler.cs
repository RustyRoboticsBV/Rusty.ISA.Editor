using System;
using System.Collections.Generic;
using Godot;
using Rusty.ISA;

namespace Rusty.ISA.Editor.Programs.Compiler
{
    /// <summary>
    /// Decompiles a list of instructions and outputs a compiler graph.
    /// </summary>
    public abstract class InstructionListDecompiler
    {
        /* Public methods. */
        /// <summary>
        /// Decompiles a list of instructions and outputs a compiler graph.
        /// </summary>
        public static CompilerGraph Decompile(InstructionSet set, List<InstructionInstance> instructions)
        {
            // Create node graph.
            CompilerGraph graph = new();
            Dictionary<string, CompilerNode> labelTable = new();
            int index = 0;
            while (index < instructions.Count)
            {
                // Create a node or node hierarchy from the instruction and possibly subsequent instructions.
                switch (instructions[index].Opcode)
                {
                    case BuiltIn.InstructionSetOpcode:
                    case BuiltIn.CommentOpcode:
                    case BuiltIn.FrameOpcode:
                    case BuiltIn.NodeOpcode:
                    case BuiltIn.GotoGroupOpcode:
                    case BuiltIn.EndGroupOpcode:
                        graph.AddNode(HandleCollection(set, instructions, ref index));
                        break;
                    default:
                        GD.PrintErr($"Encountered illegal top-level instruction with opcode '{instructions[index].Opcode}'. The "
                            + "instruction was ignored.");
                        break;
                }

                // Add to label table.
                try
                {
                    CompilerNode node = graph[^1];
                    string label = node.GetLabel().Data.GetArgument(BuiltIn.LabelName);
                    labelTable.Add(label, node);
                }
                catch { }

                index++;
            }

            // Connect nodes.
            for (int i = 0; i < graph.Count; i++)
            {
                CompilerNode node = graph[i];
                OutputData outputData = node.GetOutputData();

                // Don't attempt to connect instruction set, frame, comment and end group nodes.
                if (node.Data.GetOpcode() == BuiltIn.InstructionSetOpcode
                    || node.Data.GetOpcode() == BuiltIn.FrameOpcode
                    || node.Data.GetOpcode() == BuiltIn.CommentOpcode
                    || node.Data.GetOpcode() == BuiltIn.EndGroupOpcode)
                {
                    continue;
                }

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
            CompilerNode node = CompilerNodeMaker.GetAny(set, instructions[index]);
            index++;

            // Handle subsequent nodes.
            while (index < instructions.Count)
            {
                switch (instructions[index].Opcode)
                {
                    case BuiltIn.InstructionSetOpcode:
                    case BuiltIn.DefinitionOpcode:
                    case BuiltIn.CompileRuleOpcode:
                    case BuiltIn.CommentOpcode:
                    case BuiltIn.FrameOpcode:
                    case BuiltIn.NodeOpcode:
                    case BuiltIn.InspectorOpcode:
                    case BuiltIn.PreInstructionOpcode:
                    case BuiltIn.PostInstructionOpcode:
                    case BuiltIn.OptionRuleOpcode:
                    case BuiltIn.ChoiceRuleOpcode:
                    case BuiltIn.TupleRuleOpcode:
                    case BuiltIn.ListRuleOpcode:
                    case BuiltIn.GotoGroupOpcode:
                    case BuiltIn.EndGroupOpcode:
                        node.AddChild(HandleCollection(set, instructions, ref index));
                        break;
                    case BuiltIn.EndOfGroupOpcode:
                        node.AddChild(CompilerNodeMaker.GetAny(set, instructions[index]));
                        return node;
                    default:
                        node.AddChild(CompilerNodeMaker.GetAny(set, instructions[index]));
                        break;
                }

                index++;
            }

            // This shouldn't happen.
            return node;
        }
    }
}