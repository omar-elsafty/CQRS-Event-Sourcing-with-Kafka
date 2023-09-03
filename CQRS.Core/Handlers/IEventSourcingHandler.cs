using CQRS.Core.Domain;

namespace CQRS.Core.Handlers;

public interface IEventSourcingHandler<T>
{
    Task SaveAsync(AggregateRoot aggregate);
    
    Task<T> GetByAggregateIdAsync(Guid aggregateId);
}