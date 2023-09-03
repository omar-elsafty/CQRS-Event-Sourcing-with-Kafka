using CQRS.Core.Events;

namespace CQRS.Core.Infrastructure;

public interface IEventStore
{
    Task SaveEventsAsync(Guid aggregateId,IEnumerable<BaseEvent> events,int excpectedVersion);
    Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
}