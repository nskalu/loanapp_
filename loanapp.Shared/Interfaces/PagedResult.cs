

namespace loanapp.Shared.Interfaces
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }

        public int Page { get; set; }

        public int PageLength { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageLength);

        public IList<T> Items { get; set; } = new List<T>();
    }
}
