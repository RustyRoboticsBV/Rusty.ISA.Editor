using System;
using System.Collections.Generic;
using System.Data;
using Rusty.ActionGraph.Runtime;
using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.Compilation;

public static class Compiler
{
    /* Public methods. */
    public static InstructionProgram Compile(FileCodec file)
    {
        Godot.GD.Print(file);
        // Compile metadata & instruction set.
        Metadata metadata = CompileMetadata(file);
        InstructionSet iset = CompileInsructionSet(file);

        // Create units for nodes & joints.
        Dictionary<string, Unit> units = new();
        Dictionary<string, Codec> elements = GetElements(file);
        foreach (Codec element in file.Children)
        {
            string id = element.GetAttribute(Codec.ID);
            if (element is NodeCodec node)
            {
                // Find ndef.
                string type = node.GetAttribute(Codec.Type);
                NdefCodec ndef = ExtractWithID<NdefCodec>(file, type);
                if (ndef == null)
                    throw new KeyNotFoundException($"Could not find node definition '{type}'.");

                // Count outputs.
                OutputCountResult outputs = OutputCountResult.Create(ndef, node);

                // Create unit.
                Unit unit = new(outputs, node);
                units.Add(id, unit);
            }
            else if (element is JointCodec joint)
            {
                Unit unit = new(OutputCountResult.Default, joint);
                unit.Codec = joint;
                units.Add(id, unit);
            }
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

                fromUnit.ConnectTo(portIndex, toUnit);
            }
        }

        // Insert ends.
        int endIndex = 0;
        foreach (var unit in units)
        {
            for (int i = 0; i < unit.Value.To.Length; i++)
            {
                if (unit.Value.To[i] == null)
                {
                    Unit endUnit = new();
                    unit.Value.ConnectTo(i, endUnit);
                    units.Add("_end" + endIndex, endUnit);
                    endIndex++;
                }
            }
        }

        // Find start units.
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

        // Compile to instructions.
        int nextLabel = 0;
        visited.Clear();
        foreach (Unit startUnit in starts)
        {
            CompileUnitInstructions(startUnit, file, visited, ref nextLabel);
        }

        // Combine instructions.
        List<Instruction> instructions = new();
        foreach (var unitPair in units)
        {
            Unit unit = unitPair.Value;
            Godot.GD.Print(unitPair.Key + " " + unit);

            // Apply label to first instruction in unit.
            if (unit.Label != null && unit.Compiled.Count > 0)
                unit.Compiled[0].Label = unit.Label;

            // Apply start to first instruction in unit.
            if (unit.Start != null && unit.Compiled.Count > 0)
                unit.Compiled[0].Start = unit.Start;

            // Add instructions to list.
            foreach (Instruction instruction in unit.Compiled)
            {
                instructions.Add(instruction);
            }
        }

