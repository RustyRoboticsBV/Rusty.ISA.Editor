/// <summary>
/// A getter generator.
/// </summary>
public sealed class Getter : Accessor, ISummarizable<Getter>
{
    public new Getter This => this;

    public Getter() : base() { }

    public Getter(Statement statement) : base(statement) { }

    public static implicit operator Getter(Statement statement) => new(statement);

    protected override void GenerateHeader(CodeWriter w) => w.Append("get");
}