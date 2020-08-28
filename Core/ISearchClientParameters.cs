namespace Core
{
    public interface ISearchClientParameters
    {
        int? Top { get; }
        string Filter { get; }
    }
}