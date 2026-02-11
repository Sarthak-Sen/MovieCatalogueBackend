using Microsoft.ML.Data;

namespace MovieCatalogue.RankingTrainer.Models
{
    public class RatingCsvRow
    {
        [LoadColumn(0)]
        public int UserId { get; set; }

        [LoadColumn(1)]
        public int MovieId { get; set; }

        [LoadColumn(2)]
        public float Rating { get; set; }

        [LoadColumn(3)]
        public long Timestamp { get; set; }
    }
}
