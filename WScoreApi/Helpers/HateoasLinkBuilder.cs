using Microsoft.AspNetCore.Http;
using WScoreDomain.Common;

namespace WScoreApi.Helpers
{
    public static class HateoasLinkBuilder
    {
        public static List<LinkDto> ResourceLinks(HttpRequest req, string route, string id)
        {
            var baseUrl = $"{req.Scheme}://{req.Host}/api/v1/{route}/{id}";

            return new List<LinkDto>
            {
                new("self", baseUrl, "GET"),
                new("update", baseUrl, "PUT"),
                new("delete", baseUrl, "DELETE")
            };
        }

        public static LinkDto CreateLink(HttpRequest req, string route)
        {
            return new LinkDto(
                "create",
                $"{req.Scheme}://{req.Host}/api/v1/{route}",
                "POST"
            );
        }

        public static IEnumerable<LinkDto> BuildPaginatedLinks(
            HttpRequest req,
            int page,
            int pageSize,
            int totalPages,
            string route)
        {
            string Base(int p) =>
                $"{req.Scheme}://{req.Host}/api/v1/{route}?page={p}&pageSize={pageSize}";

            var links = new List<LinkDto>
            {
                new("self", Base(page), "GET"),
                new("first", Base(1), "GET"),
                new("last", Base(totalPages > 0 ? totalPages : 1), "GET"),
                CreateLink(req, route)
            };

            if (page > 1)
                links.Add(new("prev", Base(page - 1), "GET"));

            if (page < totalPages)
                links.Add(new("next", Base(page + 1), "GET"));

            return links;
        }
    }
}
