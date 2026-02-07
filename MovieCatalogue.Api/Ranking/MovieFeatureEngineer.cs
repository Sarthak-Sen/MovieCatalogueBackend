using MovieCatalogue.Api.Models;

namespace MovieCatalogue.Api.Ranking
{
    public class MovieFeatureEngineer
    {
        public static MovieRankingFeatures Build(MovieDTO movie, MovieSearchRequest request)
        {
            var features = new MovieRankingFeatures
            {
                AvgRating = movie.AvgRating,
                VoteCountLog = (float)Math.Log(movie.VoteCount + 1), // Log-transform vote count
                RecencyScore = CalculateRecencyScore(movie.ReleaseYear),
                GenreMatchScore = CalculateGenreMatchScore(movie.Genres, request.Genres)
            };
            return features;
        }

        private static float CalculateGenreMatchScore(string movieGenres, List<string>? requestedGenres)
        {
            if (requestedGenres == null || requestedGenres.Count == 0)
                return 0f;

            int matches = requestedGenres.Count(g => 
            movieGenres.Contains(g, StringComparison.OrdinalIgnoreCase));

            return matches / (float)requestedGenres.Count;
        }

        private static float CalculateRecencyScore(int releaseYear)
        {
            var currentYear = DateTime.Now.Year;

            // Prevent divide-by-zero or future year weirdness
            var age = Math.Max(1, currentYear - releaseYear);

            // More recent movies get slightly higher score
            return 1f / age;
        }
    }
}
