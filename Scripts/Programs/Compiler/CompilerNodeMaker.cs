namespace Rusty.ISA.Editor;

/// <summary>
/// An utility for creating compiler nodes.
/// </summary>
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
}