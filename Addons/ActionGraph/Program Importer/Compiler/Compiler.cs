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
        // Get meta, schema & graph codecs and subcontainers.
        MetaCodec meta = Extract<MetaCodec>(file);
        SchemaCodec schema = Extract<SchemaCodec>(file);
        InstrsCodec instrs = Extract<InstrsCodec>(schema);
        NodesCodec nodes = Extract<NodesCodec>(schema);
        GraphCodec graph = Extract<GraphCodec>(file);
        ElemsCodec elems = Extract<ElemsCodec>(graph);
        EdgesCodec edges = Extract<EdgesCodec>(graph);

        // Compile metadata & instruction set.
        Metadata metadata = Compile(meta);
        InstructionSet iset = Compile(instrs);

        // Create units for nodes & joints.
        Dictionary<string, Unit> units = new();
        Dictionary<string, Codec> elements = GetElements(elems);
        foreach (Codec element in elems.Children)
        {
            string id = element.GetAttribute(Codec.ID);
            if (element is NodeCodec node)
            {
                // Find ndef.
                string type = node.GetAttribute(Codec.Type);
                NdefCodec ndef = nodes.FindNode(type);
                if (ndef == null)
                    throw new KeyNotFoundException($"Could not find node definition '{type}'.");

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
        foreach (var edge in edges.Children)
        {
            FromCodec from = Extract<FromCodec>(edge);
            PortCodec port = Extract<PortCodec>(edge);
            ToCodec to = Extract<ToCodec>(edge);

            string fromStr = from.GetAttribute(Codec.Element);
            string portStr = port.GetAttribute(Codec.Index);
            string toStr = to.GetAttribute(Codec.Element);
            
            Unit fromUnit = units[fromStr];
            int portIndex = int.Parse(portStr);
            Unit toUnit = units[toStr];

            fromUnit.ConnectTo(portIndex, toUnit);
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
            Compile(startUnit, visited, nextLabel);
        }

        // Combine instructions.
        List<Instruction> instructions = new();
        foreach (Unit unit in units.Values)
        {
            // Apply label to first instruction in unit.
            if (unit.Label != null && unit.Compiled.Count > 0)
                unit.Compiled[0].Label = unit.Label;
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
    private static Dictionary<string, Codec> GetElements(ElemsCodec elems)
    {
        Dictionary<string, Codec> elements = new();
        foreach (var element in elems.Children)
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
    /// Compile an instruction set.
    /// </summary>
    private static Metadata Compile(MetaCodec meta)
    {
        Metadata metadata = new();
        if (meta == null)
            return metadata;

        var datas = meta.GetChildren<DataCodec>();
        foreach (var data in datas)
        {
            metadata.AddValue(data.GetAttribute(Codec.ID), data.InnerText);
        }
        return metadata;
    }

    /// <summary>
    /// Compile an instruction set.
    /// </summary>
    private static InstructionSet Compile(InstrsCodec instrs)
    {
        if (instrs == null)
            return new();

        // Read instructions.
        var idefs = instrs.GetChildren<IdefCodec>();
        InstructionDefinition[] definitions = new InstructionDefinition[idefs.Count];
        for (int i = 0; i < idefs.Count; i++)
        {
            definitions[i] = Compile(idefs[i]);
        }

        // Create instruction set.
        return new(definitions);
    }

    /// <summary>
    /// Compile an instruction definition.
    /// </summary>
    private static InstructionDefinition Compile(IdefCodec idef)
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
        string executionHandler = idef.GetFirstChild<ExecCodec>()?.InnerText ?? "";

        // Create definition.
        return new(opcode, parameters, executionHandler);
    }

    /// <summary>
    /// Compile a graph.
    /// </summary>
    private static void Compile(Unit unit, HashSet<Unit> visited, int nextLabel)
    {
        // Mark unit as visited.
        visited.Add(unit);

        // Compile contents.
        if (unit.Contents is JointCodec)
            unit.Compiled.Add(new DummyInstruction());
        else if (unit.Contents is NodeCodec)
        {
            // Compile contents.
            foreach (Codec child in unit.Contents.Children)
            {
                // Mark with start point if necessary.
                if (child is StartCodec start)
                    unit.Start = start.GetAttribute(Codec.ID);

                // Compile form.
                else if (child is FormCodec form)
                {
                    string type = form.GetAttribute(Codec.Type);
                    unit.Compiled.Add(new GenericInstruction()); // TODO
                }

                // Compile option.
                else if (child is OptionCodec option)
                {
                    string type = option.GetAttribute(Codec.Type);
                    unit.Compiled.Add(new GenericInstruction()); // TODO
                }

                // Compile choice.
                else if (child is ChoiceCodec choice)
                {
                    string type = choice.GetAttribute(Codec.Type);
                    unit.Compiled.Add(new GenericInstruction()); // TODO
                }

                // Compile tuple.
                else if (child is TupleCodec tuple)
                {
                    string type = tuple.GetAttribute(Codec.Type);
                    unit.Compiled.Add(new GenericInstruction()); // TODO
                }

                // Compile list.
                else if (child is ListCodec list)
                {
                    string type = list.GetAttribute(Codec.Type);
                    unit.Compiled.Add(new GenericInstruction()); // TODO
                }
            }
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
                    Compile(output, visited, nextLabel);
            }
        }
    }

    private static void Compile(Unit unit, FormCodec form, InstructionDefinition definition)
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