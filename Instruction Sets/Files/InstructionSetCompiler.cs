using Godot;
using System;
using System.Text;
using Rusty.Xml;
using Rusty.Cutscenes;
using Rusty.CutsceneImporter.InstructionDefinitions;
using System.IO;

namespace Rusty.CutsceneEditor.InstructionSets
{
    /// <summary>
    /// A class that can combine an instruction set consisting of built-in instruction definitions with a folder containing
    /// user-defined instruction definitions.
    /// </summary>
    public static class InstructionSetCompiler
    {
        public static void Compile(InstructionSet set, string filePath)
        {
            string absolutePath = PathUtility.GetPath(filePath);
            
            string folderPath = Path.GetDirectoryName(absolutePath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                GD.Print($"Created directory at '{folderPath}'.");
            }

            ZipPacker packer = new ZipPacker();
            Error error = packer.Open(absolutePath);
            if (error != Error.Ok)
                throw new Exception($"Could not open file '{absolutePath}' due to error code '{error}'!");

            for (int i = 0; i < set.Definitions.Length; i++)
            {
                InstructionDefinition def = set.Definitions[i];

                packer.StartFile($"{def.Category}/{def.Opcode}/def{def.Opcode}.xml");
                Document doc = Serializer.Compile(def);
                try
                {
                    doc.Root.GetChild("icon").InnerText = $"icon{def.Opcode}.png";
                }
                catch { }
                packer.WriteFile(Encoding.ASCII.GetBytes(doc.GenerateXml()));
                packer.CloseFile();

                if (def.Icon != null)
                {
                    packer.StartFile($"{def.Category}/{def.Opcode}/icon{def.Opcode}.png");
                    packer.WriteFile(def.Icon.GetImage().SavePngToBuffer());
                    packer.CloseFile();
                }
            }

            packer.Close();
        }
    }
}