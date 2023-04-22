global using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Movies.Api.Data;
using Movies.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();

// Add services to the container.
builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseInMemoryDatabase(databaseName: "MovieDatabase"), ServiceLifetime.Singleton);
builder.Services.AddSingleton<IMovieRepository, MovieRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();
app.UseFastEndpoints();

app.Run();
