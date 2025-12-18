using Microsoft.EntityFrameworkCore;
using Openbook.Domain;
using Openbook.Domain.Aggregates.Authors;

namespace Openbook.GraphQL.Authors;

[ExtendObjectType("Query")]
public class AuthorQueries
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    [UseProjection]
    public IQueryable<Author> GetAuthors(ApplicationDbContext db)
    {
        return db.Authors.AsQueryable();
    }

    public async Task<Author?> GetAuthorById(IDbContextFactory<ApplicationDbContext> factory, [ID(nameof(Author))] AuthorId authorId)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Authors.FindAsync(authorId);
    }
}
