namespace MovieCatalogue.Api.Models
{
    public class MovieDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public int ReleaseYear { get; set; }

        public string? Genres { get; set; }

        public float AvgRating { get; set; }

        public int VoteCount { get; set; }

        public string? PosterUrl { get; set; }
    }
}
