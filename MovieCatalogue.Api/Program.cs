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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MovieCatalogueDbContext>(options =>
{
    if (connectionString != null && connectionString.Contains("Host="))
    {
        // PostgreSQL (Production)
        options.UseNpgsql(connectionString);
    }
    else
    {
        // SQLite (Local dev fallback)
        options.UseSqlite(connectionString);
    }
});

builder.Services.AddScoped<IMovieSearchService, MovieSearchService>();
builder.Services.AddSingleton<IMovieRankingService, MovieRankingService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MovieCatalogueDbContext>();
    db.Database.Migrate();
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
