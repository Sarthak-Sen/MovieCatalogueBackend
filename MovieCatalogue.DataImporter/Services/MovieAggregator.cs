using MovieCatalogue.DataImporter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCatalogue.DataImporter.Services
{
    public class MovieAggregator
    {
        public Dictionary<int, (float AvgRating, int VoteCount)> Compute(List<RatingCsvRow> ratings)
        {
            return ratings
                .GroupBy(r => r.MovieId)
                .ToDictionary(
                    g => g.Key,
                    g => ((float)g.Average(x => x.Rating), g.Count())
                );
        }
    }
}
