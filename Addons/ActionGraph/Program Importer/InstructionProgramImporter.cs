using Godot;
using Rusty.ActionGraph.Compilation;
using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.ImportPlugins;

[GlobalClass]
public abstract partial class InstructionProgramImporter : Node
{
    /// <summary>
    /// Load a string of XML as a InstructionProgram resource.
    /// </summary>
    public static InstructionProgram Import(string xml)
    {
        // Parse the XML as a FileCodec.
        FileCodec file = Parser.Parse(xml);

        // Compile the FileCodec into a program.
        return Compiler.Compile(file);
    }
}
