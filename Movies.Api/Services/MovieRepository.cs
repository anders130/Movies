using Movies.Api.Data;
using Movies.Api.Models;

namespace Movies.Api.Services;

public class MovieRepository : IMovieRepository
{
    private readonly MovieDbContext _context;

    public MovieRepository(MovieDbContext context)
    {
        _context = context;
    }

    public List<Movie> GetMovies()
    {
        return _context.Movies.ToList();
    }
}
