using Godot;
using Godot.Collections;
using System;
using System.IO;
using Rusty.ISA;
using Rusty.Csv;

namespace Rusty.ISA.Importer.InstructionPrograms
{
    /// <summary>
    /// An importer for CSV-based ISA instruction programs.
    /// </summary>
    [Tool]
    [GlobalClass]
    public partial class InstructionProgramImporter : Node
    {
        /* Public methods. */
        /// <summary>
        /// Load an instruction program from a file.
        /// </summary>
        public static Program Import(string filePath, Dictionary importOptions)
        {
            // Get global file & folder paths.
            string globalPath = ProjectSettings.GlobalizePath(filePath);
            string folderPath = Path.GetDirectoryName(globalPath);

            // Read file as CSV table.
            CsvTable csv = new(globalPath);

            // Decompile.
            try
            {
                return Deserializer.Deserialize(csv);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Could not import instruction program at '{globalPath}' due to exception: '{ex.Message}'.");
                return null;
            }
        }
    }
}