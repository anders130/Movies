using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Movies.Api.Data;
using Movies.Api.Models;
using Movies.Api.Services;
using Movies.Api.Utils;
using Movies.Api.Endpoints;

namespace Movies.UnitTests;

public class EndpointFacts
{
    public class GetMoviesEndpointFacts : IDisposable
    {
        private readonly IMovieRepository _movieRepository;
        private readonly MovieDbContext _context;
        public GetMoviesEndpointFacts()
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
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async void ReturnsExpectedCount()
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
            await _context.SaveChangesAsync();
            var ep = Factory.Create<GetMoviesEndpoint>(_movieRepository);

            // Act
            await ep.HandleAsync(default);
            var response = ep.Response;

            // Assert
            response.Movies.Count.Should().Be(2);
        }

        [Fact]
        public async void ReturnsExpectedValues()
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
            await _context.SaveChangesAsync();
            var ep = Factory.Create<GetMoviesEndpoint>(_movieRepository);

            // Act
            await ep.HandleAsync(default);
            var response = ep.Response;

            // Assert
            response.Movies.Should().BeEquivalentTo(movieList);
        }
        [Fact]
        public async void GetEmptyMovieList()
        {
            // Arrange
            var ep = Factory.Create<GetMoviesEndpoint>(_movieRepository);

            // Act
            await ep.HandleAsync(default);
            var response = ep.Response;

            // Assert
            response.Should().NotBeNull();
            response.Movies.Should().BeEmpty();
        }
    }
}