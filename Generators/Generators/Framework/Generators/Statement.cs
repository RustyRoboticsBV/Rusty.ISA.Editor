/// <summary>
/// A base class for implementation generators.
/// </summary>
public abstract class Statement : Generator
{
    public abstract bool SingleLine { get; }
}