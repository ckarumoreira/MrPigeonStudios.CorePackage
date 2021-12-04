using System;
using System.Collections;
using System.Collections.Generic;

namespace MrPigeonStudios.Core.Utility.Pagination {

    public sealed class PagedResult<T> : IEnumerable<T> {
        private readonly IEnumerable<T> internalCollection;

        public PagedResult(
            IEnumerable<T> collection,
            int currentPage,
            int pageSize,
            int? totalPageCount = null
        ) {
            internalCollection = collection ?? throw new ArgumentNullException(nameof(collection));

            if (currentPage <= 0)
                throw new ArgumentException($"Argument '{nameof(currentPage)}' can't be below 1.");
            CurrentPage = currentPage;

            if (pageSize <= 0)
                throw new ArgumentException($"Argument '{nameof(pageSize)}' can't be below 1.");
            PageSize = pageSize;

            if (totalPageCount != null && totalPageCount < 0)
                throw new ArgumentException($"Argument '{nameof(totalPageCount)}' can't be below 0.");
            TotalPageCount = totalPageCount;
        }

        public int CurrentPage { get; }
        public int PageSize { get; }
        public int? TotalPageCount { get; }

        public IEnumerator<T> GetEnumerator() {
            return internalCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}