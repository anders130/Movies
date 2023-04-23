using Movies.Api.Models;
using OneOf;
using OneOf.Types;

namespace Movies.Api.Services;

public interface IMovieRepository
{
    List<Movie> GetMovies();
    OneOf<int, AlreadyExists> CreateMovie(Movie movie);
    OneOf<Movie, NotFound> GetMovieById(int id);
    List<Movie> GetMoviesByName(string name);
}