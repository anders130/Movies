using Movies.Api.Models;
using Movies.Api.Requests;
using Movies.Api.Services;

namespace Movies.Api.Endpoints;

public sealed class GetMovieByIdEndpoint : Endpoint<GetMovieByIdRequest, Movie>
{
    private readonly IMovieRepository _movieRepository;

    public GetMovieByIdEndpoint(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public override void Configure()
    {
        Get("movies/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetMovieByIdRequest req, CancellationToken ct)
    {
        var getMovie = _movieRepository.GetMovieById(req.Id);
        await getMovie.Match(
            movie => SendAsync(movie, cancellation: ct),
            _ => SendNotFoundAsync(ct));
    }
}
