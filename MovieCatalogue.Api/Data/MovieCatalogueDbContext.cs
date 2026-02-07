using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Api.Models;

namespace MovieCatalogue.Api.Data
{
    public class MovieCatalogueDbContext : DbContext
    {
        public MovieCatalogueDbContext(DbContextOptions<MovieCatalogueDbContext> options) : base(options)
        {
        }
        public DbSet<MovieDTO> Movies { get; set; }
    }
}
