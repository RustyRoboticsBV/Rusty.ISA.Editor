using Godot;
using System.Collections.Generic;
using Rusty.Csv;
using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// Imports a cutscene program.
    /// </summary>
    public abstract class ProgramLoader
    {
        /* Public methods. */
        /// <summary>
        /// Load a cutscene program file as a cutscene program.
        /// </summary>
        public static void Import(CutsceneGraphEdit graphEdit, string code)
        {
            InstructionSet set = graphEdit.InstructionSet;

            // Decompile instructions.
            List<InstructionInstance> instructions = Decompile(set, code);

            // Create node graph.
            Graph<NodeData> graph = new();
            Dictionary<string, CompilerNode> labelTable = new();
            int index = 0;
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

            GD.Print(graph);
            return;
        }

        /* Private methods. */
        /// <summary>
        /// Decompile an instruction program code string into a list of instruction instances.
        /// </summary>
        private static List<InstructionInstance> Decompile(InstructionSet set, string code)
        {
            // Load as CSV table.
            CsvTable table = new CsvTable("Program", code);

            // Convert to instruction list.
            List<InstructionInstance> instructions = new();
            for (int instruction = 0; instruction < table.Height; instruction++)
            {
                // Get the opcode.
                string opcode = table.GetCell(0, instruction);

                // Get the arguments.
                int parameterCount = table.Width - 1;
                try
                {
                    parameterCount = set[opcode].Parameters.Length;
                }
                catch { }

                string[] arguments = new string[parameterCount];
                for (int arg = 0; arg < parameterCount; arg++)
                {
                    try
                    {
                        arguments[arg] = table.GetCell(arg + 1, instruction);
                    }
                    catch
                    {
                        arguments[arg] = "";
                    }
                }

                // Create instruction.
                instructions.Add(new InstructionInstance(opcode, arguments));
            }

            return instructions;
        }

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