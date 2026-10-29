/// <summary>
/// A setter generator.
/// </summary>
public sealed class Setter : Accessor
{
    protected override void GenerateHeader(CodeWriter w) => w.Append("set");
}