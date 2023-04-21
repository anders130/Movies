using Movies.Api.Models;

namespace Movies.Api.Responses;

public sealed class GetMoviesResponse
{
    public List<Movie> Movies { get; set; } = null!;
}