namespace Openbook.Domain.Aggregates.Authors;

public class Author
{
    public required AuthorId AuthorId { get; set; }
    public required string Name { get; set; }
    public string? Bio { get; set; }
}