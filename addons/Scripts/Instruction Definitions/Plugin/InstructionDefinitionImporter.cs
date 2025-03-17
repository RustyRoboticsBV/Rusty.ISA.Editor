using Godot;
using Godot.Collections;
using System;
using System.IO;
using Rusty.ISA;

namespace Rusty.ISA.Importer.InstructionDefinitions
{
    /// <summary>
    /// An importer for XML-based ISA instruction definitions.
    /// </summary>
    [Tool]
    [GlobalClass]
    public partial class InstructionDefinitionImporter : Node
    {
        /* Public methods. */
        /// <summary>
        /// Load an instruction definition from a file.
        /// </summary>
        public static InstructionDefinition Import(string filePath, Dictionary importOptions)
        {
            // Get global file & folder paths.
            string globalPath = ProjectSettings.GlobalizePath(filePath);
            string folderPath = Path.GetDirectoryName(globalPath);

            // Read file.
            string xml = File.ReadAllText(globalPath);

            // Decompile.
            try
            {
                return Deserializer.Deserialize(xml, folderPath);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Could not import instruction definition at '{globalPath}' due to exception: '{ex.Message}'.");
                return null;
            }
        }
    }
}