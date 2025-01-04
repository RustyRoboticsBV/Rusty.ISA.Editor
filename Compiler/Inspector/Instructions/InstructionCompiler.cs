using Godot;
using Rusty.Cutscenes;
using Rusty.Graphs;

namespace Rusty.CutsceneEditor.Compiler
{
    public abstract class InstructionCompiler : InspectorCompiler
    {
        /* Protected methods. */
        protected static SubNode<NodeData> GetInstruction(InstructionInspector inspector)
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
                    GD.Print(inspector.Name + ": parameter " + definition.Parameters[i].Id + ": " + inspector.GetParameterInspector(i).ValueObj.ToString());
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

        protected static RootNode<NodeData> GetNode(NodeInstructionInspector inspector)
        {
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = set[BuiltIn.NodeOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.NodeXId)] = "0";
            instance.Arguments[definition.GetParameterIndex(BuiltIn.NodeYId)] = "0";

            return new(instance.ToString(), new(set, definition, instance));
        }

        protected static SubNode<NodeData> GetLabel(NodeInstructionInspector inspector)
        {
            InstructionSet set = inspector.InstructionSet;
            InstructionDefinition definition = set[BuiltIn.LabelOpcode];
            InstructionInstance instance = new(definition);

            instance.Arguments[definition.GetParameterIndex(BuiltIn.LabelNameId)] = inspector.LabelName;

            return new(instance.ToString(), new(set, definition, instance));
        }
    }
}