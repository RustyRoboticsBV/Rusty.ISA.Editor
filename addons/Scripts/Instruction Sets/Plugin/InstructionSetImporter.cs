using Godot;
using Godot.Collections;
using System;
using Rusty.ISA;

namespace Rusty.ISA.Importer.InstructionSets
{
    /// <summary>
    /// An importer for ZIP-based ISA instruction sets.
    /// </summary>
    [Tool]
    [GlobalClass]
    public partial class InstructionSetImporter : Node
    {
        /* Public methods. */
        /// <summary>
        /// Load an instruction set from a file.
        /// </summary>
        public static InstructionSet Import(string filePath, Dictionary importOptions)
        {
            // Decompile.
            try
            {
                return Deserializer.Deserialize(filePath);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Could not import instruction set at '{filePath}' due to exception: '{ex.Message}'.");
                return null;
            }
        }
    }
}