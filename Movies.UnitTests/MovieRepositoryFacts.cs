using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Movies.Api.Data;
using Movies.Api.Models;
using Movies.Api.Services;
using Movies.Api.Utils;
using Xunit.Abstractions;

namespace Movies.UnitTests;

public sealed class MovieRepositoryFacts
{
    private static (MovieDbContext, IMovieRepository) Init([CallerMemberName] string name = "")
    {
        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseInMemoryDatabase(databaseName: name + "MovieDatabase")
            .Options;

        var context = new MovieDbContext(options);
        var movieRepository = new MovieRepository(context);
        return (context, movieRepository);
    }
    public sealed class GetMoviesFacts
    {
        [Fact]
        public void ReturnsExpectedCount()
        {
            // Arrange
            var (context, movieRepository) = Init();
            context.Movies.AddRange(
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
            context.SaveChanges();

            // Act
            var movies = movieRepository.GetMovies();

            // Assert
            movies.Count.Should().Be(2);
        }

        [Fact]
        public void ReturnsExpectedValues()
        {
            // Arrange
            var (context, movieRepository) = Init();
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
            context.Movies.AddRange(movieList);
            context.SaveChanges();

            // Act
            var movies = movieRepository.GetMovies();

            // Assert
            movies.Should().BeEquivalentTo(movieList);
        }
        [Fact]
        public void ReturnsEmptyList()
        {
            // Arrange
            var (_, movieRepository) = Init();
            // Act
            var movies = movieRepository.GetMovies();

            // Assert
            movies.Should().BeEmpty();
        }
    }

    public sealed class CreateMovieFacts
    {
        [Fact]
        public void ReturnsRightId()
        {
            // Arrange
            var (context, movieRepository) = Init();
            var movie1 = new Movie
            {
                Name = "Foo"
            };
            context.Movies.Add(movie1);
            context.SaveChanges();
            var movie2 = new Movie
            {
                Name = "Bar"
            };
            // Act
            var response = movieRepository.CreateMovie(movie2);

            // Assert
            response.Value.Should().Be(2);
        }
        [Fact]
        public void ReturnsAlreadyExists()
        {
            // Arrange
            var (context, movieRepository) = Init();
            var movie1 = new Movie
            {
                Id = 1,
                Name = "Foo"
            };
            context.Movies.Add(movie1);
            context.SaveChanges();
            var movie2 = new Movie
            {
                Id = 1,
                Name = "Bar"
            };

            // Act
            var response = movieRepository.CreateMovie(movie2);

            // Assert
            response.Value.Should().BeOfType<AlreadyExists>();
        }
    }
}