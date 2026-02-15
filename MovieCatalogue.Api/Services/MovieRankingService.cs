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
            throw new NotImplementedException();
        }

        public float ScoreMovie(MovieDTO movie)
        {
            throw new NotImplementedException();
        }
    }
}
