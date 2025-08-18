namespace Rusty.ISA.Editor;

/// <summary>
/// A base class for syntax tree compilers and decompilers.
/// </summary>
public abstract class CompilerTool
{
    /* Protected methods. */
    /// <summary>
    /// Create a root node.
    /// </summary>
    protected static RootNode MakeRoot(InstructionSet set, string opcode)
    {
        RootNode root = new();
        root.Data = new(set, opcode);
        return root;
    }

    /// <summary>
    /// Create a child node.
    /// </summary>
    protected static SubNode MakeSub(InstructionSet set, string opcode)
    {
        SubNode sub = new();
        sub.Data = new(set, opcode);
        return sub;
    }
}