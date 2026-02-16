using MovieCatalogue.Api.Data;
using MovieCatalogue.Api.Models;
using MovieCatalogue.DataImporter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieCatalogue.DataImporter.Services
{
    public class MovieCatalogueImporter
    {
        private readonly MovieCatalogueDbContext _db;

        public MovieCatalogueImporter(MovieCatalogueDbContext db)
        {
            _db = db;
        }

        public void Import(
            List<MovieCsvRow> movies,
            Dictionary<int, (float AvgRating, int VoteCount)> aggregates)
        {
            if (_db.Movies.Any())
            {
                Console.WriteLine("Movies already exist. Import skipped.");
                return;
            }

            var entities = new List<MovieDTO>();

            foreach (var movie in movies)
            {
                if (!aggregates.TryGetValue(movie.MovieId, out var agg))
                    continue;

                entities.Add(new MovieDTO
                {
                    Title = movie.Title,
                    Genres = movie.Genres,
                    ReleaseYear = ExtractYear(movie.Title),
                    AvgRating = agg.AvgRating,
                    VoteCount = agg.VoteCount,
                    PosterUrl = "https://via.placeholder.com/200x300?text=Movie"
                });
            }

            _db.Movies.AddRange(entities);
            _db.SaveChanges();

            Console.WriteLine($"Imported {entities.Count} movies.");
        }

        private int ExtractYear(string title)
        {
            var match = Regex.Match(title, @"\((\d{4})\)$");
            return match.Success ? int.Parse(match.Groups[1].Value) : 2000;
        }
    }
}
