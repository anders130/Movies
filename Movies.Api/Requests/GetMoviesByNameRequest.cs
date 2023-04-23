using Microsoft.AspNetCore.Mvc;

namespace Movies.Api.Requests;

public sealed class GetMoviesByNameRequest
{
    [FromRoute]
    public string Name { get; set; } = null!;
}