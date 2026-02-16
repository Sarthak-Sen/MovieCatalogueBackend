using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Api.Data;
using MovieCatalogue.Api.Models;
using MovieCatalogue.Api.Ranking;

namespace MovieCatalogue.Api.Services
{
    public class MovieSearchService : IMovieSearchService
    {
        private readonly MovieCatalogueDbContext _context;
        private readonly IMovieRankingService _rankingService;

        public MovieSearchService(MovieCatalogueDbContext context, IMovieRankingService rankingService)
        {
            _context = context;
            _rankingService = rankingService;
        }

        public async Task<PagedResult<MovieDTO>> SearchAsync(MovieSearchRequest request)
        {
            var query = _context.Movies.AsQueryable();

            // Title search
            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var q = request.Query.ToLower();
                query = query.Where(m => m.Title!.ToLower().Contains(q));
            }

            // Year range
            if (request.MinYear.HasValue)
                query = query.Where(m => m.ReleaseYear >= request.MinYear.Value);

            if (request.MaxYear.HasValue)
                query = query.Where(m => m.ReleaseYear <= request.MaxYear.Value);

            // Rating threshold
            if (request.MinRating.HasValue)
                query = query.Where(m => m.AvgRating >= request.MinRating.Value);

            // Vote threshold
            if (request.MinVotes.HasValue)
                query = query.Where(m => m.VoteCount >= request.MinVotes.Value);

            //Genre filtering
            if (request.Genres != null && request.Genres.Any())
            {
                query = query.Where(m => request.Genres.Any(g => m.Genres!.ToLower().Contains(g.ToLower())));
            }

            var candidates = await query.ToListAsync();

            var ranked = candidates
                .Select(movie => new
                {
                    Movie = movie,
                    Score = _rankingService.ComputeFinalScore(movie, request)
                })
                .OrderByDescending(x => x.Score)
                .ToList();

            // Pagination
            int totalCount = ranked.Count;

            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = request.PageSize > 50 ? 50 : request.PageSize;

            var results = ranked.Skip((page - 1) * pageSize).Take(pageSize).Select(x => x.Movie).ToList();

            return new PagedResult<MovieDTO>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Results = results
            };
        }
    }
}
