using MovieCatalogue.Api.Models;

namespace MovieCatalogue.Api.Services
{
    public interface IMovieRankingService
    {
        float ComputeFinalScore(MovieDTO movie, MovieSearchRequest request);
    }
}
