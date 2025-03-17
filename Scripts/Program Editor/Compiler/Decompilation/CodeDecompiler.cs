using System.Collections.Generic;
using Rusty.Csv;
using Rusty.ISA;

namespace Rusty.ISA.Editor.Compiler
{
    /// <summary>
    /// Decompile a string of code into a list of instruction instances.
    /// </summary>
    public abstract class CodeDecompiler
    {
        /* Public methods. */
        /// <summary>
        /// Decompile an instruction program code string into a list of instruction instances.
        /// </summary>
        public static List<InstructionInstance> Decompile(InstructionSet set, string code)
        {
            // Load as CSV table.
            CsvTable table = new CsvTable("Program", code);

            // Convert to instruction list.
            List<InstructionInstance> instructions = new();
            for (int instruction = 0; instruction < table.Height; instruction++)
            {
                // Get the opcode.
                string opcode = table.GetCell(0, instruction);

                // Get the arguments.
                int parameterCount = table.Width - 1;
                try
                {
                    parameterCount = set[opcode].Parameters.Length;
                }
                catch { }

                string[] arguments = new string[parameterCount];
                for (int arg = 0; arg < parameterCount; arg++)
                {
                    try
                    {
                        arguments[arg] = table.GetCell(arg + 1, instruction);
                    }
                    catch
                    {
                        arguments[arg] = "";
                    }
                }

                // Create instruction.
                instructions.Add(new InstructionInstance(opcode, arguments));
            }

            return instructions;
        }
    }
}