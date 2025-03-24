using Godot;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Rusty.ISA.Editor.SetBuilder
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
					definitions.Add(builtIn[i]);
				}
			}

			// Recursively load folder.
			HandleDirectory(definitions, absolutePath);

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
				{
					string xml = File.ReadAllText(files[i]);

					XmlDocument doc = new();
					doc.LoadXml(xml);

					InstructionDefinitionDescriptor descriptor = new(doc);

					definitions.Add(descriptor.Generate(true));
				}
				else if (files[i].EndsWith(".zip"))
				{
					InstructionSetDescriptor descriptor = new();
				}
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