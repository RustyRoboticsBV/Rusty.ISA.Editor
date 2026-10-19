/// <summary>
/// A property accessor generator.
/// </summary>
public abstract class Accessor : Function, ISummarizable<Accessor>
{
    public new Accessor This => this;

    public Accessor() : base() { }

    public Accessor(Statement statement) : base(statement) { }
}