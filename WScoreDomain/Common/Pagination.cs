namespace WScoreDomain.Common;

public record PaginationQuery(int Page = 1, int PageSize = 10);

public record LinkDto(string Rel, string Href, string Method);

public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int Page { get; set; }
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 0 ? value : 10;
    }
    public int TotalItems { get; set; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalItems / PageSize) : 1;

    // HATEOAS
    public List<LinkDto> Links { get; set; } = new();
}

/// <summary>
/// Opcional: use se quiser envelopar qualquer recurso com links.
/// Se não estiver usando, pode remover sem problemas.
/// </summary>
public class ResourceWrapper<T>
{
    public T Data { get; set; } = default!;
    public List<LinkDto> Links { get; set; } = new();
}
