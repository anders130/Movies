using Microsoft.EntityFrameworkCore;

namespace Movies.Api.Utils;

public static class DbContextExtensions
{
    public static void DeleteAll<T>(this DbContext context) where T : class
    {
        foreach (var p in context.Set<T>())
        {
            context.Entry(p).State = EntityState.Deleted;
        }
    }
}