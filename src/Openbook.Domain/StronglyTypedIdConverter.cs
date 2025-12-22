using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Openbook.Domain;

public class StronglyTypedIdConverter<TId>() : ValueConverter<TId, Guid>(id => id.Value, value => (TId)Activator.CreateInstance(typeof(TId), value)!)
    where TId : struct, IStronglyTypedId;