using System.Collections.Generic;
using X.PagedList;

namespace Playground.Repositories
{
    public static class PaginationExtensions
    {
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> list, int skip, int limit, int totalCount)
        {
            var pageNumber = (skip / limit) + 1;
            return new StaticPagedList<T>(list, pageNumber, limit, totalCount);
        }
    }
}
