using Godot;
using System.Collections;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A node program unit.
/// </summary>
public sealed class NodeUnit : Unit
{
    /* Public properties. */
    public new GraphNode Element => base.Element as GraphNode;
    public new NodeInspector Inspector => base.Inspector as NodeInspector;

    /* Constructors. */
    public NodeUnit(InstructionSet set, string opcode, GraphNode element, Inspector inspector)
        : base(set, opcode, element, inspector) { }

    /* Public methods. */
    public override RootNode Compile()
    {
        RootNode node = CompilerNodeMaker.MakeRoot(Set, BuiltIn.NodeOpcode);
        node.SetArgument(BuiltIn.NodeX, (int)Element.PositionOffset.X);
        node.SetArgument(BuiltIn.NodeY, (int)Element.PositionOffset.Y);

        // Compile begin.
        ToggleTextField startPoint = Inspector.GetStartPointField();
        if (startPoint.Checked)
        {
            SubNode begin = CompilerNodeMaker.MakeSub(Set, BuiltIn.BeginOpcode);
            begin.SetArgument(BuiltIn.BeginName, startPoint.Value);
            node.AddChild(begin);
        }

        // Compile frame member.
        if (Element.Frame != null)
        {
            SubNode frameMember = CompilerNodeMaker.MakeSub(Set, BuiltIn.FrameMemberOpcode);
            frameMember.SetArgument(BuiltIn.FrameMemberID, Element.Frame.ID);
            node.AddChild(frameMember);
        }

        // Compile instruction inspector.
        SubNode instructionInspector = CompileInstruction(Inspector.GetInstructionInspector());
        GD.Print(instructionInspector);
        node.AddChild(instructionInspector);

        // Compile end-of-group.
        node.AddChild(EndOfGroup());

        return node;
    }

    /* Private methods. */
    private SubNode CompileInstruction(InstructionInspector inspector)
    {
        // Compile inspector header.
        SubNode instructionInspector = CompilerNodeMaker.MakeSub(Set, BuiltIn.InspectorOpcode);

        // Compile pre-instructions.
        if (inspector.Definition.PreInstructions.Length > 0)
        {
            // Compile pre-instruction header.
            SubNode preIntructions = CompilerNodeMaker.MakeSub(Set, BuiltIn.PreInstructionOpcode);

            // Compile pre-instructions.
            for (int i = 0; i < inspector.Definition.PreInstructions.Length; i++)
            {
                string id = inspector.Definition.PreInstructions[i].ID;
                SubNode preInstruction = CompileRule(inspector.GetPreInstruction(id));
                preIntructions.AddChild(preInstruction);
            }

            // Compile end-of-group.
            preIntructions.AddChild(EndOfGroup());

            instructionInspector.AddChild(preIntructions);
        }

        // Compile contents.
        SubNode contents = CompilerNodeMaker.MakeSub(Set, inspector.Definition.Opcode);
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
            SubNode postIntructions = CompilerNodeMaker.MakeSub(Set, BuiltIn.PostInstructionOpcode);

            // Compile post-instructions.
            for (int i = 0; i < inspector.Definition.PostInstructions.Length; i++)
            {
                string id = inspector.Definition.PostInstructions[i].ID;
                SubNode postInstruction = CompileRule(inspector.GetPostInstruction(id));
                postIntructions.AddChild(postInstruction);
            }

            // Compile end-of-group.
            postIntructions.AddChild(EndOfGroup());

            instructionInspector.AddChild(postIntructions);
        }

        // Compile end-of-group.
        instructionInspector.AddChild(EndOfGroup());

        return instructionInspector;
    }

    private SubNode CompileRule(RuleInspector inspector)
    {
        switch (inspector)
        {
            case InstructionRuleInspector i:
                return CompileInstruction(i.GetInstructionInspector());
            case OptionRuleInspector o:
                return CompileOption(inspector as OptionRuleInspector);
            case ChoiceRuleInspector c:
                return CompileChoice(inspector as ChoiceRuleInspector);
            case TupleRuleInspector t:
                return CompileTuple(inspector as TupleRuleInspector);
            case ListRuleInspector l:
                return CompileList(inspector as ListRuleInspector);
            default:
                return null;
        }
    }

    private SubNode CompileOption(OptionRuleInspector inspector)
    {
        // Compile option header.
        SubNode option = CompilerNodeMaker.MakeSub(Set, BuiltIn.OptionRuleOpcode);

        // Compile element.
        if (inspector.GetEnabled())
        {
            SubNode element = CompileRule(inspector.GetChildRule());
            option.AddChild(element);
        }

        // Compile end-of-group.
        option.AddChild(EndOfGroup());

        return option;
    }

    private SubNode CompileChoice(ChoiceRuleInspector inspector)
    {
        // Compile option header.
        SubNode choice = CompilerNodeMaker.MakeSub(Set, BuiltIn.ChoiceRuleOpcode);
        int selectedIndex = inspector.GetSelectedIndex();
        string selectedID = inspector.Rule.Types[selectedIndex].ID;
        choice.SetArgument(BuiltIn.ChoiceRuleSelected, selectedID);

        // Compile selected element.
        SubNode element = CompileRule(inspector.GetSelectedElement());
        choice.AddChild(element);

        // Compile end-of-group.
        choice.AddChild(EndOfGroup());

        return choice;
    }

    private SubNode CompileTuple(TupleRuleInspector inspector)
    {
        // Compile option header.
        SubNode tuple = CompilerNodeMaker.MakeSub(Set, BuiltIn.TupleRuleOpcode);

        // Compile elements.
        for (int i = 0; i < inspector.GetElementCount(); i++)
        {
            SubNode element = CompileRule(inspector.GetElementInspector(i));
            tuple.AddChild(element);
        }

        // Compile end-of-group.
        tuple.AddChild(EndOfGroup());

        return tuple;
    }

    private SubNode CompileList(ListRuleInspector inspector)
    {
        // Compile list header.
        SubNode list = CompilerNodeMaker.MakeSub(Set, BuiltIn.ListRuleOpcode);

        // Compile elements.
        for (int i = 0; i < inspector.GetElementCount(); i++)
        {
            SubNode element = CompileRule(inspector.GetElementInspector(i));
            list.AddChild(element);
        }

        // Compile end-of-group.
        list.AddChild(EndOfGroup());

        return list;
    }

    private SubNode EndOfGroup()
    {
        return CompilerNodeMaker.MakeSub(Set, BuiltIn.EndOfGroupOpcode);
    }
}