        // Create program.
        return new(metadata, iset, instructions.ToArray());
    }

    /* Private methods. */
    /// <summary>
    /// Collect all nodes and joints from the graph.
    /// </summary>
    private static Dictionary<string, Codec> GetElements(FileCodec file)
    {
        Dictionary<string, Codec> elements = new();
        foreach (var element in file.Children)
        {
            if (element is NodeCodec || element is JointCodec)
            {
                string id = element.GetAttribute(Codec.ID);
                if (elements.ContainsKey(id))
                    throw new DuplicateNameException($"Duplicate element ID '{id}'.");
                elements.Add(id, element);
            }
        }
        return elements;
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
                metadata.AddValue(data.GetAttribute(Codec.ID), data.InnerText);
        }
        return metadata;
    }

    /// <summary>
    /// Compile an instruction set.
    /// </summary>
    private static InstructionSet CompileInsructionSet(FileCodec file)
    {
        if (file == null)
            return new();

        // Read instructions.
        var idefs = file.GetChildren<IdefCodec>();
        InstructionDefinition[] definitions = new InstructionDefinition[idefs.Count];
        for (int i = 0; i < idefs.Count; i++)
        {
            definitions[i] = CompileIdef(idefs[i]);
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

        // Read parameters.
        var pdefs = idef.GetChildren<PdefCodec>();
        string[] parameters = new string[pdefs.Count];
        for (int i = 0; i < pdefs.Count; i++)
        {
            parameters[i] = pdefs[i].GetAttribute(Codec.ID);
        }

        // Read execution handler.
        string executionHandler = idef.GetAttribute(Codec.Exec);

        // Create definition.
        return new(opcode, parameters, executionHandler);
    }

    /// <summary>
    /// Compile a unit's instructions.
    /// </summary>
    private static void CompileUnitInstructions(Unit unit, FileCodec file, HashSet<Unit> visited, ref int nextLabel)
    {
        // Mark unit as visited.
        visited.Add(unit);

        // Compile contents.
        if (unit.Codec == null)
            unit.Compiled.Add(new EndInstruction());
        else if (unit.Codec is JointCodec)
            unit.Compiled.Add(new DummyInstruction());
        else if (unit.Codec is NodeCodec node)
        {
            NdefCodec ndef = ExtractWithID<NdefCodec>(file, unit.Codec.GetAttribute(Codec.Type));

            // Compile contents.
            int handledOutputArgs = 0;
            foreach (Codec child in unit.Codec.Children)
            {
                if (child is FormCodec or OptionCodec or ChoiceCodec or TupleCodec or ListCodec)
                    CompileInspectorInstructions(unit, ndef, node, child, ref handledOutputArgs, ref nextLabel);
            }

            unit.Start = node.GetAttribute(Codec.Start);
        }

        // Handle outputs.
        for (int i = 0; i < unit.To.Length; i++)
        {
            Unit output = unit.To[i];

            // If the unit was already compiled, generate goto instruction.
            if (visited.Contains(output))
            {
                if (!unit.OutputData.IsParameterPort(i))
                {
                    // Generate label if necessary.
                    if (output.Label == null)
                    {
                        output.Label = nextLabel.ToString();
                        nextLabel++;
                    }

                    // Generate goto.
                    unit.Compiled.Add(new GotoInstruction(output.Label));
                }
            }

            // Else, compile output unit.
            else
                CompileUnitInstructions(output, file, visited, ref nextLabel);
        }
    }

    /// <summary>
    /// Compile an inspector's instructions.
    /// </summary>
    private static void CompileInspectorInstructions(Unit unit, Codec parentDefinition, Codec parent, Codec current, ref int handledOutputArgs, ref int nextLabel)
    {
        // Find child definition.
        string currentType = current.GetAttribute(Codec.Type);
        Codec currentDefinition = ExtractWithID<Codec>(parentDefinition, currentType);

        // Compile form.
        if (current is FormCodec form && currentDefinition is FdefCodec fdef)
        {
            string opcode = fdef.GetAttribute(Codec.Type);
            List<string> arguments = new();
            foreach (Codec child in current.Children)
            {
                if (child is VadefCodec varg)
                {
                    string value = varg.GetAttribute(Codec.Value);
                    arguments.Add(value);
                }
                else if (child is OadefCodec oarg)
                {
                    int outputPort = unit.OutputData.HideDefaultOutput ? handledOutputArgs : handledOutputArgs + 1;
                    Unit to = unit.To[outputPort];

                    if (to.Label == null)
                    {
                        to.Label = nextLabel.ToString();
                        nextLabel++;
                    }

                    arguments.Add(to.Label);
                }
            }
            unit.Compiled.Add(new GenericInstruction(opcode, arguments.ToArray()));
        }

        // Compile option.
        else if (current is OptionCodec option)
        {
            Codec child = option.GetFirstChild<Codec>();
            if (child != null)
            {
                string childType = child.GetAttribute(Codec.Type);
                CompileInspectorInstructions(unit, currentDefinition, current, child, ref handledOutputArgs, ref nextLabel);
            }
        }

        // Compile choice.
        else if (current is ChoiceCodec choice)
        {
            Codec child = choice.GetFirstChild<Codec>();
            string childType = child.GetAttribute(Codec.Type);
            CompileInspectorInstructions(unit, currentDefinition, current, child, ref handledOutputArgs, ref nextLabel);
        }

        // Compile tuple.
        else if (current is TupleCodec tuple)
        {
            foreach (Codec child in tuple.Children)
            {
                string childType = child.GetAttribute(Codec.Type);
                CompileInspectorInstructions(unit, currentDefinition, current, child, ref handledOutputArgs, ref nextLabel);
            }
        }

        // Compile list.
        else if (current is ListCodec list)
        {
            foreach (Codec child in list.Children)
            {
                string childType = child.GetAttribute(Codec.Type);
                CompileInspectorInstructions(unit, currentDefinition, current, child, ref handledOutputArgs, ref nextLabel);
            }
        }

        else
            throw new InvalidOperationException($"Invalid coded pair: '{current?.GetType()?.Name ?? "null"}' and '{currentDefinition?.GetType()?.Name ?? "null"}'.");
    }

    private static T ExtractWithID<T>(Codec codec, string id)
        where T : Codec
    {
        foreach (Codec child in codec.Children)
        {
            if (child is T typed && child.GetAttribute(Codec.ID) == id)
                return typed;
        }
        return null;
    }

    /// <summary>
    /// Recursively mark all units reachable from one unit as reachable. 
    /// </summary>
    static void MarkVisited(Unit current, HashSet<Unit> marked)
    {
        if (current == null)
            return;
        if (marked.Contains(current))
            return;

        marked.Add(current);
        foreach (Unit output in current.To)
        {
            MarkVisited(output, marked);
        }
    }
}