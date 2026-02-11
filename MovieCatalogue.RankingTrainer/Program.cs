using Microsoft.ML;
using MovieCatalogue.RankingTrainer.Services;

var mlContext = new MLContext(seed: 42);

string moviesPath = Path.Combine("Data", "movies.csv");
string ratingsPath = Path.Combine("Data", "ratings.csv");

Console.WriteLine("Loading MovieLens dataset...");

var loader = new MovieLensDataLoader(mlContext);

var movies = loader.LoadMovies(moviesPath);
var ratings = loader.LoadRatings(ratingsPath);

Console.WriteLine($"Movies: {movies.Count}");
Console.WriteLine($"Ratings: {ratings.Count}");

var aggregates = loader.ComputeAggregates(ratings);

var builder = new TrainingRowBuilder();

var trainingRows = builder.Build(movies, aggregates);

var trainer = new RankingModelTrainer(mlContext);

var result = trainer.Train(trainingRows);

trainer.SaveModel(result.Model, result.Schema);

Console.WriteLine("Model training complete.");

Console.WriteLine($"Training rows: {trainingRows.Count}");
Console.WriteLine("Sample:");

foreach (var row in trainingRows.Take(3))
{
    Console.WriteLine(
        $"Rating={row.AvgRating:F2}, VotesLog={row.VoteCountLog:F2}, Label={row.Label:F2}");
}