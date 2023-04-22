using Movies.Api.Data;
using Movies.Api.Models;
using OneOf;
using OneOf.Types;

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

    public OneOf<int, AlreadyExists> CreateMovie(Movie movie)
    {
        if (movie.Id != 0 && _context.Movies.Find(movie.Id) is not null)
            return new AlreadyExists();

        _context.Movies.Add(movie);
        _context.SaveChanges();
        return movie.Id;
    }
}
