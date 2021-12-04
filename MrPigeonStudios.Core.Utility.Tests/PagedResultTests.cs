using System;
using System.Collections;
using System.Linq;
using MrPigeonStudios.Core.Utility.Pagination;
using Xunit;

namespace MrPigeonStudios.Core.Utility.Tests {

    public class PagedResultTests {

        [Fact]
        public void PagedResult_Enumerator() {
            var collection = Enumerable.Range(1, 100).AsQueryable();

            var page = collection.ToPagedResult(1, 10);

            var enumerator = ((IEnumerable)page).GetEnumerator();

            int i = 1;

            while (enumerator.MoveNext()) {
                Assert.Equal(i, enumerator.Current);
                i++;
            }
        }

        [Theory]
        [InlineData(10, 10, 91, 100, 10)]
        [InlineData(1, 10, 1, 10, 10)]
        public void PagedResult_GetPage(int currentPage, int pageSize, int expectedStart, int expectedEnd, int totalPageExpected) {
            var collection = Enumerable.Range(1, 100).AsQueryable();

            var page = collection.ToPagedResult(currentPage, pageSize);

            Assert.Equal(currentPage, page.CurrentPage);
            Assert.Equal(expectedStart, page.First());
            Assert.Equal(expectedEnd, page.Last());
            Assert.Equal(pageSize, page.PageSize);
            Assert.Equal(pageSize, page.Count());
            Assert.Equal(totalPageExpected, page.TotalPageCount);
        }

        [Fact]
        public void PagedResult_InvalidCollection() {
            IQueryable<int> collection = null;

            var exceptionType = typeof(ArgumentNullException);

            Assert.Throws(exceptionType, () => new PagedResult<int>(collection, 1, 10));
        }

        [Fact]
        public void PagedResult_InvalidCurrentPage() {
            var collection = Enumerable.Range(1, 100);

            var exceptionType = typeof(ArgumentException);

            Assert.Throws(exceptionType, () => new PagedResult<int>(collection, 0, 10));
        }

        [Fact]
        public void PagedResult_InvalidPageSize() {
            var collection = Enumerable.Range(1, 100);

            var exceptionType = typeof(ArgumentException);

            Assert.Throws(exceptionType, () => new PagedResult<int>(collection, 1, 0));
        }

        [Fact]
        public void PagedResult_InvalidTotalPageCount() {
            var collection = Enumerable.Range(1, 100);

            var exceptionType = typeof(ArgumentException);

            Assert.Throws(exceptionType, () => new PagedResult<int>(collection, 1, 10, -1));
        }

        [Fact]
        public void PagedResult_ValidNullTotalPageCount() {
            var collection = Enumerable.Range(1, 10);

            var pagedResult = new PagedResult<int>(collection, 1, 10, null);

            Assert.Equal(10, pagedResult.Count());
            Assert.Equal(10, pagedResult.PageSize);
            Assert.Equal(1, pagedResult.CurrentPage);
            Assert.Equal(1, pagedResult.Min());
            Assert.Equal(10, pagedResult.Max());
            Assert.Null(pagedResult.TotalPageCount);
        }
    }
}