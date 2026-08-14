using System;
using System.Collections.Generic;
using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.Compilation;

using Labels = Dictionary<Unit, string>;

/// <summary>
/// A utility that takes a FileCodec and compiles it into an InstructionProgram.
/// </summary>
internal static class Compiler
{
    /* Public methods. */
    public static InstructionProgram Compile(FileCodec file)
    {
        // Create units for nodes.
        Dictionary<string, Unit> units = new();
        foreach (Codec element in file.Children)
        {
            string id = element.GetAttribute(Codec.ID);
            if (element is NodeCodec node)
                units.Add(id, new NodeUnit(file, node));
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

                for (int i = 0; i < node.To.Length; i++)
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
    private static void CompileNode(NodeUnit unit, FileCodec file, List<Instruction> instructions, Labels labels)
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

        // Ensure a dummy instruction if the node compiled to no instructions.
        if (instructions.Count == 0)
            instructions.Add(new DummyInstruction());

        // Check for entry point.
        instructions[0].Start = unit.Codec.GetAttribute(Codec.Start);
    }

    /// <summary>
    /// Compile an inspector's instructions.
    /// </summary>
    private static void CompileInspector(NodeUnit node, Codec parentDefinition, Codec parent, InspectorCodec current, List<Instruction> instructions, Labels labels, ref int handledOutputArgs)
    {
        // Find child definition.
        string currentType = current.GetAttribute(Codec.Type);
        Codec currentDefinition = parentDefinition.FindChildWithAttribute(Codec.ID, currentType);

        switch (currentDefinition, current)
        {
            case (FdefCodec fdef, FormCodec form):
                CompileForm(node, fdef, form, instructions, labels, ref handledOutputArgs);
                break;
            case (OdefCodec odef, OptionCodec option):
                CompileOption(node, odef, option, instructions, labels, ref handledOutputArgs);
                break;
            case (CdefCodec cdef, ChoiceCodec choice):
                CompileChoice(node, cdef, choice, instructions, labels, ref handledOutputArgs);
                break;
            case (TdefCodec tdef, TupleCodec tuple):
                CompileTuple(node, tdef, tuple, instructions, labels, ref handledOutputArgs);
                break;
            case (LdefCodec ldef, ListCodec list):
                CompileList(node, ldef, list, instructions, labels, ref handledOutputArgs);
                break;
            default:
                throw new InvalidOperationException($"Invalid coded pair: '{current?.ToString(true) ?? "null"}' and '{currentDefinition?.ToString(true) ?? "null"}'.");
        }
    }

    /// <summary>
    /// Compile an option.
    /// </summary>
    private static void CompileOption(NodeUnit node, OdefCodec odef, OptionCodec option, List<Instruction> instructions, Labels labels, ref int handledOutputArgs)
    {
        InspectorCodec child = option.GetFirstChild<InspectorCodec>();
        if (child != null)
        {
            string childType = child.GetAttribute(Codec.Type);
            CompileInspector(node, odef, option, child, instructions, labels, ref handledOutputArgs);
        }
    }

    /// <summary>
    /// Compile a choice.
    /// </summary>
    private static void CompileChoice(NodeUnit node, CdefCodec cdef, ChoiceCodec choice, List<Instruction> instructions, Labels labels, ref int handledOutputArgs)
    {
        InspectorCodec child = choice.GetFirstChild<InspectorCodec>();
        string childType = child.GetAttribute(Codec.Type);
        CompileInspector(node, cdef, choice, child, instructions, labels, ref handledOutputArgs);
    }

    /// <summary>
    /// Compile a tuple.
    /// </summary>
    private static void CompileTuple(NodeUnit node, TdefCodec tdef, TupleCodec tuple, List<Instruction> instructions, Labels labels, ref int handledOutputArgs)
    {
        foreach (Codec element in tuple.Children)
        {
            if (element is InspectorCodec child)
            {
                string childType = element.GetAttribute(Codec.Type);
                CompileInspector(node, tdef, tuple, child, instructions, labels, ref handledOutputArgs);
            }
        }
    }

    /// <summary>
    /// Compile a list.
    /// </summary>
    private static void CompileList(NodeUnit node, LdefCodec ldef, ListCodec list, List<Instruction> instructions, Labels labels, ref int handledOutputArgs)
    {
        foreach (Codec element in list.Children)
        {
            if (element is InspectorCodec child)
            {
                string childType = element.GetAttribute(Codec.Type);
                CompileInspector(node, ldef, list, child, instructions, labels, ref handledOutputArgs);
            }
        }
    }

    /// <summary>
    /// Compile a form.
    /// </summary>
    private static void CompileForm(NodeUnit node, FdefCodec fdef, FormCodec form, List<Instruction> instructions, Labels labels, ref int handledOutputArgs)
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
        return new(definitions);
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
}