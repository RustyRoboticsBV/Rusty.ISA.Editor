using Rusty.ActionGraph.Runtime;
using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.Compilation;

public static class Compiler
{
    public static InstructionProgram Compile(FileCodec file)
    {
        Godot.GD.Print(XmlLoader.Serialize(file));
        return new(new(), new(), []);
    }
}
