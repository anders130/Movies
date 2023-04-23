using Microsoft.AspNetCore.Mvc;

namespace Movies.Api.Requests;

public sealed class DeleteMovieRequest
{
    [FromRoute]
    public int Id { get; set; }
}
