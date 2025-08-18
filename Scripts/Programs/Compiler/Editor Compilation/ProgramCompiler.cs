namespace Rusty.ISA.Editor;

/// <summary>
/// A compiler for programs.
/// </summary>
public abstract class ProgramCompiler : CompilerTool
{
    /* Constructors. */
    public static RootNode Compile(Ledger ledger)
    {
        // Compile metadata.
        RootNode metadata = MetadataCompiler.Compile(ledger);

        // Compile graph.
        RootNode graph = GraphCompiler.Compile(ledger);

        // Wrap in program node.
        RootNode program = MakeRoot(ledger.Set, BuiltIn.ProgramOpcode);
        program.AddChild(metadata);
        program.AddChild(graph);
        program.AddChild(MakeSub(ledger.Set, BuiltIn.EndOfGroupOpcode));

        // Compute checksum.
        string checksum = program.ComputeChecksum();
        program.GetChildWith(BuiltIn.MetadataOpcode)
            .GetChildWith(BuiltIn.ChecksumOpcode)
            .SetArgument(BuiltIn.ChecksumValue, checksum);

        return program;
    }
}