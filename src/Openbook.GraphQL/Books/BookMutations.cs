using Microsoft.EntityFrameworkCore;
using Openbook.Domain;
using Openbook.Domain.Aggregates.Books;
using Openbook.Domain.Aggregates.Authors;

namespace Openbook.GraphQL.Books;

[ExtendObjectType("Mutation")]
public class BookMutations
{
    public async Task<Book?> CreateBook(IDbContextFactory<ApplicationDbContext> factory, string title, AuthorId authorId)
    {
        await using var db = await factory.CreateDbContextAsync();
        var author = await db.Authors.FindAsync(authorId);
        if (author == null) return null;
        var book = new Book { BookId = new BookId(Guid.NewGuid()), Title = title, Author = author };
        db.Books.Add(book);
        await db.SaveChangesAsync();
        return book;
    }

    public async Task<Book?> UpdateBook(IDbContextFactory<ApplicationDbContext> factory, BookId id, string title)
    {
        await using var db = await factory.CreateDbContextAsync();
        var book = await db.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.BookId == id);
        if (book == null) return null;
        book.Title = title;
        await db.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteBook(IDbContextFactory<ApplicationDbContext> factory, BookId id)
    {
        await using var db = await factory.CreateDbContextAsync();
        var book = await db.Books.FindAsync(id);
        if (book == null) return false;
        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return true;
    }
}
