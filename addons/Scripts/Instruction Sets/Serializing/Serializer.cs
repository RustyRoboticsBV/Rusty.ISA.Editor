using Godot;
using System;
using System.Text;
using Rusty.Xml;
using Rusty.ISA;
using DefinitionKeywords = Rusty.ISA.Importer.InstructionDefinitions.Keywords;
using DefinitionSerializer = Rusty.ISA.Importer.InstructionDefinitions.Serializer;

namespace Rusty.ISA.Importer.InstructionSets
{
    /// <summary>
    /// A class that save an instruction set to a file.
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Save an instruction set to a file.
        /// </summary>
        public static void Serialize(InstructionSet set, string filePath)
        {
            // Get global file path.
            string absolutePath = PathUtility.GetPath(filePath);

            // Create ZIP file.
            ZipPacker packer = new();
            Error error = packer.Open(absolutePath);
            if (error != Error.Ok)
                throw new Exception($"Could not open file '{absolutePath}' due to error code '{error}'!");

            // Write all instructions to the file.
            string index = "";
            for (int i = 0; i < set.Definitions.Length; i++)
            {
                InstructionDefinition def = set[i];

                // Get opcode.
                string opcode = def.Opcode;

                // Get category.
                string category = def.Category;
                if (category == "")
                    category = Keywords.UndefinedCategory;

                // Add definition file to ZIP archive.
                packer.StartFile($"{category}/{opcode}/{Keywords.DefinitionFilename}");
                Document doc = null;//DefinitionSerializer.Serialize(def);
                try
                {
                    doc.Root.GetChild(DefinitionKeywords.Icon).InnerText = $"{Keywords.IconFilename}";
                }
                catch { }
                packer.WriteFile(Encoding.ASCII.GetBytes(doc.GenerateXml()));
                packer.CloseFile();

                // Add icon file to ZIP archive.
                if (def.Icon != null)
                {
                    packer.StartFile($"{category}/{opcode}/{Keywords.IconFilename}");
                    packer.WriteFile(def.Icon.GetImage().SavePngToBuffer());
                    packer.CloseFile();
                }

                // Add to index string.
                if (index != "")
                    index += "\n";
                index += $"{category}/{opcode}";
            }

            // Create index file.
            packer.StartFile(Keywords.IndexFilename);
            packer.WriteFile(Encoding.ASCII.GetBytes(index));
            packer.CloseFile();

            // Close the ZIP file.
            packer.Close();
        }
    }
}