using FastEndpoints;
using Movies.Api.Data;
using Movies.Api.Endpoints;
using Movies.Api.Models;
using Movies.Api.Requests;
using Movies.Api.Services;
using System.Net;
using Movies.Api.Mappers;

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
            endpoint.Map = new GetMovieByIdMapper();
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
    public sealed class GetMoviesByNameEndpointFacts
    {
        private static (MovieDbContext, GetMoviesByNameEndpoint) Init()
        {
            var context = MovieDbContextUtils.GetUniqueMemoryMovieDbContext();
            var movieRepository = new MovieRepository(context);
            var endpoint = Factory.Create<GetMoviesByNameEndpoint>(movieRepository);
            return (context, endpoint);
        }

        [Fact]
        public async void GetEmptyList()
        {
            // Arrange
            var (context, endpoint) = Init();
            context.Movies.AddRange(new Movie { Name = "Foo" }, new Movie { Name = "Bar" });
            await context.SaveChangesAsync();
            var req = new GetMoviesByNameRequest
            {
                Name = "Test"
            };

            // Act
            await endpoint.HandleAsync(req, default);
            var response = endpoint.Response;

            // Assert
            response.Movies.Should().BeEmpty();
        }
        [Fact]
        public async void GetOneMovie()
        {
            // Arrange
            var (context, endpoint) = Init();
            var movie = new Movie
            {
                Name = "Prey"
            };
            context.Movies.AddRange(new Movie { Name = "Foo" }, movie);
            await context.SaveChangesAsync();
            var req = new GetMoviesByNameRequest
            {
                Name = "pREy"
            };

            // Act
            await endpoint.HandleAsync(req, default);
            var response = endpoint.Response;

            // Assert
            response.Movies.Should().NotBeEmpty();
            response.Movies.Should().HaveCount(1);
            response.Movies.First().Name.Should().Be(movie.Name);
        }
        [Fact]
        public async void GetTwoMoviesWithSameName()
        {
            // Arrange
            var (context, endpoint) = Init();
            var movie = new Movie
            {
                Name = "Prey"
            };
            var movie2 = new Movie
            {
                Name = "Prey"
            };
            context.Movies.AddRange(movie, movie2);
            await context.SaveChangesAsync();
            var req = new GetMoviesByNameRequest
            {
                Name = "pREy"
            };

            // Act
            await endpoint.HandleAsync(req, default);
            var response = endpoint.Response;

            // Assert
            response.Movies.Should().NotBeEmpty();
            response.Movies.Should().HaveCount(2);
            response.Movies.Should().Contain(movie);
            response.Movies.Should().Contain(movie2);
        }
        [Fact]
        public async void GetOneMovieOfSimilarNames()
        {
            // Arrange
            var (context, endpoint) = Init();
            var movie = new Movie
            {
                Name = "Prey"
            };
            var movie2 = new Movie
            {
                Name = "Prey2"
            };
            context.Movies.AddRange(movie, movie2);
            await context.SaveChangesAsync();
            var req = new GetMoviesByNameRequest
            {
                Name = "pREy"
            };

            // Act
            await endpoint.HandleAsync(req, default);
            var response = endpoint.Response;

            // Assert
            response.Movies.Should().NotBeEmpty();
            response.Movies.Should().HaveCount(1);
            response.Movies.Should().Contain(movie);
        }
    }
    public sealed class DeleteMovieEndpointFacts
    {
        private static (MovieDbContext, DeleteMovieEndpoint) Init()
        {
            var context = MovieDbContextUtils.GetUniqueMemoryMovieDbContext();
            var movieRepository = new MovieRepository(context);
            var endpoint = Factory.Create<DeleteMovieEndpoint>(movieRepository);
            return (context, endpoint);
        }

        [Fact]
        public async void DeletedMovie()
        {
            // Arrange
            var (context, endpoint) = Init();
            context.Movies.Add(new Movie
            {
                Name = "Foo"
            });
            await context.SaveChangesAsync();
            var req = new DeleteMovieRequest
            {
                Id = 1
            };

            // Act
            await endpoint.HandleAsync(req, default);
            var statusCode = endpoint.HttpContext.Response.StatusCode;

            // Assert
            statusCode.Should().Be(204);
            context.Movies.Should().BeEmpty();
        }
        [Fact]
        public async void MovieNotFound()
        {
            // Arrange
            var (context, endpoint) = Init();
            context.Movies.Add(new Movie
            {
                Name = "Foo"
            });
            await context.SaveChangesAsync();
            var req = new DeleteMovieRequest
            {
                Id = 2
            };

            // Act
            await endpoint.HandleAsync(req, default);
            var statusCode = endpoint.HttpContext.Response.StatusCode;

            // Assert
            statusCode.Should().Be(404);
            context.Movies.Should().HaveCount(1);
        }
    }
}