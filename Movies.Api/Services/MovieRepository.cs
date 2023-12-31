﻿using Movies.Api.Data;
using Movies.Api.Models;
using OneOf;
using OneOf.Types;

namespace Movies.Api.Services;

public class MovieRepository : IMovieRepository
{
    private readonly MovieDbContext _context;

    public MovieRepository(MovieDbContext context)
    {
        _context = context;
    }

    public List<Movie> GetMovies()
    {
        return _context.Movies.ToList();
    }

    public OneOf<int, AlreadyExists> CreateMovie(Movie movie)
    {
        if (movie.Id != 0 && _context.Movies.Find(movie.Id) is not null)
            return new AlreadyExists();

        _context.Movies.Add(movie);
        _context.SaveChanges();
        return movie.Id;
    }

    public OneOf<Movie, NotFound> GetMovieById(int id)
    {
        var movie = _context.Movies.Find(id);
        if (movie is null)
            return new NotFound();
        return movie;
    }

    public List<Movie> GetMoviesByName(string name)
    {
        return _context.Movies.Where(m => m.Name!.ToLower().Equals(name.ToLower())).ToList();
    }

    public OneOf<Success, NotFound> DeleteMovie(int id)
    {
        var movie = _context.Movies.Find(id);
        if (movie is null)
            return new NotFound();
        _context.Movies.Remove(movie);
        _context.SaveChanges();
        return new Success();
    }
}
