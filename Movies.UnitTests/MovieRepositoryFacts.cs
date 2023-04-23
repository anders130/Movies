using Movies.Api.Data;
using Movies.Api.Models;
using Movies.Api.Services;
using OneOf.Types;

namespace Movies.UnitTests;

public sealed class MovieRepositoryFacts
{
    private static (MovieDbContext, IMovieRepository) Init()
    {
        var context = MovieDbContextUtils.GetUniqueMemoryMovieDbContext();
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

    public sealed class GetMovieByIdFacts
    {
        [Fact]
        public void GetRightMovie()
        {
            // Arrange
            var (context, movieRepository) = Init();
            var movie1 = new Movie
            {
                Name = "Foo"
            };
            var movie2 = new Movie
            {
                Name = "Bar"
            };
            context.Movies.AddRange(movie1, movie2);
            context.SaveChanges();

            // Act
            var response = movieRepository.GetMovieById(2);

            // Assert
            response.Value.Should().Be(movie2);
        }
        [Fact]
        public void GetNotFound()
        {
            // Arrange
            var (context, movieRepository) = Init();
            var movie1 = new Movie
            {
                Name = "Foo"
            };
            var movie2 = new Movie
            {
                Name = "Bar"
            };
            context.Movies.AddRange(movie1, movie2);
            context.SaveChanges();

            // Act
            var response = movieRepository.GetMovieById(3);
            
            // Assert
            response.Value.Should().BeOfType<NotFound>();
        }
    }

    public sealed class GetMoviesByNameFacts
    {
        [Fact]
        public void GetEmptyList()
        {
            // Arrange
            var (context, movieRepository) = Init();
            context.Movies.AddRange(new Movie { Name = "Foo" }, new Movie{ Name = "Bar" });
            context.SaveChanges();

            // Act
            var movies = movieRepository.GetMoviesByName("pREy");

            // Assert
            movies.Should().BeEmpty();
        }
        [Fact]
        public void GetOneMovie()
        {
            // Arrange
            var (context, movieRepository) = Init();
            var movie = new Movie
            {
                Name = "Prey"
            };
            context.Movies.AddRange(new Movie {  Name = "Foo" }, movie);
            context.SaveChanges();

            // Act
            var movies = movieRepository.GetMoviesByName("pREy");

            // Assert
            movies.Should().NotBeEmpty();
            movies.Should().HaveCount(1);
            movies.First().Name.Should().Be(movie.Name);
        }
        [Fact]
        public void GetTwoMoviesWithSameName()
        {
            // Arrange
            var (context, movieRepository) = Init();
            var movie = new Movie
            {
                Name = "Prey"
            }; 
            var movie2 = new Movie
            {
                Name = "Prey"
            };
            context.Movies.AddRange(movie, movie2);
            context.SaveChanges();

            // Act
            var movies = movieRepository.GetMoviesByName("pREy");

            // Assert
            movies.Should().NotBeEmpty();
            movies.Should().HaveCount(2);
            movies.Should().Contain(movie);
            movies.Should().Contain(movie2);
        }
        [Fact]
        public void GetOneMovieOfSimilarNames()
        {
            // Arrange
            var (context, movieRepository) = Init();
            var movie = new Movie
            {
                Name = "Prey"
            };
            var movie2 = new Movie
            {
                Name = "Prey2"
            };
            context.Movies.AddRange(movie, movie2);
            context.SaveChanges();

            // Act
            var movies = movieRepository.GetMoviesByName("pREy");

            // Assert
            movies.Should().NotBeEmpty();
            movies.Should().HaveCount(1);
            movies.Should().Contain(movie);
        }
    }

    public sealed class DeleteMovieFacts
    {
        [Fact]
        public void ReturnsSuccess()
        {
            // Arrange
            var (context, movieRepository) = Init();
            context.Movies.Add(new Movie
            {
                Name = "Foo"
            });
            context.SaveChanges();

            // Act
            var response = movieRepository.DeleteMovie(1);

            // Assert
            response.Value.Should().BeOfType<Success>();
            context.Movies.Should().BeEmpty();
        }
        [Fact]
        public void ReturnsNotFound()
        {
            // Arrange
            var (context, movieRepository) = Init();
            context.Movies.Add(new Movie
            {
                Name = "Foo"
            });
            context.SaveChanges();

            // Act
            var response = movieRepository.DeleteMovie(2);

            // Assert
            response.Value.Should().BeOfType<NotFound>();
            context.Movies.Should().HaveCount(1);
        }
    }
}