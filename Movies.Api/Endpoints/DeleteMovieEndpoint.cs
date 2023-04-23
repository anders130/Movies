using Movies.Api.Requests;
using Movies.Api.Services;

namespace Movies.Api.Endpoints;

public sealed class DeleteMovieEndpoint : Endpoint<DeleteMovieRequest>
{
    private readonly IMovieRepository _movieRepository;

    public DeleteMovieEndpoint(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public override void Configure()
    {
        Delete("movies/id/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteMovieRequest req, CancellationToken ct)
    {
        var deleteMovie = _movieRepository.DeleteMovie(req.Id);

        await deleteMovie.Match(_ => SendNoContentAsync(ct), _ => SendNotFoundAsync(ct));
    }
}
