namespace Application.Common;

public class PagedList<T>
{
    public IList<T>? Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasNextPage => Page * PageSize < TotalCount;
    public bool HasPerviosPage => Page > 1;

    public static PagedList<T>? Create(int page, int pageSize)
    {
        return default;
    }
}
