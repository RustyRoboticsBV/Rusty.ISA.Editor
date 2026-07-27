using System;
using System.Collections.Generic;
using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.Compilation;

public static class Compiler
{
    /* Public methods. */
    public static InstructionProgram Compile(FileCodec file)
    {
        // Create units for nodes & joints.
        Dictionary<string, Unit> units = new();
        foreach (Codec element in file.Children)
        {
            string id = element.GetAttribute(Codec.ID);
            if (element is NodeCodec node)
                units.Add(id, new NodeUnit(file, node));
            else if (element is JointCodec joint)
                units.Add(id, new JointUnit(joint));
        }

        // Connect units according to graph edges.
        foreach (Codec codec in file.Children)
        {
            if (codec is EdgeCodec edge)
            {
                string from = edge.GetAttribute(Codec.From);
                string port = edge.GetAttribute(Codec.Port);
                string to = edge.GetAttribute(Codec.To);

                Unit fromUnit = units[from];
                int portIndex = int.Parse(port);
                Unit toUnit = units[to];

                if (fromUnit is NodeUnit node)
                    node.ConnectTo(portIndex, toUnit);
                else if (fromUnit is JointUnit joint)
                    joint.ConnectTo(toUnit);
            }
        }

        // Find start units.
        // TODO: while this guarantees all units are visited, it does not guarantee the minimum set.
        // TODO: replace with SCC-based algorithm.
        HashSet<Unit> starts = new();
        HashSet<Unit> visited = new();
        foreach (var unit in units)
        {
            if (unit.Value.From.Count == 0)
            {
                starts.Add(unit.Value);
                MarkVisited(unit.Value, visited);
            }
        }
        foreach (var unit in units)
        {
            if (visited.Contains(unit.Value))
                continue;
            starts.Add(unit.Value);
            MarkVisited(unit.Value, visited);
        }

        // Determine execution order, insert gotos and insert ends.
        visited.Clear();
        List<Unit> executionOrder = new();
        HashSet<Unit> gotoTargets = new();
        foreach (Unit unit in starts)
        {
            Linearize(unit, visited, executionOrder, gotoTargets);
        }

        // Generate a label for each goto target.
        Dictionary<Unit, string> labels = [];
        int nextLabel = 0;

        foreach (Unit target in gotoTargets)
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
                case JointUnit:
                    unitInstructions.Add(new DummyInstruction());
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
        return new(metadata, iset, instructions.ToArray());
    }

    /* Private methods. */
    /// <summary>
    /// Recursively mark all units reachable from one unit as reachable. 
    /// </summary>
    private static void MarkVisited(Unit current, HashSet<Unit> marked)
    {
        if (current == null)
            return;
        if (marked.Contains(current))
            return;

        marked.Add(current);
        if (current is NodeUnit node)
        {
            foreach (Unit output in node.To)
            {
                MarkVisited(output, marked);
            }
        }
        else if (current is JointUnit joint)
            MarkVisited(joint.To, marked);
    }

    /// <summary>
    /// Recursively linearize a subgraph: determine the execution order, insert gotos & ends, and find label targets.
    /// </summary>
    private static void Linearize(Unit unit, HashSet<Unit> visited, List<Unit> executionOrder, HashSet<Unit> labelTargets)
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

            case JointUnit joint:

                // Insert end if necessary.
                if (joint.To == null)
                    joint.ConnectTo(new EndUnit());

                // Insert goto if necessary.
                else if (visited.Contains(joint.To))
                {
                    // Register as label target.
                    labelTargets.Add(joint.To);

                    // Insert goto.
                    GotoUnit gto = new();
                    gto.ConnectTo(joint.To);
                    joint.ConnectTo(gto);
                }

                // Continue with output unit.
                Linearize(joint.To, visited, executionOrder, labelTargets);
                break;

            case NodeUnit node:

