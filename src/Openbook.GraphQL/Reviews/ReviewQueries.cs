using Microsoft.EntityFrameworkCore;
using Openbook.Domain;
using Openbook.Domain.Aggregates.Reviews;

namespace Openbook.GraphQL.Reviews;

[ExtendObjectType("Query")]
public class ReviewQueries
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Review> GetReviews(ApplicationDbContext db)
    {
        return db.Reviews.AsQueryable();
    }

    public async Task<Review?> GetReviewById(IDbContextFactory<ApplicationDbContext> factory, ReviewId id)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Reviews.FindAsync(id);
    }
}
