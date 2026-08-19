using System;
using System.Collections.Generic;
using System.Text;
using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.CodeGen;

/// <summary>
/// The result of counting the outputs of a node codec.
/// </summary>
internal sealed class OutputCountResult
{
    /* Public properties. */
    public static OutputCountResult Default => new();

    public bool HideDefaultOutput { get; set; }
    public List<OutputPortInfo> Arguments { get; } = new();

    /* Constructors. */
    public OutputCountResult() { }

    public OutputCountResult(FileCodec file, Codec node)
    {
        // Find the ndef.
        string nodeType = node.GetAttribute(Codec.Type);
        NdefCodec ndef = file.FindNdef(nodeType);

        // Search for outputs.
        Search(file, ndef, node);
    }

    /* Public methods. */
    public override string ToString()
    {
        StringBuilder sb = new();
        if (!HideDefaultOutput)
            sb.Append("Default");
        foreach (OutputPortInfo arg in Arguments)
        {
            if (sb.Length > 0)
                sb.Append("\n");
            sb.Append(arg.ToString());
        }
        return sb.ToString();
    }

    public int CountOutputs()
    {
        if (Arguments.Count == 0)
            return 1;
        else
        {
            if (HideDefaultOutput)
                return Arguments.Count;
            else
                return Arguments.Count + 1;
        }
    }

    public bool IsParameterPort(int index)
    {
        if (index < 0 || index >= CountOutputs())
            throw new ArgumentOutOfRangeException(nameof(index));

        return !HideDefaultOutput && index == 0;
    }

    /* Private methods. */
    private void Search(FileCodec file, Codec definition, Codec instance)
    {
        // If a form, search it.
        if (definition is FdefCodec fdef && instance is FormCodec form)
        {
            Search(file, fdef, form);
            return;
        }

        // Search children.
        else if (definition is ICodecGroup<InspectorDefinitionCodec> collection && instance is ICodecGroup<InspectorCodec> inspector)
        {
            for (int i = 0; i < inspector.Children.Count; i++)
            {
                Codec child = inspector.Children[i];
                string type = child.GetAttribute(Codec.Type);
                InspectorDefinitionCodec childDefinition = collection.Find(type);
                if (childDefinition == null)
                    throw new NullReferenceException($"Cannot find definition '{type}' in '{definition.ToString(true)}'.");
                Search(file, childDefinition, child);
            }
        }

        else
            throw new InvalidOperationException("Cannot find outputs of codec " + instance + " / " + definition);
    }

    private void Search(FileCodec file, FdefCodec fdef, FormCodec form)
    {
        // Find the idef.
        IdefCodec idef = file.FindIdef(fdef.GetAttribute(Codec.Type));

        // Parallel-search arguments.
        int count = Math.Min(fdef.Children.Count, form.Children.Count);
        for (int i = 0; i < count; i++)
        {
            if (fdef.Children[i] is OadefCodec oadef && form.Children[i] is OutCodec output)
            {
                PdefCodec parameter = idef.FindPdef(oadef.GetAttribute(Codec.Type));

                Arguments.Add(new OutputPortInfo(parameter, oadef, output));

                if (oadef.GetAttribute(Codec.NoDefault).ToLower() == "true")
                    HideDefaultOutput = true;
            }
        }
    }
}