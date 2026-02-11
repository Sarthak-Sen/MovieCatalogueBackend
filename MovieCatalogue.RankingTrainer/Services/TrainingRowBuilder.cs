using MovieCatalogue.RankingTrainer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieCatalogue.RankingTrainer.Services
{
    public class TrainingRowBuilder
    {
        public List<MovieRankingTrainingRow> Build(List<MovieCsvRow> movies, Dictionary<int, MovieAggregates> aggregates)
        {
            var rows = new List<MovieRankingTrainingRow>();

            foreach (var movie in movies)
            {
                if (!aggregates.TryGetValue(movie.MovieId, out var agg))
                    continue;

                int year = ExtractYear(movie.Title);

                float voteLog = (float)Math.Log(agg.VoteCount + 1);

                float recency = 1f / Math.Max(1, DateTime.Now.Year - year);

                float label = agg.AvgRating * voteLog;

                rows.Add(new MovieRankingTrainingRow
                {
                    AvgRating = agg.AvgRating,
                    VoteCountLog = voteLog,
                    RecencyScore = recency,
                    //GenreMatchScore = 0f,
                    Label = label
                });
            }

            return rows;

        }


        private int ExtractYear(string title)
        {
            // Match a year like (1995) ONLY if it appears at the end of the title
            var match = Regex.Match(title, @"\((\d{4})\)$");

            if (match.Success)
            {
                if (int.TryParse(match.Groups[1].Value, out int year))
                    return year;
            }

            // Fallback if no valid year found
            return 2000;
        }
    }
}
