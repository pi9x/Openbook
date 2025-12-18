namespace Openbook.Domain.Aggregates.Reviews;

public readonly record struct ReviewId(Guid Value) : IStronglyTypedId;