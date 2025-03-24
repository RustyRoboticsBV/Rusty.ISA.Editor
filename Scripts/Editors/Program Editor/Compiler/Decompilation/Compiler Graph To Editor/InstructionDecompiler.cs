using Godot;
using Rusty.Graphs;

namespace Rusty.ISA.Editor.Programs.Compiler
{
    /// <summary>
    /// A generic instruction node decompiler.
    /// </summary>
    public abstract class InstructionDecompiler : RuleDecompiler
    {
        /// <summary>
        /// Apply an instruction node to an instruction inspector. This ONLY touches the main instruction arguments, and does NOT
        /// handle the start point, pre-instructions and post-instructions!
        /// See the NodeDecompiler for the former, and InspectorDecompiler for the latter!
        /// </summary>
        public static void Apply(Node<NodeData> instructionNode, InstructionInspector instructionInspector)
        {
            // Make sure that the opcodes match.
            if (instructionNode.Data.GetOpcode() != instructionInspector.Definition.Opcode)
            {
                GD.PrintErr($"Tried to apply an instruction node with opcode '{instructionNode.Data.GetOpcode()}' to an "
                    + $"inspector with opcode '{instructionInspector.Definition.Opcode}'. This is not allowed!");
            }

            // Apply the instance arguments.
            instructionInspector.SetArguments(instructionNode.Data.Instance);
        }
    }
}