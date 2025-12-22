namespace Openbook.Domain.Aggregates.Books;

public readonly record struct BookId(Guid Value) : IStronglyTypedId;