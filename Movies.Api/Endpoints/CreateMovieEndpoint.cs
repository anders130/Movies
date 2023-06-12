using Movies.Api.Models;
using Movies.Api.Requests;
using Movies.Api.Responses;
using Movies.Api.Services;
using System.Net;

namespace Movies.Api.Endpoints;

public sealed class CreateMovieEndpoint : Endpoint<CreateMovieRequest, CreateMovieResponse>
{
    private readonly IMovieRepository _movieRepository;

    public CreateMovieEndpoint(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public override void Configure()
    {
        Post("movies");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateMovieRequest req, CancellationToken ct)
    {
        var createMovie = _movieRepository.CreateMovie(new Movie
        {
            Name = req.Name
        });
        var response = new CreateMovieResponse
        {
            Id = createMovie.Match(id => id, _ => throw new InvalidOperationException())
        };
        await SendAsync(response, (int)HttpStatusCode.Created, ct);
    }
}
