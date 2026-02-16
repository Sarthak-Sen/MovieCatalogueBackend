using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Api.Data;
using MovieCatalogue.RankingTrainer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCatalogue.RankingTrainer.Services
{
    public class MovieCatalogueDataLoader
    {
        private readonly MovieCatalogueDbContext _db;
        public MovieCatalogueDataLoader(MovieCatalogueDbContext db)
        {
            _db = db;
        }

        public async Task<List<MovieRankingTrainingRow>> LoadTrainingRowsAsync()
        {
            var movies = await _db.Movies.ToListAsync();

            return movies.Select(m => new MovieRankingTrainingRow
            {
                AvgRating = (float)m.AvgRating,
                VoteCountLog = (float)Math.Log(m.VoteCount + 1),
                RecencyScore = 1f / Math.Max(1, DateTime.Now.Year - m.ReleaseYear),

                // Proxy label
                Label = (float)m.AvgRating * (float)Math.Log(m.VoteCount + 1)
            }).ToList();
        }
    }
}
