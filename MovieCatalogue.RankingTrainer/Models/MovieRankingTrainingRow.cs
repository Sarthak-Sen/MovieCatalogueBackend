using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCatalogue.RankingTrainer.Models
{
    public class MovieRankingTrainingRow
    {
        public float AvgRating { get; set; }

        public float VoteCountLog { get; set; }

        public float RecencyScore { get; set; }

        //public float GenreMatchScore { get; set; }

        public float Label { get; set; }
    }
}
