using Rusty.Graphs;

namespace Rusty.ISA.ProgramEditor.Compiler
{
    /// <summary>
    /// An inspector node decompiler.
    /// </summary>
    public abstract class InspectorDecompiler
    {
        /// <summary>
        /// Apply an inspector node to an instruction inspector.
        /// </summary>
        public static void Apply(Node<NodeData> inspectorNode, InstructionInspector instructionInspector)
        {
            SubNode<NodeData> preInstructions = null;
            SubNode<NodeData> postInstructions = null;
            SubNode<NodeData> mainInstruction = null;
            foreach (SubNode<NodeData> child in inspectorNode.Children)
            {
                switch (child.Data.GetOpcode())
                {
                    case BuiltIn.PreInstructionOpcode:
                        if (preInstructions == null)
                            preInstructions = child;
                        break;
                    case BuiltIn.PostInstructionOpcode:
                        if (postInstructions == null)
                            postInstructions = child;
                        break;
                    case BuiltIn.EndOfGroupOpcode:
                        break;
                    default:
                        if (mainInstruction == null)
                            mainInstruction = child;
                        break;
                }
            }

            if (mainInstruction != null)
                InstructionDecompiler.Apply(mainInstruction, instructionInspector);
            if (preInstructions != null)
                SecondaryInstructionsDecompiler.Apply(preInstructions, instructionInspector.PreInstructions);
            if (postInstructions != null)
                SecondaryInstructionsDecompiler.Apply(postInstructions, instructionInspector.PostInstructions);
        }
    }
}