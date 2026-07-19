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
                units.Add(id, unit);
            }
            else if (element is JointCodec joint)
            {
                Unit unit = new(1);
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
        List<Instruction> instructions = new();
        Dictionary<Unit, string> labels = new();
        int nextLabel = 0;
        visited.Clear();
        foreach (Unit startUnit in starts)
        {
            Compile(startUnit, instructions, visited, labels, nextLabel);
        }

        // Compile.
        Metadata metadata = Compile(meta);
        InstructionSet iset = Compile(instrs);

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
    private static void Compile(Unit unit, List<Instruction> instructions, HashSet<Unit> visited, Dictionary<Unit, string> labels, int nextLabel)
    {
        // Mark unit as visited.
        visited.Add(unit);

        // Compile contents.
        if (unit.Contents == null)
            instructions.Add(new DummyInstruction());
        else
        {
            // Compile contents.
            // TODO: implement.
            GenericInstruction instruction = new("TEST", []);
            instructions.Add(instruction);

            // Mark with start point if necessary.
            StartCodec start = unit.Contents.GetFirstChild<StartCodec>();
            if (start != null)
                instruction.Start = start.GetAttribute(Codec.ID);
        }

        // Handle outputs.
        foreach (Unit output in unit.Outputs)
        {
            // If empty, generate end instruction.
            if (output == null)
                instructions.Add(new EndInstruction());

            // If not empty...
            else
            {
                // If the unit was already compiled, generate goto instruction.
                if (visited.Contains(output))
                {
                    // Generate label if necessary.
                    if (!labels.ContainsKey(output))
                    {
                        labels.Add(output, nextLabel.ToString());
                        nextLabel++;
                    }

                    // Generate goto.
                    instructions.Add(new GotoInstruction(labels[output]));
                }

                // Else, compile output unit.
                else
                    Compile(output, instructions, visited, labels, nextLabel);
            }
        }
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