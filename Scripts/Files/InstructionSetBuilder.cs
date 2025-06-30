using Godot;
using System.Collections.Generic;
using System.IO;

namespace Rusty.ISA.Editor;

/// <summary>
/// A class that can combine an instruction set consisting of built-in instruction definitions with a folder containing
/// user-defined instruction definitions.
/// </summary>
public static class InstructionSetBuilder
{
    /* Public methods. */
    /// <summary>
    /// Build an instruction set, from a built-in instruction set and a folder containing all the user-defined instruction
    /// definitions.
    /// </summary>
    public static InstructionSet Build(InstructionSet builtIn, string folderPath)
    {
        List<InstructionDefinition> definitions = new();
        List<InstructionSet> modules = new();

        // Get absolute path to folder.
        string absolutePath = PathUtility.GetPath(folderPath);

        // Create folder if it didn't exist yet.
        if (!Directory.Exists(absolutePath))
        {
            Directory.CreateDirectory(absolutePath);
            GD.Print($"Created definitions folder at '{absolutePath}'.");
        }

        // Copy over built-in instructions.
        if (builtIn != null)
        {
            for (int i = 0; i < builtIn.Count; i++)
            {
                InstructionDefinition definition = builtIn[i].DuplicateDeep() as InstructionDefinition;
                definitions.Add(definition);
            }
        }

        // Recursively load folder contents.
        HandleDirectory(definitions, modules, absolutePath, absolutePath);

        // Create instruction set and return it.
        return new InstructionSet(definitions.ToArray(), modules.ToArray());
    }

    /* Private methods */
    /// <summary>
    /// Take all XML files in a directory hierarchy and try to load them as instruction definitions.
    /// </summary>
    private static void HandleDirectory(List<InstructionDefinition> definitions, List<InstructionSet> modules,
        string folderPath, string rootFolderPath)
    {
        // Handle files.
        string[] files = Directory.GetFiles(folderPath);
        for (int i = 0; i < files.Length; i++)
        {
            // Case 1: Definition file
            if (files[i].EndsWith(".xml"))
            {
                // Read XML file.
                string xml = File.ReadAllText(files[i]);

                // Deserialize and add to list.
                InstructionDefinition definition = (InstructionDefinition)XmlDeserializer.Deserialize(xml, folderPath);
                definitions.Add(definition);
            }

            // Case 2: Module file.
            else if (files[i].EndsWith(".zip"))
            {
                InstructionSet module = SetDeserializer.Deserialize(files[i]);
                modules.Add(module);
            }
        }

        // Handle sub-directories.
        string[] subFolders = Directory.GetDirectories(folderPath);
        for (int i = 0; i < subFolders.Length; i++)
        {
            HandleDirectory(definitions, modules, subFolders[i], rootFolderPath);
        }
    }
}