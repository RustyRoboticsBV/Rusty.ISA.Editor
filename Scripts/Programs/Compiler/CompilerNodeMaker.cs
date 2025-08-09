using System.Collections.Generic;

namespace Rusty.ISA.Editor;

public static class CompilerNodeMaker
{
    public static RootNode MakeRoot(InstructionSet set, string opcode)
    {
        RootNode root = new();
        root.Data = new(set, opcode);
        return root;
    }

    public static SubNode MakeSub(InstructionSet set, string opcode)
    {
        SubNode sub = new();
        sub.Data = new(set, opcode);
        return sub;
    }

    public static SubNode MakeSub(InstructionSet set, Inspector inspector)
    {
        string opcode = inspector.ReadMetaData(InstructionInspectorFactory.Opcode);

        // Compile inspector header.
        SubNode instructionInspector = MakeSub(set, BuiltIn.InspectorOpcode);

        // Compile contents.
        List<SubNode> preInstructions = new();
        SubNode contents = MakeSub(set, opcode);
        List<SubNode> postInstructions = new();
        for (int i = 0; i < inspector.GetContentsCount(); i++)
        {
            string key = inspector.GetKey(i);
            IGuiElement element = inspector.GetAt(i);
            if (key.StartsWith(InstructionInspectorFactory.PreInstruction))
            {
                string id = key.Substring(InstructionInspectorFactory.PreInstruction.Length);
                if (element is Inspector nestedInspector)
                    preInstructions.Add(MakeSub(set, nestedInspector));
            }
            if (key.StartsWith(InstructionInspectorFactory.Parameter))
            {
                string id = key.Substring(InstructionInspectorFactory.Parameter.Length);
                if (element is IField field)
                    contents.SetArgument(id, field.Value);
            }
            if (key.StartsWith(InstructionInspectorFactory.PostInstruction))
            {
                string id = key.Substring(InstructionInspectorFactory.PostInstruction.Length);
                if (element is Inspector nestedInspector)
                    postInstructions.Add(MakeSub(set, nestedInspector));
            }
        }

        // Compile end-of-group.
        instructionInspector.AddChild(MakeSub(set, BuiltIn.EndOfGroupOpcode));

        return contents;
    }
}