using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.ML;
using MovieCatalogue.DataImporter.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCatalogue.DataImporter.Services
{
    public class MovieLensLoader
    {
        private readonly MLContext _mlContext;
        public MovieLensLoader(MLContext mlContext)
        {
            _mlContext = mlContext;
        }

        public List<MovieCsvRow> LoadMovies(string path)
        {
            using var reader = new StreamReader(path);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                BadDataFound = null
            };

            using var csv = new CsvReader(reader, config);

            return csv.GetRecords<MovieCsvRow>().ToList();
        }

        public List<RatingCsvRow> LoadRatings(string path)
        {
            using var reader = new StreamReader(path);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                BadDataFound = null
            };

            using var csv = new CsvReader(reader, config);

            return csv.GetRecords<RatingCsvRow>().ToList();

        }
    }
}
