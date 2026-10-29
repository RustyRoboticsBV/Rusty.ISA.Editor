/// <summary>
/// A base class for comment generators.
/// </summary>
public abstract class Commentary : Statement
{
    /* Public properties. */
    public override bool SingleLine => !Text.Contains('\n');

    public string Text { get; set; }
}