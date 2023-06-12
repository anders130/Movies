using Movies.Api.Data;
using Movies.Api.Models;

namespace Movies.Api.Services;

public sealed class MockMovieRepository : MovieRepository
{
    public MockMovieRepository(MovieDbContext context) : base(context)
    {
        context.Movies.AddRange(new Movie
        {
            Name = "Star Trek - Der Film"
        }, new Movie
        {
            Name = "Star Trek II- Der Zorn des Khan"
        }, new Movie
        {
            Name = "Star Trek III - Auf der Suche nach Mr. Spock"
        }, new Movie
        {
            Name = "Star Trek IV - Zurück in die Vergangenheit"
        }, new Movie
        {
            Name = "Star Trek V - Am Rande des Universums"
        }, new Movie
        {
            Name = "Star Trek VI - Das Unentdeckte Land"
        }, new Movie
        {
            Name = "Star Trek - Treffen der Generationen"
        }, new Movie
        {
            Name = "Star Trek - Der erste Kontakt"
        }, new Movie
        {
            Name = "Star Trek - Der Aufstand"
        }, new Movie
        {
            Name = "Star Trek: Nemesis"
        });
        context.SaveChanges();
    }
}
