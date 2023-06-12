using Movies.Api.Models;
using Movies.Api.Requests;
using Movies.Api.Responses;

namespace Movies.Api.Mappers;

public sealed class GetMovieByIdMapper : Mapper<GetMovieByIdRequest, GetMovieByIdResponse, Movie>
{
    public override GetMovieByIdResponse FromEntity(Movie e) => new()
    {
        Id = e.Id,
        Name = e.Name
    };
}