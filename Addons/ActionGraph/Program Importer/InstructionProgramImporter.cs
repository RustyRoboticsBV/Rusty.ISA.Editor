using Godot;
using Rusty.ActionGraph.Compilation;
using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.ImportPlugins;

/// <summary>
/// An importer for instruction programs. Serves as an entry point for the GDScript-based import plugin.
/// </summary>
[GlobalClass]
public abstract partial class InstructionProgramImporter : Node
{
    /// <summary>
    /// Load a string of XML as a InstructionProgram resource.
    /// </summary>
    public static InstructionProgram Import(string xml)
    {
        // Parse the XML as a codec.
        FileCodec codec = Parser.Parse(xml);
        Godot.GD.Print(Serializer.Serialize(codec));

        // Compile the codec into a program.
        return Compiler.Compile(codec);
    }
}
