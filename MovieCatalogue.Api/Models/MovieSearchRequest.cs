namespace MovieCatalogue.Api.Models
{
    public class MovieSearchRequest
    {
        public string? Query { get; set; }
        // Example: ["Action", "Drama"]
        public List<string>? Genres { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
        public float? MinRating { get; set; }
        public int? MinVotes { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
