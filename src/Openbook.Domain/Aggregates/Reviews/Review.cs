using System.ComponentModel.DataAnnotations;

namespace Openbook.Domain.Aggregates.Reviews;

public class Review
{
    public required ReviewId ReviewId { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
    public string? Content { get; set; }
}