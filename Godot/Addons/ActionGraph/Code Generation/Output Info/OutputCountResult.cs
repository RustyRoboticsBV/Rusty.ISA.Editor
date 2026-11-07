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

    public OutputCountResult(FileElement file, NodeElement node)
    {
        var definition = Find<NodeDefinitionElement>(file, node.Definition);
        Search(file, definition, node);
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
    private void Search(FileElement file, Element definition, Element instance)
    {
        // If a form, search it.
        if (definition is FormDefinitionElement fdef && instance is FormElement form)
            Search(file, fdef, form);

        // Search children.
        else if (definition is InspectorDefinitionElement idef && instance is InspectorElement inspector)
        {
            for (int i = 0; i < instance.ChildCount; i++)
            {
                if (instance.GetChild(i) is InspectorElement child)
                {
                    var childDefinition = Find<InspectorDefinitionElement>(idef, child.Definition);
                    if (childDefinition == null)
                        throw new NullReferenceException($"Cannot find definition '{child.Definition}' in '{ElementPrinter.ToString(definition, true)}'.");
                    Search(file, childDefinition, child);
                }
            }
        }
        else if (definition is NodeDefinitionElement ndef && instance is NodeElement node)
        {
            for (int i = 0; i < instance.ChildCount; i++)
            {
                if (instance.GetChild(i) is InspectorElement child)
                {
                    var childDefinition = Find<InspectorDefinitionElement>(ndef, child.Definition);
                    if (childDefinition == null)
                        throw new NullReferenceException($"Cannot find definition '{child.Definition}' in '{ElementPrinter.ToString(definition, true)}'.");
                    Search(file, childDefinition, child);
                }
            }
        }

        else
            throw new InvalidOperationException("Cannot find outputs of codec " + instance + " / " + definition);
    }

    private void Search(FileElement file, FormDefinitionElement definition, FormElement form)
    {
        // Find the instruction definition.
        var instruction = Find<InstructionDefinitionElement>(file, definition.Instruction);
        if (instruction == null)
            throw new NullReferenceException($"Cannot find instruction definition '{definition.Instruction}'.");

        // Parallel-search arguments.
        int count = Math.Min(definition.ChildCount, form.ChildCount);
        for (int i = 0; i < count; i++)
        {
            if (definition.GetChild(i) is PortDefinitionElement portDefinition && form.GetChild(i) is PortElement port)
            {
                var parameter = Find<ParameterDefinitionElement>(instruction, portDefinition.Parameter);
                if (parameter == null)
                    throw new NullReferenceException($"Cannot find parameter definition '{portDefinition.Parameter}'.");

                Arguments.Add(new OutputPortInfo(parameter, portDefinition, port));

                if (portDefinition.RemoveDefaultPort)
                    HideDefaultOutput = true;
            }
        }
    }

    private static T Find<T>(Element parent, string id)
        where T : Element
    {
        if (parent == null)
            throw new ArgumentNullException(nameof(parent));

        for (int i = 0; i < parent.ChildCount; i++)
        {
            if (parent.GetChild(i) is T definition && definition.ID == id)
                return definition;
        }
        return null;
    }
}