namespace MovieCatalogue.Api.Models
{
    public class MovieRankingFeatures
    {
        public float AvgRating { get; set; }
        public float VoteCountLog { get; set; }
        public float RecencyScore { get; set; }
        public float GenreMatchScore { get; set; }
    }
}
