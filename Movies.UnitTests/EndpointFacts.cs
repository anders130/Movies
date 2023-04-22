﻿using FastEndpoints;
using Movies.Api.Data;
using Movies.Api.Models;
using Movies.Api.Services;
using Movies.Api.Endpoints;

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
}