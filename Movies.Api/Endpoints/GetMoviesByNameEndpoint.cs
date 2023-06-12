using Movies.Api.Requests;
using Movies.Api.Responses;
using Movies.Api.Services;

namespace Movies.Api.Endpoints;

public sealed class GetMoviesByNameEndpoint : Endpoint<GetMoviesByNameRequest, GetMoviesByNameResponse>
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
        var response = new GetMoviesByNameResponse
        {
            Movies = _movieRepository.GetMoviesByName(req.Name)
        };
        await SendAsync(response, cancellation: ct);
    }
}