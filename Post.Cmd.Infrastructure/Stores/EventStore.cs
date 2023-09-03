using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Cmd.Core.Aggregates;

namespace Post.Cmd.Infrastructure.Stores;

public class EventStore : IEventStore
{
    private IEventStoreRepository _eventStoreRepository;
    private IEventProducer _eventProducer;

    public EventStore(IEventStoreRepository eventStoreRepository,IEventProducer eventProducer)
    {
        _eventStoreRepository = eventStoreRepository;
        _eventProducer = eventProducer;
    }

    public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int excpectedVersion)
    {
        var eventsStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
        if (excpectedVersion != -1 && eventsStream[^1].Version != excpectedVersion)
            throw new Exception("Concurrency Exception");
        
        var version = excpectedVersion;

        foreach (var @event in events)
        {
            version++;
            @event.Version = version;

            var eventType = @event.GetType().Name;
            var eventModel = new EventModel
            {
                TimeStamp = DateTime.UtcNow,
                AggregateIdentifier = aggregateId,
                AggregateType = nameof(PostAggregate),
                Version = version,
                EventType = eventType,
                EventData = @event
            };
            await _eventStoreRepository.SaveAsync(eventModel);
            var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
            await _eventProducer.ProduceAsync(topic, @event);   
        }
    }

    public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
    {
        var eventsStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
        if (!eventsStream.Any())
        {
            throw new Exception("Incorrect post Id");
        }

        return eventsStream.OrderBy(e => e.Version).Select(e => e.EventData).ToList();
    }
}