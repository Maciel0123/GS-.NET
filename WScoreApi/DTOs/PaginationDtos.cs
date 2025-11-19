using System.Collections.Generic;
using System.Linq;
using WScoreDomain.Common;

namespace WScoreApi.DTOs
{
    public record PagedRequest(int Page = 1, int PageSize = 10);

    public class PagedResponse<T>
    {
        public IEnumerable<T> Items { get; init; } = Enumerable.Empty<T>();
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages { get; init; }
        public List<LinkDto> Links { get; init; } = new();
    }
}
