using Microsoft.ML;
using MovieCatalogue.Api.Models;
using MovieCatalogue.Api.Ranking;

namespace MovieCatalogue.Api.Services
{
    public class MovieRankingService : IMovieRankingService
    {
        private readonly PredictionEngine<MovieModelInput, MovieModelOutput> _engine;

        public MovieRankingService()
        {
            var mlContext = new MLContext();

            var modelPath = Path.Combine("MLModels", "rankingModel.zip");

            var model = mlContext.Model.Load(modelPath, out _);

            _engine = mlContext.Model.CreatePredictionEngine<MovieModelInput, MovieModelOutput>(model);
        }

        public float ComputeFinalScore(MovieDTO movie, MovieSearchRequest request)
        {

            var input = new MovieModelInput
            {
                AvgRating = movie.AvgRating,
                VoteCountLog = (float)Math.Log(movie.VoteCount + 1),
                RecencyScore = 1f / Math.Max(1, DateTime.Now.Year - movie.ReleaseYear)
            };

            float mlScore = _engine.Predict(input).Score;

            float genreBoost = ComputeGenreBoost(movie.Genres, request.Genres);

            return mlScore + genreBoost;
        }

        private float ComputeGenreBoost(string movieGenres, List<string>? requestedGenres)
        {
            if (requestedGenres == null || requestedGenres.Count == 0)
                return 0f;

            int matches = requestedGenres.Count(g =>
                movieGenres.Contains(g, StringComparison.OrdinalIgnoreCase));

            return matches / (float)requestedGenres.Count;
        }
    }
}
