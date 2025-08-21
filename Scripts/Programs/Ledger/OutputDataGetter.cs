namespace Rusty.ISA.Editor;

/// <summary>
/// An utility for getting output data from a program unit.
/// </summary>
public static class OutputDataGetter
{
    /* Public methods. */
    /// <summary>
    /// Get the output data of a program unit.
    /// </summary>
    public static OutputData GetOutputData(Inspector inspector)
    {
        OutputData outputs = new();
        CollectOutputs(ref outputs, inspector);
        return outputs;
    }

    /* Private methods. */
    private static void CollectOutputs(ref OutputData outputs, Inspector inspector)
    {
        switch (inspector)
        {
            case JointInspector:
                return;
            case FrameInspector:
            case CommentInspector:
                outputs.HasDefaultOutput = false;
                return;
            case NodeInspector node:
                CollectOutputs(ref outputs, node.GetInstructionInspector());
                return;
        }
    }

    private static void CollectOutputs(ref OutputData outputs, InstructionInspector inspector)
    {
        for (int i = 0; i < inspector.Definition.PreInstructions.Length; i++)
        {
            string id = inspector.Definition.PreInstructions[i].ID;
            CollectOutputs(ref outputs, inspector.GetPreInstruction(id));
        }

        for (int i = 0; i < inspector.Definition.Parameters.Length; i++)
        {
            string id = inspector.Definition.Parameters[i].ID;
            if (inspector.Definition.Parameters[i] is OutputParameter output)
                CollectOutput(ref outputs, inspector.GetParameterInspector(id) as OutputInspector);
        }

        for (int i = 0; i < inspector.Definition.PostInstructions.Length; i++)
        {
            string id = inspector.Definition.PostInstructions[i].ID;
            CollectOutputs(ref outputs, inspector.GetPostInstruction(id));
        }
    }

    private static void CollectOutput(ref OutputData outputs, OutputInspector inspector)
    {
        outputs.AddOutput(inspector.Preview.Evaluate(), inspector.Parameter.RemoveDefaultOutput);
    }

    private static void CollectOutputs(ref OutputData outputs, RuleInspector inspector)
    {
        switch (inspector)
        {
            case InstructionRuleInspector i:
                CollectOutputs(ref outputs, i.GetInstructionInspector());
                break;
            case OptionRuleInspector o:
                if (o.GetEnabled())
                    CollectOutputs(ref outputs, o.GetChildRule());
                break;
            case ChoiceRuleInspector c:
                CollectOutputs(ref outputs, c.GetSelectedElement());
                break;
            case TupleRuleInspector t:
                for (int i = 0; i < t.GetElementCount(); i++)
                {
                    CollectOutputs(ref outputs, t.GetElementInspector(i));
                }
                break;
            case ListRuleInspector l:
                for (int i = 0; i < l.GetElementCount(); i++)
                {
                    CollectOutputs(ref outputs, l.GetElementInspector(i));
                }
                break;
        }
    }
}