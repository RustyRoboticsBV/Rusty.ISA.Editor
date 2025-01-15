using Godot;
using System.Text;
using Rusty.Xml;
using Rusty.Cutscenes;
using Rusty.CutsceneImporter.InstructionDefinitions;

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
            ZipPacker packer = new ZipPacker();
            Error error = packer.Open(filePath);
            if (error != Error.Ok)
                throw new System.Exception($"Could not open file '{filePath}'!");

            for (int i = 0; i < set.Definitions.Length; i++)
            {
                InstructionDefinition def = set.Definitions[i];

                packer.StartFile($"{def.Category}/{def.Opcode}/def{def.Opcode}.xml");
                Document doc = InstructionDefinitionCompiler.Compile(def);
                doc.Root.GetChild("icon").InnerText = $"icon{def.Opcode}.png";
                packer.WriteFile(Encoding.ASCII.GetBytes(doc.GenerateXml()));
                packer.CloseFile();

                packer.StartFile($"{def.Category}/{def.Opcode}/icon{def.Opcode}.png");
                packer.WriteFile(def.Icon.GetImage().SavePngToBuffer());
                packer.CloseFile();
            }

            packer.Close();
        }
    }
}