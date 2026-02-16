using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using MovieCatalogue.Api.Data;
using MovieCatalogue.RankingTrainer.Services;

Console.WriteLine("=== Ranking Trainer (DB Model) ===");

string solutionRoot = Directory.GetParent(AppContext.BaseDirectory)!
    .Parent!.Parent!.Parent!.Parent!.FullName;

string dbPath = Path.Combine(solutionRoot, "AppData", "movies.db");

Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

string conn = $"Data Source={dbPath}";

// Setup DbContext
var dbOptions = new DbContextOptionsBuilder<MovieCatalogueDbContext>()
    .UseSqlite(conn)
    .Options;

using var db = new MovieCatalogueDbContext(dbOptions);

// Ensure DB exists
db.Database.EnsureCreated();

if (!db.Movies.Any())
{
    Console.WriteLine("No movies found in DB. Run DataImporter first.");
    return;
}

// Load training rows from DB
var loader = new MovieCatalogueDataLoader(db);
var trainingRows = await loader.LoadTrainingRowsAsync();

Console.WriteLine($"Loaded {trainingRows.Count} training rows.");

// Train model
var mlContext = new MLContext();

var trainer = new RankingModelTrainer(mlContext);

var result = trainer.Train(trainingRows);

// Save model
trainer.SaveModel(result.Model, result.Schema);

Console.WriteLine("=== Training Complete ===");