using Movies.Api.Responses;
using Movies.Api.Services;

namespace Movies.Api.Endpoints;

public sealed class GetMoviesEndpoint : EndpointWithoutRequest<GetMoviesResponse>
{
    private readonly IMovieRepository _movieRepository;

    public GetMoviesEndpoint(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public override void Configure()
    {
        Get("movies");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = new GetMoviesResponse
        {
            Movies = _movieRepository.GetMovies()
        };
        await SendAsync(response, cancellation: ct);
    }
}