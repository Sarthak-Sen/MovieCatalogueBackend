using Microsoft.ML;
using MovieCatalogue.RankingTrainer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCatalogue.RankingTrainer.Services
{
    public class MovieLensDataLoader
    {
        private readonly MLContext _mlContext;
        public MovieLensDataLoader(MLContext mlContext)
        {
            _mlContext = mlContext;
        }

        public List<MovieCsvRow> LoadMovies(string path)
        {
            var dataView = _mlContext.Data
                .LoadFromTextFile<MovieCsvRow>(path, hasHeader: true, separatorChar: ',');

            return _mlContext.Data
                .CreateEnumerable<MovieCsvRow>(dataView, reuseRowObject: false)
                .ToList();
        }

        public List<RatingCsvRow> LoadRatings(string path)
        {
            var dataView = _mlContext.Data
                .LoadFromTextFile<RatingCsvRow>(path, hasHeader: true, separatorChar: ',');

            return _mlContext.Data
                .CreateEnumerable<RatingCsvRow>(dataView, reuseRowObject: false)
                .ToList();

        }

        public Dictionary<int, MovieAggregates> ComputeAggregates(List<RatingCsvRow> ratings)
        {
            return ratings.GroupBy(r => r.MovieId).Select(g => new MovieAggregates
            {
                MovieId = g.Key,
                AvgRating = g.Average(x => x.Rating),
                VoteCount = g.Count()
            }).ToDictionary(x => x.MovieId);
        }
    }
}
