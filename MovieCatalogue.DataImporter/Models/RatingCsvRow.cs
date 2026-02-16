using CsvHelper.Configuration.Attributes;
using Microsoft.ML.Data;

namespace MovieCatalogue.DataImporter.Models
{
    public class RatingCsvRow
    {
        [Name("userId")]
        public int UserId { get; set; }

        [Name("movieId")]
        public int MovieId { get; set; }

        [Name("rating")]
        public float Rating { get; set; }

        [Name("timestamp")]
        public long Timestamp { get; set; }
    }
}
