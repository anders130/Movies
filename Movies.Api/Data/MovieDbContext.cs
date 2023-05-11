using Microsoft.EntityFrameworkCore;
using Movies.Api.Models;

namespace Movies.Api.Data;

public sealed class MovieDbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; } = null!;

    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>()
            .HasKey(m => m.Id);
        modelBuilder.Entity<Movie>()
            .Property(m => m.Id)
            .ValueGeneratedOnAdd();

        base.OnModelCreating(modelBuilder);
    }
}