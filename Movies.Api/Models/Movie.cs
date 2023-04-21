using System.ComponentModel.DataAnnotations;

namespace Movies.Api.Models;

public sealed class Movie
{
    public int Id { get; set; }

    [Required, MinLength(2)]
    public string? Name { get; set; }
}