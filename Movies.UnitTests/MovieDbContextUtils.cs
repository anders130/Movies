using Microsoft.EntityFrameworkCore;
using Movies.Api.Data;

namespace Movies.UnitTests;

public static class MovieDbContextUtils
{
    public static MovieDbContext GetUniqueMemoryMovieDbContext()
    {
        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseInMemoryDatabase(databaseName: "MovieDatabase" + Guid.NewGuid())
            .Options;

        return new MovieDbContext(options);
    }
}