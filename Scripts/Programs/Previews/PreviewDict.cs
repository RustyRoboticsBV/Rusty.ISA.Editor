using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A repository for all preview scripts in the program.
/// </summary>
public static class PreviewDict
{
    /* Private properties. */
    private static Dictionary<InstructionResource, Preview> Previews { get; } = new();

    /* Public methods. */
    /// <summary>
    /// Add a new preview for some resource.
    /// </summary>
    public static void Add(InstructionResource resource, Preview preview)
    {
        Previews.Add(resource, preview);
    }

    /// <summary>
    /// Check if there is a preview for some resource.
    /// </summary>
    public static bool Has(InstructionResource resource)
    {
        return Previews.ContainsKey(resource);
    }

    /// <summary>
    /// Delete all previews.
    /// </summary>
    public static void Clear()
    {
        Previews.Clear();
    }

    /// <summary>
    /// Get an instruction preview. Adds it if it didn't exist yet.
    /// </summary>
    public static InstructionPreview ForInstruction(InstructionDefinition definition)
    {
        return null;
        if (!Has(definition))
            Add(definition, new InstructionPreview(definition));
        return Previews[definition] as InstructionPreview;
    }

    /// <summary>
    /// Get an editor node info preview. Adds it if it didn't exist yet.
    /// </summary>
    public static EditorNodePreview ForEditorNode(InstructionDefinition definition)
    {
        return null;
        if (!Has(definition.EditorNode))
            Add(definition.EditorNode, new EditorNodePreview(definition));
        return Previews[definition] as EditorNodePreview;
    }

    /// <summary>
    /// Get an parameter preview. Adds it if it didn't exist yet.
    /// </summary>
    public static ParameterPreview ForParameter(Parameter parameter)
    {
        return null;
        if (!Has(parameter))
            Add(parameter, new ParameterPreview(parameter));
        return Previews[parameter] as ParameterPreview;
    }

    /// <summary>
    /// Get an output parameter preview. Adds it if it didn't exist yet.
    /// </summary>
    public static OutputPreview ForOutput(InstructionDefinition definition, string outputID)
    {
        return null;
        OutputParameter output = definition.GetParameter(outputID) as OutputParameter;
        if (!Has(output))
            Add(output, new OutputPreview(definition, outputID));
        return Previews[output] as OutputPreview;
    }

    /// <summary>
    /// Get a compile rule preview. Adds it if it didn't exist yet.
    /// </summary>
    public static RulePreview ForRule(CompileRule rule)
    {
        return null;
        switch (rule)
        {
            case InstructionRule i:
                return ForInstructionRule(i);
            case OptionRule o:
                return ForOptionRule(o);
            case ChoiceRule c:
                return ForChoiceRule(c);
            case TupleRule t:
                return ForTupleRule(t);
            case ListRule l:
                return ForListRule(l);
            default:
                return null;
        }
    }

    /// <summary>
    /// Get an instruction rule preview. Adds it if it didn't exist yet.
    /// </summary>
    public static InstructionRulePreview ForInstructionRule(InstructionRule rule)
    {
        return null;
        if (!Has(rule))
            Add(rule, new InstructionRulePreview(rule));
        return Previews[rule] as InstructionRulePreview;
    }

    /// <summary>
    /// Get an option rule preview. Adds it if it didn't exist yet.
    /// </summary>
    public static OptionRulePreview ForOptionRule(OptionRule rule)
    {
        return null;
        if (!Has(rule))
            Add(rule, new OptionRulePreview(rule));
        return Previews[rule] as OptionRulePreview;
    }

    /// <summary>
    /// Get a choice rule preview. Adds it if it didn't exist yet.
    /// </summary>
    public static ChoiceRulePreview ForChoiceRule(ChoiceRule rule)
    {
        return null;
        if (!Has(rule))
            Add(rule, new ChoiceRulePreview(rule));
        return Previews[rule] as ChoiceRulePreview;
    }

    /// <summary>
    /// Get a tuple rule preview. Adds it if it didn't exist yet.
    /// </summary>
    public static TupleRulePreview ForTupleRule(TupleRule rule)
    {
        return null;
        if (!Has(rule))
            Add(rule, new TupleRulePreview(rule));
        return Previews[rule] as TupleRulePreview;
    }

    /// <summary>
    /// Get a list rule preview. Adds it if it didn't exist yet.
    /// </summary>
    public static ListRulePreview ForListRule(ListRule rule)
    {
        return null;
        if (!Has(rule))
            Add(rule, new ListRulePreview(rule));
        return Previews[rule] as ListRulePreview;
    }
}