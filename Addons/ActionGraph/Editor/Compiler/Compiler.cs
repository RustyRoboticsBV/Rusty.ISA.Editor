using System.Collections.Generic;
using Rusty.ActionGraph.Runtime;
using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.Compilation;

public static class Compiler
{
    public static InstructionProgram Compile(FileCodec file)
    {
        Godot.GD.Print(XmlLoader.Serialize(file));
        // Collect nodes, joints & edges.
        List<NodeCodec> nodes = file.GetFirstChild<GraphCodec>()?.GetFirstChild<ElemsCodec>()?.GetChildren<NodeCodec>() ?? [];
        List<JointCodec> joints = file.GetFirstChild<GraphCodec>()?.GetFirstChild<ElemsCodec>()?.GetChildren<JointCodec>() ?? [];
        List<EdgeCodec> edges = file.GetFirstChild<GraphCodec>()?.GetFirstChild<EdgesCodec>()?.GetChildren<EdgeCodec>() ?? [];

        InstructionSet iset = Compile(file?.GetFirstChild<SchemaCodec>()?.GetFirstChild<InstrsCodec>());

        return new(new(), iset, []);
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
    private static InstructionDefinition Compile(IdefCodec codec)
    {
        // Read opcode.
        string opcode = codec.GetAttribute(Codec.ID);

        // Read parameters.
        var pdefs = codec.GetChildren<PdefCodec>();
        string[] parameters = new string[pdefs.Count];
        for (int i = 0; i < pdefs.Count; i++)
        {
            parameters[i] = pdefs[i].GetAttribute(Codec.ID);
        }

        // Read execution handler.
        string executionHandler = codec.GetFirstChild<ExecCodec>()?.InnerText ?? "";

        // Create definition.
        return new(opcode, parameters, executionHandler);
    }
}