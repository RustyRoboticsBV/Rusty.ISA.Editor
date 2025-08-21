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
    public static InstructionPreview ForInstruction(InstructionSet set, InstructionDefinition definition)
    {
        if (!Has(definition))
            Add(definition, new InstructionPreview(set, definition));
        return Previews[definition] as InstructionPreview;
    }

    /// <summary>
    /// Get an editor node info preview. Adds it if it didn't exist yet.
    /// </summary>
    public static EditorNodePreview ForEditorNode(InstructionSet set, InstructionDefinition definition)
    {
        if (!Has(definition.EditorNode))
            Add(definition.EditorNode, new EditorNodePreview(set, definition));
        return Previews[definition.EditorNode] as EditorNodePreview;
    }

    /// <summary>
    /// Get an parameter preview. Adds it if it didn't exist yet.
    /// </summary>
    public static ParameterPreview ForParameter(Parameter parameter)
    {
        if (!Has(parameter))
            Add(parameter, new ParameterPreview(parameter));
        return Previews[parameter] as ParameterPreview;
    }

    /// <summary>
    /// Get an output parameter preview. Adds it if it didn't exist yet.
    /// </summary>
    public static OutputPreview ForOutput(InstructionDefinition definition, string outputID)
    {
        OutputParameter output = definition.GetParameter(outputID) as OutputParameter;
        if (!Has(output))
            Add(output, new OutputPreview(definition, outputID));
        return Previews[output] as OutputPreview;
    }

    /// <summary>
    /// Get a compile rule preview. Adds it if it didn't exist yet.
    /// </summary>
    public static RulePreview ForRule(InstructionSet set, CompileRule rule)
    {
        switch (rule)
        {
            case InstructionRule i:
                return ForInstructionRule(set, i);
            case OptionRule o:
                return ForOptionRule(set, o);
            case ChoiceRule c:
                return ForChoiceRule(set, c);
            case TupleRule t:
                return ForTupleRule(set, t);
            case ListRule l:
                return ForListRule(set, l);
            default:
                return null;
        }
    }

    /// <summary>
    /// Get an instruction rule preview. Adds it if it didn't exist yet.
    /// </summary>
    public static InstructionRulePreview ForInstructionRule(InstructionSet set, InstructionRule rule)
    {
        if (!Has(rule))
            Add(rule, new InstructionRulePreview(set, rule));
        return Previews[rule] as InstructionRulePreview;
    }

    /// <summary>
    /// Get an option rule preview. Adds it if it didn't exist yet.
    /// </summary>
    public static OptionRulePreview ForOptionRule(InstructionSet set, OptionRule rule)
    {
        if (!Has(rule))
            Add(rule, new OptionRulePreview(set, rule));
        return Previews[rule] as OptionRulePreview;
    }

    /// <summary>
    /// Get a choice rule preview. Adds it if it didn't exist yet.
    /// </summary>
    public static ChoiceRulePreview ForChoiceRule(InstructionSet set, ChoiceRule rule)
    {
        if (!Has(rule))
            Add(rule, new ChoiceRulePreview(set, rule));
        return Previews[rule] as ChoiceRulePreview;
    }

    /// <summary>
    /// Get a tuple rule preview. Adds it if it didn't exist yet.
    /// </summary>
    public static TupleRulePreview ForTupleRule(InstructionSet set, TupleRule rule)
    {
        if (!Has(rule))
            Add(rule, new TupleRulePreview(set, rule));
        return Previews[rule] as TupleRulePreview;
    }

    /// <summary>
    /// Get a list rule preview. Adds it if it didn't exist yet.
    /// </summary>
    public static ListRulePreview ForListRule(InstructionSet set, ListRule rule)
    {
        if (!Has(rule))
            Add(rule, new ListRulePreview(set, rule));
        return Previews[rule] as ListRulePreview;
    }
}