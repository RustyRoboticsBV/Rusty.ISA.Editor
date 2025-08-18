namespace Rusty.ISA.Editor;

/// <summary>
/// A base class for compilers.
/// </summary>
public abstract class Compiler
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