using System;
using System.Collections.Generic;
using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.CodeGen;

using Labels = Dictionary<Unit, string>;

/// <summary>
/// A utility that takes a file codec and converts it into an instruction program.
/// </summary>
internal static class CodeGenerator
{
    /* Public methods. */
    /// <summary>
    /// Convert a file codec into an instruction program.
    /// </summary>
    public static InstructionProgram Generate(FileElement file)
    {
        // Create units for nodes.
        Dictionary<string, Unit> units = new();
        for (int i = 0; i < file.ChildCount; i++)
        {
            Element child = file.GetChild(i);
            if (child is NodeElement node)
                units.Add(child.ID, new NodeUnit(file, node));
        }

        // Connect units according to graph edges.
        for (int i = 0; i < file.ChildCount; i++)
        {
            Element child = file.GetChild(i);
            if (child is EdgeElement edge)
            {
                Unit from = units[edge.From];
                uint port = edge.Port;
                Unit to = units[edge.To];

                if (from is NodeUnit node)
                    node.ConnectTo(port, to);
            }
        }

        // Find start units.
        List<SCC> sccs = Tarjan.GetSCCs(units.Values);
        List<Unit> starts = new();
        foreach (SCC scc in sccs)
        {
            if (scc.IsStartComponent())
                starts.Add(scc.GetRepresentativeUnit());
        }

        // Determine execution order, insert gotos and insert ends.
        HashSet<Unit> visited = new();
        List<Unit> executionOrder = new();
        HashSet<Unit> jumpTargets = new();
        foreach (Unit unit in starts)
        {
            Linearize(unit, visited, executionOrder, jumpTargets);
        }

        // Generate a label for each jump target.
        Labels labels = [];
        int nextLabel = 0;

        foreach (Unit target in jumpTargets)
        {
            labels[target] = "L_" + nextLabel.ToString();
            nextLabel++;
        }

        // Compile to instructions.
        List<Instruction> instructions = new();
        List<Instruction> unitInstructions = new();
        foreach (Unit unit in executionOrder)
        {
            // Compile this unit's instructions.
            unitInstructions.Clear();
            switch (unit)
            {
                case EndUnit:
                    unitInstructions.Add(new EndInstruction());
                    break;
                case GotoUnit gto:
                    unitInstructions.Add(new GotoInstruction(labels[gto.To]));
                    break;
                case NodeUnit node:
                    CompileNode(node, file, unitInstructions, labels);
                    break;
            }

            // Add label if necessary.
            if (labels.ContainsKey(unit) && unitInstructions.Count > 0)
                unitInstructions[0].Label = labels[unit];

            // Add unit's instructions to compiled program.
            instructions.AddRange(unitInstructions);
        }

        // Compile metadata & instruction set.
        Metadata metadata = CompileMetadata(file);
        InstructionSet iset = CompileInstructionSet(file);

        // Create program.
        return new(metadata, iset, new(instructions));
    }

    /* Private methods. */
    /// <summary>
    /// Recursively linearize a subgraph: determine the execution order, insert gotos & ends, and find jump targets.
    /// </summary>
    private static void Linearize(Unit unit, HashSet<Unit> visited, List<Unit> executionOrder, HashSet<Unit> jumpTargets)
    {
        // Register unit as visited.
        if (!visited.Add(unit))
            return;

        // Add unit to execution order.
        executionOrder.Add(unit);

        switch (unit)
        {
            case EndUnit:
            case GotoUnit:
                break;

            case NodeUnit node:

                for (uint i = 0; i < node.To.Length; i++)
                {
                    // Insert end if necessary.
                    if (node.To[i] == null)
                        node.ConnectTo(i, new EndUnit());

                    // Insert goto if necessary.
                    else if (visited.Contains(node.To[i]))
                    {
                        // Register as jump target.
                        jumpTargets.Add(node.To[i]);

                        // Insert goto.
                        GotoUnit gto = new();
                        gto.ConnectTo(node.To[i]);
                        node.ConnectTo(i, gto);
                    }

                    // Continue with output unit.
                    Linearize(node.To[i], visited, executionOrder, jumpTargets);

                    // Register as jump target if parameter output.
                    if (i > 0 || (i == 0 && node.OutputData.HideDefaultOutput))
                        jumpTargets.Add(node.To[i]);
                }
                break;

            default:
                throw new InvalidOperationException($"Invalid unit: {unit}");
        }
    }

    /// <summary>
    /// Compile a unit's instructions.
    /// </summary>
    private static void CompileNode(NodeUnit unit, FileElement file, List<Instruction> instructions, Labels labels)
    {
        // Find node definition.
        NodeElement node = unit.Element;
        var ndef = Find<NodeDefinitionElement>(file, node.Definition);

        // Compile contents.
        int handledOutputArgs = 0;
        for (int i = 0; i < node.ChildCount; i++)
        {
            if (node.GetChild(i) is InspectorElement inspector)
                CompileInspector(unit, ndef, node, inspector, instructions, labels, ref handledOutputArgs);
        }

        // Ensure a dummy instruction if the node compiled to no instructions.
        if (instructions.Count == 0)
            instructions.Add(new DummyInstruction());

        // Check for entry point.
        instructions[0].Start = node.StartPointName;
    }

