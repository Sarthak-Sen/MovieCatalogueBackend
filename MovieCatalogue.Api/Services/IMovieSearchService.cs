using MovieCatalogue.Api.Models;

namespace MovieCatalogue.Api.Services
{
    public interface IMovieSearchService
    {
        Task<PagedResult<MovieDTO>> SearchAsync(MovieSearchRequest request);
    }
}
