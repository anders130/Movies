﻿using Movies.Api.Models;
using OneOf;
using OneOf.Types;

namespace Movies.Api.Services;

public interface IMovieRepository
{
    List<Movie> GetMovies();
    OneOf<int, AlreadyExists> CreateMovie(Movie movie);
}