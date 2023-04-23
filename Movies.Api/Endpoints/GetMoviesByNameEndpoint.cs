using Movies.Api.Models;
using Movies.Api.Requests;
using Movies.Api.Services;

namespace Movies.Api.Endpoints;

public sealed class GetMoviesByNameEndpoint : Endpoint<GetMoviesByNameRequest, List<Movie>>
{
    private readonly IMovieRepository _movieRepository;

    public GetMoviesByNameEndpoint(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public override void Configure()
    {
        Get("movies/name/{name}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetMoviesByNameRequest req, CancellationToken ct)
    {
        var movies = _movieRepository.GetMoviesByName(req.Name);
        await SendAsync(movies, cancellation: ct);
    }
}