using System;
using System.Linq;

namespace MrPigeonStudios.Core.Utility.Pagination {

    public static class PagedResultExtension {

        public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> query, int currentPage, int pageSize) {
            var itemCount = query.Count();
            var skip = (currentPage - 1) * pageSize;
            var totalPageCount = (int)Math.Ceiling((double)itemCount / pageSize);
            var collection = query.Skip(skip).Take(pageSize).ToList();

            return new PagedResult<T>(collection, currentPage, pageSize, totalPageCount);
        }
    }
}