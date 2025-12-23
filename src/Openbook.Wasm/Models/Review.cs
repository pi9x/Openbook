namespace Openbook.Wasm.Models;

public class Review
{
    public Guid ReviewId { get; set; }
    public Guid BookId { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; } = null!;
}