using Godot;

namespace Rusty.ISA.Editor;

public class Preview
{
    private const string SkeletonCode = """
func eval() -> String:
    EVAL
""";

    public Preview(string code)
    {
        string source = SkeletonCode.Replace("EVAL", code);
    }
}