using Microsoft.EntityFrameworkCore;
using Openbook.Domain;
using Openbook.Domain.Aggregates.Authors;

namespace Openbook.GraphQL.Authors;

[ExtendObjectType("Mutation")]
public class AuthorMutations
{
    public async Task<Author> CreateAuthor(IDbContextFactory<ApplicationDbContext> factory, string name, string? bio)
    {
        await using var db = await factory.CreateDbContextAsync();
        var author = new Author { AuthorId = new AuthorId(Guid.NewGuid()), Name = name, Bio = bio };
        db.Authors.Add(author);
        await db.SaveChangesAsync();
        return author;
    }

    public async Task<Author?> UpdateAuthor(IDbContextFactory<ApplicationDbContext> factory, AuthorId id, string name, string? bio)
    {
        await using var db = await factory.CreateDbContextAsync();
        var author = await db.Authors.FindAsync(id);
        if (author == null) return null;
        author.Name = name;
        author.Bio = bio;
        await db.SaveChangesAsync();
        return author;
    }

    public async Task<bool> DeleteAuthor(IDbContextFactory<ApplicationDbContext> factory, AuthorId id)
    {
        await using var db = await factory.CreateDbContextAsync();
        var author = await db.Authors.FindAsync(id);
        if (author == null) return false;
        db.Authors.Remove(author);
        await db.SaveChangesAsync();
        return true;
    }
}
