public interface IGenerator<T>
    where T : Generator
{
    public T This { get; }
}