    /// <summary>
    /// Compile an inspector's instructions.
    /// </summary>
    private static void CompileInspector(NodeUnit node, Element parentDefinition, Element parent, InspectorElement current, List<Instruction> instructions, Labels labels, ref int handledOutputArgs)
    {
        // Find child definition.
        Element currentDefinition = Find<InspectorElement>(current, current.Definition);

        switch (currentDefinition, current)
        {
            case (FormDefinitionElement fdef, FormElement form):
                CompileForm(node, fdef, form, instructions, labels, ref handledOutputArgs);
                break;
            case (OptionDefinitionElement odef, OptionElement option):
                CompileOption(node, odef, option, instructions, labels, ref handledOutputArgs);
                break;
            case (ChoiceDefinitionElement cdef, ChoiceElement choice):
                CompileChoice(node, cdef, choice, instructions, labels, ref handledOutputArgs);
                break;
            case (TupleDefinitionElement tdef, TupleElement tuple):
                CompileTuple(node, tdef, tuple, instructions, labels, ref handledOutputArgs);
                break;
            case (ListDefinitionElement ldef, ListElement list):
                CompileList(node, ldef, list, instructions, labels, ref handledOutputArgs);
                break;
            default:
                throw new InvalidOperationException($"Invalid coded pair: '{ElementPrinter.ToString(current, true)
                    ?? "null"}' and '{ElementPrinter.ToString(currentDefinition, true) ?? "null"}'.");
        }
    }

    /// <summary>
    /// Compile an option.
    /// </summary>
    private static void CompileOption(NodeUnit node, OptionDefinitionElement odef, OptionElement option, List<Instruction> instructions, Labels labels, ref int handledOutputArgs)
    {
        InspectorElement child = FindFirst<InspectorElement>(option);
        if (child != null)
            CompileInspector(node, odef, option, child, instructions, labels, ref handledOutputArgs);
    }

    /// <summary>
    /// Compile a choice.
    /// </summary>
    private static void CompileChoice(NodeUnit node, ChoiceDefinitionElement cdef, ChoiceElement choice, List<Instruction> instructions, Labels labels, ref int handledOutputArgs)
    {
        InspectorElement child = FindFirst<InspectorElement>(choice);
        CompileInspector(node, cdef, choice, child, instructions, labels, ref handledOutputArgs);
    }

    /// <summary>
    /// Compile a tuple.
    /// </summary>
    private static void CompileTuple(NodeUnit node, TupleDefinitionElement tdef, TupleElement tuple, List<Instruction> instructions, Labels labels, ref int handledOutputArgs)
    {
        for (int i = 0; i < tuple.ChildCount; i++)
        {
            if (tuple.GetChild(i) is InspectorElement child)
                CompileInspector(node, tdef, tuple, child, instructions, labels, ref handledOutputArgs);
        }
    }

    /// <summary>
    /// Compile a list.
    /// </summary>
    private static void CompileList(NodeUnit node, ListDefinitionElement ldef, ListElement list, List<Instruction> instructions, Labels labels, ref int handledOutputArgs)
    {
        for (int i = 0; i < list.ChildCount; i++)
        {
            if (list.GetChild(i) is InspectorElement child)
                CompileInspector(node, ldef, list, child, instructions, labels, ref handledOutputArgs);
        }
    }

    /// <summary>
    /// Compile a form.
    /// </summary>
    private static void CompileForm(NodeUnit node, FormDefinitionElement fdef, FormElement form, List<Instruction> instructions, Labels labels, ref int handledOutputArgs)
    {
        // Compile arguments.
        List<string> arguments = new();
        for (int i = 0; i < form.ChildCount; i++)
        {
            // Field argument.
            if (form.GetChild(i) is FieldElement field)
                arguments.Add(field.Value);

            // Port argument.
            else if (form.GetChild(i) is PortElement port)
            {
                // Find parameter target unit.
                int outputPort = node.OutputData.HideDefaultOutput ? handledOutputArgs : handledOutputArgs + 1;
                Unit to = node.To[outputPort];
                handledOutputArgs++;

                // Set argument value.
                string value = labels[to];
                arguments.Add(value);
            }
        }
        instructions.Add(new GenericInstruction(fdef.InstructionDefinition, arguments.ToArray()));
    }

    /// <summary>
    /// Compile an instruction set.
    /// </summary>
    private static InstructionSet CompileInstructionSet(FileElement file)
    {
        if (file == null)
            return new();

        // Read instructions.
        List<InstructionDefinition> definitions = new();
        for (int i = 0; i < file.ChildCount; i++)
        {
            if (file.GetChild(i) is InstructionDefinitionElement definition)
                definitions.Add(CompileInstruction(definition));
        }

        // Create instruction set.
        return new(definitions);
    }

    /// <summary>
    /// Compile an instruction definition.
    /// </summary>
    private static InstructionDefinition CompileInstruction(InstructionDefinitionElement idef)
    {
        // Read parameters.
        List<string> parameters = new();
        for (int i = 0; i < idef.ChildCount; i++)
        {
            if (idef.GetChild(i) is ParameterDefinitionElement pdef)
                parameters.Add(pdef.ID);
        }

        // Create definition.
        return new(idef.ID, parameters.ToArray(), idef.ExecutionHandler);
    }

    /// <summary>
    /// Compile metadata.
    /// </summary>
    private static Metadata CompileMetadata(FileElement file)
    {
        Metadata metadata = new();
        if (file == null)
            return metadata;

        for (int i = 0; i < file.ChildCount; i++)
        {
            if (file.GetChild(i) is MetadataElement data)
                metadata.AddValue(data.Key, data.Value);
        }
        return metadata;
    }

    /// <summary>
    /// Find a child element.
    /// </summary>
    private static T FindFirst<T>(Element element)
        where T : Element
    {
        for (int i = 0; i < element.ChildCount; i++)
        {
            if (element.GetChild(i) is T definition)
                return definition;
        }
        return null;
    }

    /// <summary>
    /// Find a child element.
    /// </summary>
    private static T Find<T>(Element element, string id)
        where T : Element
    {
        for (int i = 0; i < element.ChildCount; i++)
        {
            if (element.GetChild(i) is T definition && definition.ID == id)
                return definition;
        }
        return null;
    }
}