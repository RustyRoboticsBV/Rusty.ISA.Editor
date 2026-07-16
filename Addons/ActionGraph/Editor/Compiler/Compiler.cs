using System.Collections.Generic;
using Rusty.ActionGraph.Runtime;
using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.Compilation;

public static class Compiler
{
    public static InstructionProgram Compile(FileCodec file)
    {
        // Collect nodes, joints & edges.
        List<NodeCodec> nodes = file.GetFirstChild<GraphCodec>()?.GetFirstChild<ElemsCodec>()?.GetChildren<NodeCodec>() ?? [];
        List<JointCodec> joints = file.GetFirstChild<GraphCodec>()?.GetFirstChild<ElemsCodec>()?.GetChildren<JointCodec>() ?? [];
        List<EdgeCodec> edges = file.GetFirstChild<GraphCodec>()?.GetFirstChild<EdgesCodec>()?.GetChildren<EdgeCodec>() ?? [];

        return new(new(), new(), []);
    }
}