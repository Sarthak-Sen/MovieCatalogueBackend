namespace MovieCatalogue.Api.Models
{
    public class PagedResult<T>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<T> Results { get; set; }
    }
}
