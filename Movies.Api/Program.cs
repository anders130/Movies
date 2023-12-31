global using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Movies.Api.Data;
using Movies.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

// Add services to the container.
builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);
builder.Services.AddSingleton<IMovieRepository, MovieRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwaggerGen();

app.Run();
