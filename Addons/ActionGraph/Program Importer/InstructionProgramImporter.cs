using Godot;
using Rusty.ActionGraph.CodeGen;
using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.ImportPlugins;

/// <summary>
/// An importer for instruction programs. Serves as an entry point for the GDScript-based import plugin.
/// </summary>
[GlobalClass]
public abstract partial class InstructionProgramImporter : Node
{
    //// <summary>
    /// Load a file as an InstructionProgram resource.
    /// </summary>
    public static InstructionProgram Import(string path)
    {
        if (path.EndsWith(".agbp"))
            return ImportBinary(FileAccess.GetFileAsBytes(path));
        else if (path.EndsWith(".agxp"))
            return ImportXml(FileAccess.GetFileAsString(path));
        else
            throw new System.IO.FileLoadException($"Invalid file extension for file at: {path}");
    }

    /// <summary>
    /// Load a string of bytes as an InstructionProgram resource.
    /// </summary>
    public static InstructionProgram ImportBinary(byte[] bytes)
    {
        // Parse the file as a codec.
        FileCodec codec = BinaryParser.Parse(bytes);

        // Compile the codec into a program.
        InstructionProgram program = CodeGenerator.Generate(codec);
        return program;
    }

    /// <summary>
    /// Load a string of XML as an InstructionProgram resource.
    /// </summary>
    public static InstructionProgram ImportXml(string text)
    {
        // Parse the XML as a codec.
        FileCodec codec = Serialization.XmlParser.Parse(text);

        // Compile the codec into a program.
        InstructionProgram program = CodeGenerator.Generate(codec);
        return program;
    }
}