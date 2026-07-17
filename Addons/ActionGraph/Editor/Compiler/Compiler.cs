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
        // Collect nodes, joints & edges.
        var elements = GetElements(file?.GetFirstChild<GraphCodec>()?.GetFirstChild<ElemsCodec>());
        var edges = file.GetFirstChild<GraphCodec>()?.GetFirstChild<EdgesCodec>()?.GetChildren<EdgeCodec>() ?? [];

        // Compile into units.
        foreach (var element in elements)
        {

        }

        // Compile.
        Metadata metadata = Compile(file?.GetFirstChild<MetaCodec>());
        InstructionSet iset = Compile(file?.GetFirstChild<SchemaCodec>()?.GetFirstChild<InstrsCodec>());
        List<Instruction> instructions = Compile(iset, file.GetFirstChild<GraphCodec>());

        return new(metadata, iset, instructions.ToArray());
    }

    /* Private methods. */
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
    private static List<Instruction> Compile(InstructionSet iset, GraphCodec graph)
    {
        List<Instruction> instructions = new();

        return instructions;
    }
}