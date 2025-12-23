namespace Openbook.Wasm.Models;

public class Book
{
    public Guid BookId { get; set; }
    public string? Title { get; set; }
    public Author? Author { get; set; }
    public List<Review>? Reviews { get; set; }
}