using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using MovieCatalogue.Api.Data;
using MovieCatalogue.DataImporter.Services;

Console.WriteLine("=== MovieLens Importer ===");

string solutionRoot = Directory.GetParent(AppContext.BaseDirectory)!
    .Parent!.Parent!.Parent!.Parent!.FullName;

string dbPath = Path.Combine(solutionRoot, "AppData", "movies.db");

Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

string conn = $"Data Source={dbPath}";

Console.WriteLine("=== CONNECTION STRING ===");
Console.WriteLine(conn);
Console.WriteLine("=== FULL PATH ===");
Console.WriteLine(Path.GetFullPath(conn.Replace("Data Source=", "")));

var dbOptions = new DbContextOptionsBuilder<MovieCatalogueDbContext>()
    .UseSqlite(conn)
    .Options;

using var db = new MovieCatalogueDbContext(dbOptions);
db.Database.EnsureCreated();

var mlContext = new MLContext();

string moviesPath = Path.Combine("Data", "movies.csv");
string ratingsPath = Path.Combine("Data", "ratings.csv");

var loader = new MovieLensLoader(mlContext);
var aggregator = new MovieAggregator();
var importer = new MovieCatalogueImporter(db);

var movies = loader.LoadMovies(moviesPath);
var ratings = loader.LoadRatings(ratingsPath);

var aggregates = aggregator.Compute(ratings);

importer.Import(movies, aggregates);

Console.WriteLine("=== Import Complete ===");