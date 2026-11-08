using Godot;
using System.IO;
using Rusty.ActionGraph.CodeGen;
using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.ImportPlugins;

using FileAccess = Godot.FileAccess;
using XmlParser = Serialization.XmlParser;

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
            throw new FileLoadException($"Invalid file extension for file at: '{path}'.");
    }

    /// <summary>
    /// Load a string of bytes as an InstructionProgram resource.
    /// </summary>
    public static InstructionProgram ImportBinary(byte[] bytes)
    {
        // Parse the byte array as an element.
        FileElement element = BinaryParser.Parse(bytes);

        // Compile the element into a program.
        InstructionProgram program = CodeGenerator.Generate(element);
        return program;
    }

    /// <summary>
    /// Load a string of XML as an InstructionProgram resource.
    /// </summary>
    public static InstructionProgram ImportXml(string text)
    {
        // Parse the XML as an element.
        FileElement element = XmlParser.Parse(text);
        // TODO: remove ///
        var file = File.Create("Test/Test.agbp");
        file.Write(BinarySerializer.Serialize(element));
        file.Close();
        ///////////////////

        // Compile the element into a program.
        InstructionProgram program = CodeGenerator.Generate(element);
        return program;
    }
}