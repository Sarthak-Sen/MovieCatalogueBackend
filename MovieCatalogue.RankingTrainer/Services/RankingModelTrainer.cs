using Microsoft.ML;
using Microsoft.ML.Data;
using MovieCatalogue.RankingTrainer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCatalogue.RankingTrainer.Services
{
    public class RankingModelTrainer
    {
        private readonly MLContext _mlContext;

        public RankingModelTrainer(MLContext mlContext)
        {
            _mlContext = mlContext;
        }

        public (ITransformer Model, DataViewSchema Schema) Train(List<MovieRankingTrainingRow> trainingRows)
        {
            Console.WriteLine("Converting training data to IDataView...");

            var dataView = _mlContext.Data.LoadFromEnumerable(trainingRows);

            // Split into train/test
            var split = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

            Console.WriteLine("Building training pipeline...");

            var pipeline = _mlContext.Transforms.Concatenate(
                "Features",
                nameof(MovieRankingTrainingRow.AvgRating),
                nameof(MovieRankingTrainingRow.VoteCountLog),
                nameof(MovieRankingTrainingRow.RecencyScore)
            )
            .Append(_mlContext.Regression.Trainers.FastTree());

            Console.WriteLine("Training regression model...");

            var model = pipeline.Fit(split.TrainSet);

            Console.WriteLine("Evaluating model...");

            var predictions = model.Transform(split.TestSet);

            var metrics = _mlContext.Regression.Evaluate(
            predictions,
            labelColumnName: "Label",
            scoreColumnName: "Score");

            PrintMetrics(metrics);

            return (model, dataView.Schema);

        }

        public void SaveModel(ITransformer model, DataViewSchema schema)
        {
            string outputPath = Path.Combine("Output", "rankingModel.zip");

            Directory.CreateDirectory("Output");

            _mlContext.Model.Save(model, schema, outputPath);

            Console.WriteLine($"Model saved to: {outputPath}");
        }

        private void PrintMetrics(RegressionMetrics metrics)
        {
            Console.WriteLine("=== Model Metrics ===");
            Console.WriteLine($"R-Squared: {metrics.RSquared:F4}");
            Console.WriteLine($"RMSE:      {metrics.RootMeanSquaredError:F4}");
            Console.WriteLine("=====================");
        }

    }
}
