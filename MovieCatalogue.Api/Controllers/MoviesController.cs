using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Api.Data;
using MovieCatalogue.Api.Models;
using MovieCatalogue.Api.Services;

namespace MovieCatalogue.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieCatalogueDbContext _context;
        private readonly IMovieSearchService _searchService;

        public MoviesController(MovieCatalogueDbContext context, IMovieSearchService searchService)
        {
            _context = context;
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _context.Movies
                .OrderBy(m => m.Id)
                .ToListAsync();

            return Ok(movies);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] MovieSearchRequest request)
        {
            var results = await _searchService.SearchAsync(request);
            return Ok(results);
        }
    }
}
