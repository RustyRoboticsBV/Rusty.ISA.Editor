/// <summary>
/// A getter generator.
/// </summary>
public sealed class Getter : Accessor
{
    /* Protected methods. */
    protected override void GenerateHeader(CodeWriter w) => w.Append("get");
}