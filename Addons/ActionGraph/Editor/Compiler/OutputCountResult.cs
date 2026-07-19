using Rusty.ActionGraph.Serialization;
using System;
using System.Collections.Generic;

/// <summary>
/// The result of counting the outputs of a node codec.
/// </summary>
public struct OutputCountResult
{
    /* Public properties. */
    public bool HideDefaultOutput { get; set; }
    public int ParameterOutputs { get; set; }

    /* Public methods. */
    public int CountOutputs() => HideDefaultOutput ? ParameterOutputs : ParameterOutputs + 1;

    public static OutputCountResult Combine(OutputCountResult a, OutputCountResult b)
    {
        OutputCountResult result = new();
        if (a.HideDefaultOutput || b.HideDefaultOutput)
            result.HideDefaultOutput = true;
        result.ParameterOutputs = a.ParameterOutputs + b.ParameterOutputs;
        return result;
    }

    public static OutputCountResult Create(NdefCodec ndef, NodeCodec node)
    {
        OutputCountResult result = new();
        List<InspectorDefinitionCodec> definitions = ndef.GetChildren<InspectorDefinitionCodec>();
        List<InspectorCodec> inspectors = node.GetChildren<InspectorCodec>();
        for (int i = 0; i < definitions.Count && i < inspectors.Count; i++)
        {
            Combine(result, Create(definitions[i], inspectors[i]));
        }
        return result;
    }

    public static OutputCountResult Create(InspectorDefinitionCodec definition, InspectorCodec inspector)
    {
        if (definition is FdefCodec fdef && inspector is FormCodec form)
            return Create(fdef, form);
        if (definition is OdefCodec odef && inspector is OptionCodec option)
            return Create(odef, option);
        if (definition is CdefCodec cdef && inspector is ChoiceCodec choice)
            return Create(cdef, choice);
        if (definition is TdefCodec tdef && inspector is TupleCodec tuple)
            return Create(tdef, tuple);
        if (definition is LdefCodec ldef && inspector is ListCodec list)
            return Create(ldef, list);
        throw new ArgumentException("Bad arguments.");
    }

    public static OutputCountResult Create(FdefCodec fdef, FormCodec form)
    {
        OutputCountResult result = new();
        foreach (ArgCodec codec in form.GetChildren<ArgCodec>())
        {
            string type = codec.GetAttribute(Codec.Type);
            OadefCodec oadef = fdef.FindOadef(type);
            if (oadef != null)
            {
                result.ParameterOutputs++;
                if (oadef.InnerText.Trim().ToLower() == "true")
                    result.HideDefaultOutput = true;
            }
        }
        return result;
    }

    public static OutputCountResult Create(OdefCodec odef, OptionCodec option)
    {
        InspectorDefinitionCodec definition = odef.GetFirstChild<InspectorDefinitionCodec>();
        InspectorCodec inspector = option.GetFirstChild<InspectorCodec>();
        return Create(definition, inspector);
    }

    public static OutputCountResult Create(CdefCodec cdef, ChoiceCodec choice)
    {
        string selected = choice.GetAttribute(Codec.Select);

        InspectorDefinitionCodec definition = null;
        foreach (var codec in cdef.GetChildren<InspectorDefinitionCodec>())
        {
            if (codec.GetAttribute(Codec.ID) == selected)
            {
                definition = codec;
                break;
            }
        }

        InspectorCodec inspector = choice.GetFirstChild<InspectorCodec>();

        return Create(definition, inspector);
    }

    public static OutputCountResult Create(TdefCodec tdef, TupleCodec tuple)
    {
        OutputCountResult result = new();

        var definitions = tdef.GetChildren<InspectorDefinitionCodec>();
        var inspectors = tuple.GetChildren<InspectorCodec>();
        for (int i = 0; i < definitions.Count && i < inspectors.Count; i++)
        {
            Combine(result, Create(definitions[i], inspectors[i]));
        }

        return result;
    }

    public static OutputCountResult Create(LdefCodec ldef, ListCodec list)
    {
        OutputCountResult result = new();

        var definition = ldef.GetFirstChild<InspectorDefinitionCodec>();
        var inspectors = list.GetChildren<InspectorCodec>();
        for (int i = 0; i < inspectors.Count; i++)
        {
            Combine(result, Create(definition, inspectors[i]));
        }

        return result;
    }
}