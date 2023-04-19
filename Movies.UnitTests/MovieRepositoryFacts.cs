using Microsoft.EntityFrameworkCore;
using Movies.Api.Data;
using Movies.Api.Models;
using Movies.Api.Services;
using Movies.Api.Utils;

namespace Movies.UnitTests;

public class MovieRepositoryFacts
{
    public class GetMoviesFacts : IDisposable
    {
        private readonly IMovieRepository _movieRepository;
        private readonly MovieDbContext _context;
        public GetMoviesFacts()
        {
            var options = new DbContextOptionsBuilder<MovieDbContext>()
                .UseInMemoryDatabase(databaseName: "MovieDatabase")
                .Options;

            _context = new MovieDbContext(options);
            _movieRepository = new MovieRepository(_context);
        }
        public void Dispose()
        {
            _context.DeleteAll<Movie>();
            _context.SaveChanges();
            _context.Dispose();
        }

        [Fact]
        public void ReturnsExpectedCount()
        {
            // Arrange
            _context.Movies.AddRange(
                new Movie
                {
                    Id = 1,
                    Name = "Foo"
                },
                new Movie
                {
                    Id = 2,
                    Name = "Bar"
                });
            _context.SaveChanges();

            // Act
            var movies = _movieRepository.GetMovies();

            // Assert
            movies.Count.Should().Be(2);
        }

        [Fact]
        public void ReturnsExpectedValues()
        {
            // Arrange
            var movieList = new List<Movie>
            {
                new()
                {
                    Id = 1,
                    Name = "Foo"
                },
                new()
                {
                    Id = 2,
                    Name = "Bar"
                }
            };
            _context.Movies.AddRange(movieList);
            _context.SaveChanges();

            // Act
            var movies = _movieRepository.GetMovies();

            // Assert
            movies.Should().BeEquivalentTo(movieList);
        }
    }
}