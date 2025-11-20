namespace WScoreDomain.Common;

public record LinkDto(string Rel, string Href, string Method);

public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int Page { get; set; } = 1;

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 0 ? value : 10;
    }

    public int TotalItems { get; set; }
    public int TotalPages =>
        PageSize > 0 ? (int)Math.Ceiling((double)TotalItems / PageSize) : 1;

    public List<LinkDto> Links { get; set; } = new();
}
