namespace Movies.Api.Responses;

public sealed class GetMovieByIdResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
}