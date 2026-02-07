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

builder.Services.AddDbContext<MovieCatalogueDbContext>(options =>
    options.UseSqlite("Data Source=movies.db"));

builder.Services.AddScoped<IMovieSearchService, MovieSearchService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MovieCatalogueDbContext>();

    db.Database.EnsureCreated();

    if (!db.Movies.Any())
    {
        db.Movies.AddRange(
            new MovieDTO
            {
                Title = "Inception",
                ReleaseYear = 2010,
                Genres = "Action|Sci-Fi",
                AvgRating = 8.8f,
                VoteCount = 2000000,
                PosterUrl = "https://via.placeholder.com/200x300?text=Inception"
            },
            new MovieDTO
            {
                Title = "Interstellar",
                ReleaseYear = 2014,
                Genres = "Drama|Sci-Fi",
                AvgRating = 8.6f,
                VoteCount = 1800000,
                PosterUrl = "https://via.placeholder.com/200x300?text=Interstellar"
            },
            new MovieDTO
            {
                Title = "The Dark Knight",
                ReleaseYear = 2008,
                Genres = "Action|Crime",
                AvgRating = 9.0f,
                VoteCount = 2500000,
                PosterUrl = "https://via.placeholder.com/200x300?text=Dark+Knight"
            }
        );

        db.SaveChanges();
    }
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
