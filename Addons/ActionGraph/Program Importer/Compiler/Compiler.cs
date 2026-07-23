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
                    throw new KeyNotFoundException($"Could not find node currentDefinition '{type}'.");

                // Count outputs.
                var outputs = OutputCountResult.Create(ndef, node);

                // Create unit.
                Unit unit = new(outputs.CountOutputs());
                unit.Contents = node;
                units.Add(id, unit);
            }
            else if (element is JointCodec joint)
            {
                Unit unit = new(1);
                unit.Contents = joint;
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

        // Find start units.
        HashSet<Unit> starts = new();
        HashSet<Unit> visited = new();
        foreach (var unit in units)
        {
            if (unit.Value.Input.Count == 0)
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
            CompileUnit(startUnit, file, visited, nextLabel);
        }

        // Combine instructions.
        List<Instruction> instructions = new();
        foreach (Unit unit in units.Values)
        {
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
    /// Retrieve the first child of some type from a codec. Throw an exception if it can't be found.
    /// </summary>
    private static T Extract<T>(Codec codec)
        where T : Codec
    {
        T child = codec.GetFirstChild<T>();
        if (child == null)
            throw new NullReferenceException($"Missing {typeof(T).Name} in {codec.GetType().Name}.");
        return child;
    }

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
    /// Compile a graph.
    /// </summary>
    private static void CompileUnit(Unit unit, FileCodec file, HashSet<Unit> visited, int nextLabel)
    {
        // Mark unit as visited.
        visited.Add(unit);

        // Compile contents.
        if (unit.Contents is JointCodec)
            unit.Compiled.Add(new DummyInstruction());
        else if (unit.Contents is NodeCodec node)
        {
            NdefCodec ndef = ExtractWithID<NdefCodec>(file, unit.Contents.GetAttribute(Codec.Type));

            // Compile contents.
            foreach (Codec child in unit.Contents.Children)
            {
                if (child is FormCodec or OptionCodec or ChoiceCodec or TupleCodec or ListCodec)
                    Compile(unit, ndef, child, node);
            }

            unit.Start = node.GetAttribute(Codec.Start);
        }

        // Handle outputs.
        foreach (Unit output in unit.Outputs)
        {
            // If empty, generate end instruction.
            if (output == null)
                unit.Compiled.Add(new EndInstruction());

            // If not empty...
            else
            {
                // If the unit was already compiled, generate goto instruction.
                if (visited.Contains(output))
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

                // Else, compile output unit.
                else
                    CompileUnit(output, file, visited, nextLabel);
            }
        }
    }

    private static void Compile(Unit unit, Codec parentDefinition, Codec parent, Codec current)
    {
        // Find child definition.
        string childInspectorType = current.GetAttribute(Codec.Type);
        Codec currentDefinition = ExtractWithID<Codec>(parentDefinition, childInspectorType);

        // Compile form.
        if (current is FormCodec form)
        {
            FdefCodec fdef = ExtractWithID<FdefCodec>(parent, form.GetAttribute(Codec.Type));
            string opcode = fdef.GetAttribute(Codec.Type);
            List<string> arguments = new();
            foreach (Codec child in current.Children)
            {
                if (child is VadefCodec varg)
                    arguments.Add(varg.InnerText);
                else if (child is OadefCodec oarg)
                    arguments.Add("");
            }
            unit.Compiled.Add(new GenericInstruction(opcode, arguments.ToArray()));
        }

        // Compile option.
        else if (current is OptionCodec option)
        {
            Codec child = option.GetFirstChild<Codec>();
            string childType = child.GetAttribute(Codec.Type);
            Compile(unit, currentDefinition, current, child);
        }

        // Compile choice.
        else if (current is ChoiceCodec choice)
        {
            Codec child = choice.GetFirstChild<Codec>();
            string childType = child.GetAttribute(Codec.Type);
            Compile(unit, currentDefinition, current, child);
        }

        // Compile tuple.
        else if (current is TupleCodec tuple)
        {
            foreach (Codec child in tuple.Children)
            {
                string childType = child.GetAttribute(Codec.Type);
                Compile(unit, currentDefinition, current, child);
            }
        }

        // Compile list.
        else if (current is ListCodec list)
        {
            foreach (Codec child in list.Children)
            {
                string childType = child.GetAttribute(Codec.Type);
                Compile(unit, currentDefinition, current, child);
            }
        }
    }

    private static T ExtractWithID<T>(Codec codec, string id)
        where T : Codec
    {
        foreach (Codec child in codec.Children)
        {
            if (child is T typed && child.GetAttribute(id) == id)
                return typed;
        }
        return null;
    }

    private static void Compile(Unit unit, FormCodec form, FdefCodec fdef, InstructionDefinition definition)
    {
        GenericInstruction instruction = new(definition.Opcode, new string[definition.Parameters.Length]);
        unit.Compiled.Add(instruction);
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
        foreach (Unit output in current.Outputs)
        {
            MarkVisited(output, marked);
        }
    }
}