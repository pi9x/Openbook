using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR.Management;
using Microsoft.EntityFrameworkCore;
using Openbook.Domain;
using Openbook.Domain.Aggregates.Reviews;
using Openbook.Domain.Aggregates.Books;

namespace Openbook.GraphQL.Reviews;

[ExtendObjectType("Mutation")]
public class ReviewMutations
{
    public async Task<Review?> CreateReview(IDbContextFactory<ApplicationDbContext> factory, BookId bookId, int rating, string? content, [Service] IServiceHubContext hubContext)
    {
        await using var db = await factory.CreateDbContextAsync();
        var book = await db.Books.FindAsync(bookId);
        if (book == null) return null;
        var review = new Review { ReviewId = new ReviewId(Guid.NewGuid()), BookId = book.BookId, Rating = rating, Content = content };
        book.AddReview(review);
        await db.SaveChangesAsync();
        
        // Send real-time update to SignalR clients
        try 
        {
            await hubContext.Clients.All.SendAsync("ReviewCreated", new
            {
                ReviewId = review.ReviewId.Value,
                BookId = review.BookId.Value,
                review.Rating,
                review.Content
            });
        } 
        catch 
        {
            // The notification for the created review is not critical and errors can be ignored.
            // The response should be success even when SignalR is down.
            // Azure Functions may be terminated immediately after the response is sent, so we should not use fire-and-forget by not awaiting the task.
        }
        
        return review;
    }

    public async Task<Review?> UpdateReview(IDbContextFactory<ApplicationDbContext> factory, ReviewId id, int rating, string? content)
    {
        await using var db = await factory.CreateDbContextAsync();
        var review = await db.Reviews.FindAsync(id);
        if (review == null) return null;
        review.Rating = rating;
        review.Content = content;
        await db.SaveChangesAsync();
        return review;
    }

    public async Task<bool> DeleteReview(IDbContextFactory<ApplicationDbContext> factory, ReviewId id)
    {
        await using var db = await factory.CreateDbContextAsync();
        var review = await db.Reviews.FindAsync(id);
        if (review == null) return false;
        db.Reviews.Remove(review);
        await db.SaveChangesAsync();
        return true;
    }
}
