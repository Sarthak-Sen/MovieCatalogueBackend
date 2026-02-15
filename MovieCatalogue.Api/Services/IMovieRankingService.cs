using MovieCatalogue.Api.Models;

namespace MovieCatalogue.Api.Services
{
    public interface IMovieRankingService
    {
        float ScoreMovie(MovieDTO movie);

        float ComputeFinalScore(MovieDTO movie, MovieSearchRequest request);
    }
}
