using System.Reflection;

namespace Rusty.ISA.Editor;

/// <summary>
/// A metadata compiler.
/// </summary>
public abstract class MetadataCompiler : CompilerTool
{
    /* Public methods. */
    /// <summary>
    /// Create a metadata block for an instruction set.
    /// </summary>
    public static RootNode Compile(Ledger ledger)
    {
        InstructionSet instructionSet = ledger.Set;

        // Create metadata block.
        RootNode metadata = MakeRoot(instructionSet, BuiltIn.MetadataOpcode);

        // Create checksum (but don't set its value yet).
        SubNode checksum = MakeSub(instructionSet, BuiltIn.ChecksumOpcode);
        metadata.AddChild(checksum);

        // Create ISA block.
        SubNode isa = MakeSub(instructionSet, BuiltIn.InstructionSetOpcode);
        for (int i = 0; i < instructionSet.Count; i++)
        {
            // Create instruction definition block.
            isa.AddChild(ProcessDefinition(instructionSet, instructionSet[i].Opcode));
        }
        isa.AddChild(MakeSub(instructionSet, BuiltIn.EndOfGroupOpcode));
        metadata.AddChild(isa);

        // Add end-of-group.
        metadata.AddChild(MakeSub(instructionSet, BuiltIn.EndOfGroupOpcode));

        return metadata;
    }

    /* Private methods. */
    /// <summary>
    /// Generate a sub-node hierarchy for an instruction definition.
    /// </summary>
    private static SubNode ProcessDefinition(InstructionSet set, string opcode)
    {
        InstructionDefinition instruction = set[opcode];

        // Instruction definition header.
        SubNode definition = MakeSub(set, BuiltIn.DefinitionOpcode);
        definition.SetArgument(BuiltIn.DefinitionOpcodeParameter, opcode);
        definition.SetArgument(BuiltIn.DefinitionEditorOnly, instruction.Implementation == null);

        // Parameters.
        for (int i = 0; i < instruction.Parameters.Length; i++)
        {
            definition.AddChild(ProcessParameter(set, instruction.Parameters[i]));
        }

        // Pre-instructions.
        if (instruction.PreInstructions.Length > 0)
        {
            SubNode pre = MakeSub(set, BuiltIn.PreInstructionOpcode);
            foreach (CompileRule rule in instruction.PreInstructions)
            {
                pre.AddChild(ProcessRule(set, rule));
            }
            pre.AddChild(MakeRoot(set, BuiltIn.EndOfGroupOpcode));
            definition.AddChild(pre);
        }

        // Post-instructions.
        if (instruction.PostInstructions.Length > 0)
        {
            SubNode post = MakeSub(set, BuiltIn.PostInstructionOpcode);
            foreach (CompileRule rule in instruction.PostInstructions)
            {
                post.AddChild(ProcessRule(set, rule));
            }
            post.AddChild(MakeRoot(set, BuiltIn.EndOfGroupOpcode));
            definition.AddChild(post);
        }

        // End-of-group.
        definition.AddChild(MakeSub(set, BuiltIn.EndOfGroupOpcode));

        return definition;
    }

    /// <summary>
    /// Generate a sub-node for a parameter.
    /// </summary>
    private static SubNode ProcessParameter(InstructionSet set, Parameter parameter)
    {
        SubNode definition = MakeSub(set, BuiltIn.ParameterOpcode);
        definition.SetArgument(BuiltIn.ParameterType, parameter.GetType().GetCustomAttribute<XmlClassAttribute>().XmlKeyword);
        definition.SetArgument(BuiltIn.ParameterID, parameter.ID);
        return definition;
    }

    /// <summary>
    /// Generate a sub-node / sub-node hierarchy for a compile rule.
    /// </summary>
    private static SubNode ProcessRule(InstructionSet set, CompileRule rule)
    {
        if (rule is InstructionRule instruction)
        {
            SubNode reference = MakeSub(set, BuiltIn.ReferenceOpcode);
            reference.SetArgument(BuiltIn.ReferenceID, rule.ID);
            reference.SetArgument(BuiltIn.ReferenceOpcodeParameter, instruction.Opcode);
            return reference;
        }
        else
        {
            SubNode definition = MakeSub(set, BuiltIn.CompileRuleOpcode);
            definition.SetArgument(BuiltIn.CompileRuleType, rule.GetType().GetCustomAttribute<XmlClassAttribute>().XmlKeyword);
            definition.SetArgument(BuiltIn.CompileRuleID, rule.ID);
            switch (rule)
            {
                case OptionRule option:
                    if (option.Type != null)
                        definition.AddChild(ProcessRule(set, option.Type));
                    break;
                case ChoiceRule choice:
                    foreach (CompileRule type in choice.Types)
                    {
                        if (type != null)
                            definition.AddChild(ProcessRule(set, type));
                    }
                    break;
                case TupleRule tuple:
                    foreach (CompileRule type in tuple.Types)
                    {
                        if (type != null)
                            definition.AddChild(ProcessRule(set, type));
                    }
                    break;
                case ListRule list:
                    if (list.Type != null)
                        definition.AddChild(ProcessRule(set, list.Type));
                    break;
            }
            definition.AddChild(MakeSub(set, BuiltIn.EndOfGroupOpcode));
            return definition;
        }
    }
}