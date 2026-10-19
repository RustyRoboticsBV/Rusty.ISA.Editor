public interface ISummarizable<T> : IGenerator<T>
    where T : Generator
{
    public Summary Summary { get; set; }

    public T AddSummary(Summary summary)
    {
        Summary = summary;
        return This;
    }
}