                for (int i = 0; i < node.To.Length; i++)
                {
                    // Insert end if necessary.
                    if (node.To[i] == null)
                        node.ConnectTo(i, new EndUnit());

                    // Insert goto if necessary.
                    else if (visited.Contains(node.To[i]))
                    {
                        // Register as label target.
                        labelTargets.Add(node.To[i]);

                        // Insert goto.
                        GotoUnit gto = new();
                        gto.ConnectTo(node.To[i]);
                        node.ConnectTo(i, gto);
                    }

                    // Continue with output unit.
                    Linearize(node.To[i], visited, executionOrder, labelTargets);

                    // Register as label target if parameter output.
                    if (i > 0 || (i == 0 && node.OutputData.HideDefaultOutput))
                        labelTargets.Add(node.To[i]);
                }
                break;
        }
    }

    /// <summary>
    /// Compile a unit's instructions.
    /// </summary>
    private static void CompileNode(NodeUnit unit, FileCodec file, List<Instruction> instructions, Dictionary<Unit, string> labels)
    {
        // Find node definition.
        NodeCodec node = unit.Codec;
        NdefCodec ndef = file.FindNdef(node.GetAttribute(Codec.Type));

        // Compile contents.
        int handledOutputArgs = 0;
        foreach (Codec child in unit.Codec.Children)
        {
            if (child is InspectorCodec inspector)
                CompileInspector(unit, ndef, node, inspector, instructions, labels, ref handledOutputArgs);
        }

        // Check for entry point.
        if (instructions.Count > 0)
            instructions[0].Start = unit.Codec.GetAttribute(Codec.Start);
    }

    /// <summary>
    /// Compile an inspector's instructions.
    /// </summary>
    private static void CompileInspector(NodeUnit node, Codec parentDefinition, Codec parent, InspectorCodec current, List<Instruction> instructions, Dictionary<Unit, string> labels, ref int handledOutputArgs)
    {
        // Find child definition.
        string currentType = current.GetAttribute(Codec.Type);
        Codec currentDefinition = parentDefinition.FindChildWithAttribute(Codec.ID, currentType);

        // Compile form.
        if (current is FormCodec form && currentDefinition is FdefCodec fdef)
        {
            string opcode = fdef.GetAttribute(Codec.Type);
            List<string> arguments = new();
            foreach (Codec child in form.Children)
            {
                if (child is ArgCodec varg)
                {
                    string value = varg.GetAttribute(Codec.Value);
                    arguments.Add(value);
                }
                else if (child is OutCodec oarg)
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
            instructions.Add(new GenericInstruction(opcode, arguments.ToArray()));
        }

        // Compile option.
        else if (current is OptionCodec option && currentDefinition is OdefCodec odef)
        {
            InspectorCodec child = option.GetFirstChild<InspectorCodec>();
            if (child != null)
            {
                string childType = child.GetAttribute(Codec.Type);
                CompileInspector(node, currentDefinition, current, child, instructions, labels, ref handledOutputArgs);
            }
        }

        // Compile choice.
        else if (current is ChoiceCodec choice && currentDefinition is CdefCodec cdef)
        {
            InspectorCodec child = choice.GetFirstChild<InspectorCodec>();
            string childType = child.GetAttribute(Codec.Type);
            CompileInspector(node, currentDefinition, current, child, instructions, labels, ref handledOutputArgs);
        }

        // Compile tuple.
        else if (current is TupleCodec tuple && currentDefinition is TdefCodec tdef)
        {
            foreach (Codec element in tuple.Children)
            {
                if (element is InspectorCodec child)
                {
                    string childType = element.GetAttribute(Codec.Type);
                    CompileInspector(node, currentDefinition, current, child, instructions, labels, ref handledOutputArgs);
                }
            }
        }

        // Compile list.
        else if (current is ListCodec list && currentDefinition is LdefCodec ldef)
        {
            foreach (Codec element in list.Children)
            {
                if (element is InspectorCodec child)
                {
                    string childType = element.GetAttribute(Codec.Type);
                    CompileInspector(node, currentDefinition, current, child, instructions, labels, ref handledOutputArgs);
                }
            }
        }

        else
            throw new InvalidOperationException($"Invalid coded pair: '{current?.GetType()?.Name ?? "null"}' and '{currentDefinition?.GetType()?.Name ?? "null"}'.");
    }

    /// <summary>
    /// Compile metadata.
    /// </summary>
    private static Metadata CompileMetadata(FileCodec file)
    {
        Metadata metadata = new();
        if (file == null)
            return metadata;

        foreach (Codec child in file.Children)
        {
            if (child is MetaCodec data)
                metadata.AddValue(data.GetAttribute(Codec.ID), data.GetAttribute(Codec.Value));
        }
        return metadata;
    }

    /// <summary>
    /// Compile an instruction set.
    /// </summary>
    private static InstructionSet CompileInstructionSet(FileCodec file)
    {
        if (file == null)
            return new();

        // Read instructions.
        List<InstructionDefinition> definitions = new();
        foreach (Codec child in file.Children)
        {
            if (child is IdefCodec idef)
                definitions.Add(CompileIdef(idef));
        }

        // Create instruction set.
        return new(definitions.ToArray());
    }

    /// <summary>
    /// Compile an instruction definition.
    /// </summary>
    private static InstructionDefinition CompileIdef(IdefCodec idef)
    {
        // Read opcode.
        string opcode = idef.GetAttribute(Codec.ID);

        // Read execution handler.
        string exec = idef.GetAttribute(Codec.Exec);

        // Read parameters.
        List<string> parameters = new();
        foreach (Codec child in idef.Children)
        {
            if (child is PdefCodec pdef)
                parameters.Add(pdef.GetAttribute(Codec.ID));
        }

        // Create definition.
        return new(opcode, parameters.ToArray(), exec);
    }
}