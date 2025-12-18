namespace Openbook.Domain.Aggregates.Books;

public class Book
{
    public required BookId BookId { get; set; }
    public required Authors.Author Author { get; set; }
    public required string Title { get; set; }
    public ICollection<Reviews.Review> Reviews { get; set; } = [];
    
    public void AddReview(Reviews.Review review)
    {
        Reviews.Add(review);
    }
}