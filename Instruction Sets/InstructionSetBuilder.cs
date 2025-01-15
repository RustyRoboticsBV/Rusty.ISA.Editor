using System.Collections.Generic;
using System.IO;
using Godot;
using Rusty.CutsceneImporter.InstructionDefinitions;
using Rusty.Cutscenes;

namespace Rusty.CutsceneEditor.InstructionSets
{
	/// <summary>
	/// A class that can combine an instruction set consisting of built-in instruction definitions with a folder containing
	/// user-defined instruction definitions.
	/// </summary>
	public static class InstructionSetBuilder
	{
		/// <summary>
		/// Build an instruction set, from a built-in instruction set and a folder containing all the user-defined instruction
		/// definitions.
		/// </summary>
		public static InstructionSet Build(InstructionSet builtIn, string folderPath)
		{
			List<InstructionDefinition> definitions = new();

			// Copy over built-in instructions.
			for (int i = 0; i < builtIn.Definitions.Length; i++)
			{
				definitions.Add(builtIn.Definitions[i]);
			}

			// Recursively load folder.
			HandleDirectory(definitions, folderPath);

            // Create instruction set and return it.
            return new InstructionSet(definitions.ToArray());
		}

		/* Private methods */
		/// <summary>
		/// Take all XML files in a directory hierarchy and try to load them as instruction definitions.
		/// </summary>
		private static void HandleDirectory(List<InstructionDefinition> definitions, string folderPath)
        {
            // Handle files.
            string[] files = Directory.GetFiles(folderPath);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].EndsWith(".xml"))
                    definitions.Add(InstructionDefinitionImporter.Import(files[i], new()));
            }

			// Handle sub-directories.
			string[] subFolders = Directory.GetDirectories(folderPath);
			for (int i = 0; i < subFolders.Length; i++)
			{
				HandleDirectory(definitions, subFolders[i]);
			}
        }
	}
}