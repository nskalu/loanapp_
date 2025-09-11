using loanapp.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace loanapp.Shared.Helpers
{
    public static class PagingHelper
    {
        public static Task<TResult> GetSkipAndTake<T, TResult>(this IQueryable<T> queryable, IPagedRequest request, int maxPageLength = 50)
            where TResult : PagedResult<T>, new()
        {
            if (request.Page <= 0)
            {
                request.Page = 1;
            }
            if (request.PageLength <= 0)
            {
                request.PageLength = 10;
            }
            if (request.PageLength > maxPageLength)
            {
                request.PageLength = maxPageLength;
            }
            var totalCount = queryable.Count();
            var items = queryable
                .Skip((request.Page - 1) * request.PageLength)
                .Take(request.PageLength)
                .ToList();
            var result = new TResult
            {
                TotalCount = totalCount,
                Page = request.Page,
                PageLength = request.PageLength,
                Items = items
            };
            return Task.FromResult(result);
        }
    }
}
