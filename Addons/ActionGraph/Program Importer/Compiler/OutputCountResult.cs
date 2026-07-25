using System;
using System.Collections.Generic;
using System.Text;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A OadefCodec and OutCodec pair representing an argument output port.
/// </summary>
internal class OutputPortInfo
{
    /* Public properties. */
    public PdefCodec Parameter { get; private set; }
    public OadefCodec Definition { get; private set; }
    public OutCodec Instance { get; private set; }

    /* Constructors. */
    public OutputPortInfo(PdefCodec parameter, OadefCodec definition, OutCodec instance)
    {
        Parameter = parameter;
        Definition = definition;
        Instance = instance;
    }

    /* Public methods. */
    public override string ToString()
    {
        return Parameter.GetAttribute(Codec.ID)
            + " / " + Definition.GetAttribute(Codec.ID)
            + ": " + Instance.GetAttribute(Codec.Value);
    }
}

/// <summary>
/// The result of counting the outputs of a node codec.
/// </summary>
internal class OutputCountResult
{
    /* Public properties. */
    public static OutputCountResult Default => new();

    public bool HideDefaultOutput { get; set; }
    public List<OutputPortInfo> Arguments { get; } = new();

    /* Constructors. */
    public OutputCountResult() { }

    public OutputCountResult(NodeCodec node, FileCodec file)
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
            //Search(file, fdef, form);
            return;
        }

        // Search children.
        else if (definition is CollectionDefinitionCodec collection && instance is InspectorCodec inspector)
        {
            for (int i = 0; i < inspector.Children.Count; i++)
            {
                //Codec child = inspector.Children[i];
                //InspectorDefinitionCodec childDefinition = collection.FindInspector(child.GetAttribute(Codec.Type));
                //Search(file, childDefinition, child);
            }
        }
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

                if (oadef.GetAttribute(Codec.HideDefault).ToLower() == "true")
                    HideDefaultOutput = true;
            }
        }
    }
}