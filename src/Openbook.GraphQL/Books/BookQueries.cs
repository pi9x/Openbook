using Microsoft.EntityFrameworkCore;
using Openbook.Domain;
using Openbook.Domain.Aggregates.Books;

namespace Openbook.GraphQL.Books;

[ExtendObjectType("Query")]
public class BookQueries
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    [UseProjection]
    public IQueryable<Book> GetBooks(ApplicationDbContext db)
    {
        return db.Books
            .Include(b => b.Author)
            .Include(b => b.Reviews)
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable();
    }

    public async Task<Book?> GetBookById(IDbContextFactory<ApplicationDbContext> factory, BookId id)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Books
            .Include(b => b.Author)
            .Include(b => b.Reviews)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(b => b.BookId == id);
    }
}
