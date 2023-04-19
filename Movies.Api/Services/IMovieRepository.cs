using Movies.Api.Models;

namespace Movies.Api.Services;

public interface IMovieRepository
{
    List<Movie> GetMovies();
}