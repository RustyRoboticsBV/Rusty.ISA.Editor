namespace Rusty.ISA.Editor;

/// <summary>
/// A node element compiler.
/// </summary>
public abstract class NodeCompiler : Compiler
{
    /* Public methods. */
    public static RootNode Compile(InstructionSet set, GraphNode element, NodeInspector inspector)
    {
        RootNode node = MakeRoot(set, BuiltIn.NodeOpcode);
        node.SetArgument(BuiltIn.NodeX, (int)element.PositionOffset.X);
        node.SetArgument(BuiltIn.NodeY, (int)element.PositionOffset.Y);

        // Compile begin.
        ToggleTextField startPoint = inspector.GetStartPointField();
        if (startPoint.Checked)
        {
            SubNode begin = MakeSub(set, BuiltIn.BeginOpcode);
            begin.SetArgument(BuiltIn.BeginName, startPoint.Value);
            node.AddChild(begin);
        }

        // Compile frame member.
        if (element.Frame != null)
        {
            SubNode frameMember = MakeSub(set, BuiltIn.FrameMemberOpcode);
            frameMember.SetArgument(BuiltIn.FrameMemberID, element.Frame.ID);
            node.AddChild(frameMember);
        }

        // Compile instruction inspector.
        SubNode instructionInspector = CompileInstruction(set, inspector.GetInstructionInspector());
        node.AddChild(instructionInspector);

        // Compile end-of-group.
        node.AddChild(EndOfGroup(set));

        return node;
    }

    /* Private methods. */
    private static SubNode CompileInstruction(InstructionSet set, InstructionInspector inspector)
    {
        // Compile inspector header.
        SubNode instructionInspector = MakeSub(set, BuiltIn.InspectorOpcode);

        // Compile pre-instructions.
        if (inspector.Definition.PreInstructions.Length > 0)
        {
            // Compile pre-instruction header.
            SubNode preIntructions = MakeSub(set, BuiltIn.PreInstructionOpcode);

            // Compile pre-instructions.
            for (int i = 0; i < inspector.Definition.PreInstructions.Length; i++)
            {
                string id = inspector.Definition.PreInstructions[i].ID;
                SubNode preInstruction = CompileRule(set, inspector.GetPreInstruction(id));
                preIntructions.AddChild(preInstruction);
            }

            // Compile end-of-group.
            preIntructions.AddChild(EndOfGroup(set));

            instructionInspector.AddChild(preIntructions);
        }

        // Compile contents.
        SubNode contents = MakeSub(set, inspector.Definition.Opcode);
        for (int i = 0; i < inspector.Definition.Parameters.Length; i++)
        {
            string id = inspector.Definition.Parameters[i].ID;
            IField parameter = inspector.GetParameterField(id);
            if (parameter != null)
                contents.SetArgument(id, parameter.Value);
        }
        instructionInspector.AddChild(contents);

        // Compile post-instructions.
        if (inspector.Definition.PostInstructions.Length > 0)
        {
            // Compile post-instruction header.
            SubNode postIntructions = MakeSub(set, BuiltIn.PostInstructionOpcode);

            // Compile post-instructions.
            for (int i = 0; i < inspector.Definition.PostInstructions.Length; i++)
            {
                string id = inspector.Definition.PostInstructions[i].ID;
                SubNode postInstruction = CompileRule(set, inspector.GetPostInstruction(id));
                postIntructions.AddChild(postInstruction);
            }

            // Compile end-of-group.
            postIntructions.AddChild(EndOfGroup(set));

            instructionInspector.AddChild(postIntructions);
        }

        // Compile end-of-group.
        instructionInspector.AddChild(EndOfGroup(set));

        return instructionInspector;
    }

    private static SubNode CompileRule(InstructionSet set, RuleInspector inspector)
    {
        switch (inspector)
        {
            case InstructionRuleInspector i:
                return CompileInstruction(set, i.GetInstructionInspector());
            case OptionRuleInspector o:
                return CompileOption(set, inspector as OptionRuleInspector);
            case ChoiceRuleInspector c:
                return CompileChoice(set, inspector as ChoiceRuleInspector);
            case TupleRuleInspector t:
                return CompileTuple(set, inspector as TupleRuleInspector);
            case ListRuleInspector l:
                return CompileList(set, inspector as ListRuleInspector);
            default:
                return null;
        }
    }

    private static SubNode CompileOption(InstructionSet set, OptionRuleInspector inspector)
    {
        // Compile option header.
        SubNode option = MakeSub(set, BuiltIn.OptionRuleOpcode);

        // Compile element.
        if (inspector.GetEnabled())
        {
            SubNode element = CompileRule(set, inspector.GetChildRule());
            option.AddChild(element);
        }

        // Compile end-of-group.
        option.AddChild(EndOfGroup(set));

        return option;
    }

    private static SubNode CompileChoice(InstructionSet set, ChoiceRuleInspector inspector)
    {
        // Compile option header.
        SubNode choice = MakeSub(set, BuiltIn.ChoiceRuleOpcode);
        int selectedIndex = inspector.GetSelectedIndex();
        string selectedID = inspector.Rule.Types[selectedIndex].ID;
        choice.SetArgument(BuiltIn.ChoiceRuleSelected, selectedID);

        // Compile selected element.
        SubNode element = CompileRule(set, inspector.GetSelectedElement());
        choice.AddChild(element);

        // Compile end-of-group.
        choice.AddChild(EndOfGroup(set));

        return choice;
    }

    private static SubNode CompileTuple(InstructionSet set, TupleRuleInspector inspector)
    {
        // Compile option header.
        SubNode tuple = MakeSub(set, BuiltIn.TupleRuleOpcode);

        // Compile elements.
        for (int i = 0; i < inspector.GetElementCount(); i++)
        {
            SubNode element = CompileRule(set, inspector.GetElementInspector(i));
            tuple.AddChild(element);
        }

        // Compile end-of-group.
        tuple.AddChild(EndOfGroup(set));

        return tuple;
    }

    private static SubNode CompileList(InstructionSet set, ListRuleInspector inspector)
    {
        // Compile list header.
        SubNode list = MakeSub(set, BuiltIn.ListRuleOpcode);

        // Compile elements.
        for (int i = 0; i < inspector.GetElementCount(); i++)
        {
            SubNode element = CompileRule(set, inspector.GetElementInspector(i));
            list.AddChild(element);
        }

        // Compile end-of-group.
        list.AddChild(EndOfGroup(set));

        return list;
    }

    private static SubNode EndOfGroup(InstructionSet set)
    {
        return MakeSub(set, BuiltIn.EndOfGroupOpcode);
    }
}