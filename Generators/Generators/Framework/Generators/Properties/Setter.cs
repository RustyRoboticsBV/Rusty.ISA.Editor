/// <summary>
/// A setter generator.
/// </summary>
public sealed class Setter : Accessor, ISummarizable<Setter>
{
    public new Setter This => this;

    public Setter() : base() { }

    public Setter(Statement statement) : base(statement) { }

    public static implicit operator Setter(Statement statement) => new(statement);

    protected override void GenerateHeader(CodeWriter w) => w.Append("set");
}