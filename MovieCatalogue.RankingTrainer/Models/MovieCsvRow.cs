using Microsoft.ML.Data;

namespace MovieCatalogue.RankingTrainer.Models
{
    public class MovieCsvRow
    {
        [LoadColumn(0)]
        public int MovieId { get; set; }

        [LoadColumn(1)]
        public string Title { get; set; }

        [LoadColumn(2)]
        public string Genres { get; set; }
    }
}
