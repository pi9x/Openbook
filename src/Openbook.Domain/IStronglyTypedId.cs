namespace Openbook.Domain;

public interface IStronglyTypedId
{
    public Guid Value { get; }
}