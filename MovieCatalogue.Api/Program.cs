using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Api.Data;
using MovieCatalogue.Api.Models;
using MovieCatalogue.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string solutionRoot = Directory.GetParent(AppContext.BaseDirectory)!
    .Parent!.Parent!.Parent!.Parent!.FullName;

string dbPath = Path.Combine(solutionRoot, "AppData", "movies.db");

Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

string conn = $"Data Source={dbPath}";

builder.Services.AddDbContext<MovieCatalogueDbContext>(opt =>
    opt.UseSqlite(conn)
);

builder.Services.AddScoped<IMovieSearchService, MovieSearchService>();
builder.Services.AddSingleton<IMovieRankingService, MovieRankingService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MovieCatalogueDbContext>();

    db.Database.EnsureCreated();
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
