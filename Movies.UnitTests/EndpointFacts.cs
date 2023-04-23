using System.Net;
using FastEndpoints;
using Movies.Api.Data;
using Movies.Api.Models;
using Movies.Api.Services;
using Movies.Api.Endpoints;
using Movies.Api.Requests;

namespace Movies.UnitTests;

public sealed class EndpointFacts
{
    public sealed class GetMoviesEndpointFacts
    {
        private static (MovieDbContext, GetMoviesEndpoint) Init()
        {
            var context = MovieDbContextUtils.GetUniqueMemoryMovieDbContext();
            var movieRepository = new MovieRepository(context);
            var endpoint = Factory.Create<GetMoviesEndpoint>(movieRepository);
            return (context, endpoint);
        }

        [Fact]
        public async void ReturnsExpectedCount()
        {
            // Arrange
            var (context, endpoint) = Init();
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
            await context.SaveChangesAsync();

            // Act
            await endpoint.HandleAsync(default);
            var response = endpoint.Response;

            // Assert
            response.Movies.Count.Should().Be(2);
        }

        [Fact]
        public async void ReturnsExpectedValues()
        {
            // Arrange
            var (context, endpoint) = Init();
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
            await context.SaveChangesAsync();

            // Act
            await endpoint.HandleAsync(default);
            var response = endpoint.Response;

            // Assert
            response.Movies.Should().BeEquivalentTo(movieList);
        }
        [Fact]
        public async void GetEmptyMovieList()
        {
            // Arrange
            var (_, endpoint) = Init();

            // Act
            await endpoint.HandleAsync(default);
            var response = endpoint.Response;

            // Assert
            response.Should().NotBeNull();
            response.Movies.Should().BeEmpty();
        }
    }
    public sealed class CreateMovieEndpointFacts
    {
        private static (MovieDbContext, CreateMovieEndpoint) Init()
        {
            var context = MovieDbContextUtils.GetUniqueMemoryMovieDbContext();
            var movieRepository = new MovieRepository(context);
            var endpoint = Factory.Create<CreateMovieEndpoint>(movieRepository);
            return (context, endpoint);
        }

        [Fact]
        public async void ReturnedIdWasCreated()
        {
            // Arrange
            var (context, endpoint) = Init();
            var req = new CreateMovieRequest
            {
                Name = "Foo"
            };

            // Act
            await endpoint.HandleAsync(req, default);
            var response = endpoint.Response;
            var movie = await context.Movies.FindAsync(response.Id);

            // Assert
            movie.Should().NotBeNull();
        }
    }
    public sealed class GetMovieByIdEndpointFacts
    {
        private static (MovieDbContext, GetMovieByIdEndpoint) Init()
        {
            var context = MovieDbContextUtils.GetUniqueMemoryMovieDbContext();
            var movieRepository = new MovieRepository(context);
            var endpoint = Factory.Create<GetMovieByIdEndpoint>(movieRepository);
            return (context, endpoint);
        }

        [Fact]
        public async void GetRightMovie()
        {
            // Arrange
            var (context, endpoint) = Init();
            var movie = new Movie
            {
                Name = "Foo"
            };
            context.Movies.Add(movie);
            await context.SaveChangesAsync();
            var req = new GetMovieByIdRequest()
            {
                Id = 1
            };

            // Act
            await endpoint.HandleAsync(req, default);
            var response = endpoint.Response;

            // Assert
            response.Should().BeEquivalentTo(movie);
        }
        [Fact]
        public async void GetNotFound()
        {
            // Arrange
            var (context, endpoint) = Init();
            var req = new GetMovieByIdRequest()
            {
                Id = 1
            };

            // Act
            await endpoint.HandleAsync(req, default);
            var statusCode = endpoint.HttpContext.Response.StatusCode;

            // Assert
            statusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}