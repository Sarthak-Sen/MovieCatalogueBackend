using CsvHelper.Configuration.Attributes;
using Microsoft.ML.Data;

namespace MovieCatalogue.DataImporter.Models
{
    public class MovieCsvRow
    {
        [Name("movieId")]
        public int MovieId { get; set; }

        [Name("title")]
        public string Title { get; set; }

        [Name("genres")]
        public string Genres { get; set; }
    }
}
