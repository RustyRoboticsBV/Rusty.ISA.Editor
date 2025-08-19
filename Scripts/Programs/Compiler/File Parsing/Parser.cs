using Godot;
using System;
using Rusty.Csv;

namespace Rusty.ISA.Editor;

/// <summary>
/// A program file to syntax tree decompiler.
/// </summary>
public abstract class Parser : CompilerTool
{
    /* Public methods. */
    public static RootNode Parse(InstructionSet set, string code)
    {
        try
        {
            // Read CSV table.
            CsvTable csv = new("code", code);

            // Decompile instruction instances.
            InstructionInstance[] instances = new InstructionInstance[csv.Height];
            for (int i = 0; i < csv.Height; i++)
            {
                InstructionDefinition definition = set[csv[0, i]];
                instances[i] = new(definition);
                for (int j = 0; j < definition.Parameters.Length; j++)
                {
                    instances[i].Arguments[j] = csv[j + 1, i];
                }
            }

            // Create node tree.
            int currentIndex = 0;
            SubNode program = Decompile(set, instances, ref currentIndex);
            RootNode result = program.ToRoot();
            result.StealChildren(program);

            // Evaluate checksum.
            SubNode checksum = result?.GetChildWith(BuiltIn.MetadataOpcode)?.GetChildWith(BuiltIn.ChecksumOpcode);
            if (checksum != null)
            {
                string checksumOld = checksum.GetArgument(BuiltIn.ChecksumValue);
                string checksumNew = result?.ComputeChecksum();
                if (checksumNew != checksumOld)
                {
                    Log.Warning("The loaded program had a bad checksum! This means the program was either externally modified "
                        + "or corrupted, or that the instruction set was changed since last time that the result was last "
                        + "modified!"
                        + "\n- Old checksum: " + checksumOld
                        + "\n- New checksum: " + checksumNew);
                }
            }
            else
                Log.Warning("The loaded result had no checksum. No data validation could be done.");

            // Return finished tree's root node.
            return result;
        }
        catch (Exception exception)
        {
            code = code.Replace("\r\n", "\n");
            code = code.Replace("\r", "\n");
            Log.Error("The following result could not be loaded due to a syntax error:\n" + code);

            if (OS.HasFeature("editor"))
                throw new FormatException("", exception);

            return null;
        }
    }

    /* Private methods. */
    /// <summary>
    /// Decompile a subset of a list of instruction instances.
    /// </summary>
    private static SubNode Decompile(InstructionSet set, InstructionInstance[] instances, ref int currentIndex)
    {
        // Create node for current instruction.
        InstructionInstance instance = instances[currentIndex];

        SubNode node = MakeSub(set, instance.Opcode);
        node.Data.Instance = instance;

        // Determine how to proceed.
        currentIndex++;
        switch (instance.Opcode)
        {
            // Groups.
            case BuiltIn.ProgramOpcode:
            case BuiltIn.MetadataOpcode:
            case BuiltIn.InstructionSetOpcode:
            case BuiltIn.DefinitionOpcode:
            case BuiltIn.CompileRuleOpcode:
            case BuiltIn.GraphOpcode:
            case BuiltIn.CommentOpcode:
            case BuiltIn.FrameOpcode:
            case BuiltIn.JointOpcode:
            case BuiltIn.NodeOpcode:
            case BuiltIn.InspectorOpcode:
            case BuiltIn.PreInstructionOpcode:
            case BuiltIn.PostInstructionOpcode:
            case BuiltIn.OptionRuleOpcode:
            case BuiltIn.ChoiceRuleOpcode:
            case BuiltIn.TupleRuleOpcode:
            case BuiltIn.ListRuleOpcode:
            case BuiltIn.GotoGroupOpcode:
            case BuiltIn.EndGroupOpcode:
                while (currentIndex < instances.Length)
                {
                    SubNode child = Decompile(set, instances, ref currentIndex);
                    node.AddChild(child);
                    if (child.Opcode == BuiltIn.EndOfGroupOpcode)
                        break;
                }
                break;

            // Non-groups.
            default:
                break;
        }

        return node;
    }
}