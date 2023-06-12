using Movies.Api.Models;

namespace Movies.Api.Responses;

public sealed class GetMoviesByNameResponse
{
    public IEnumerable<Movie> Movies { get; set; } = null!;
}