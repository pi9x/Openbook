namespace Openbook.Domain.Aggregates.Authors;

public readonly record struct AuthorId(Guid Value) : IStronglyTypedId;