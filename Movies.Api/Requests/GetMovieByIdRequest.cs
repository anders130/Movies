using Microsoft.AspNetCore.Mvc;

namespace Movies.Api.Requests;

public sealed class GetMovieByIdRequest
{
    [FromRoute]
    public int Id { get; set; }
}