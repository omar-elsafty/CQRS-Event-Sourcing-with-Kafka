using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Cmd.Infrastructure.Config;

namespace Post.Cmd.Infrastructure.Repositories;

public class EventStoreRepository: IEventStoreRepository
{
    private readonly MonogoDbConfig _mongoDbConfig;
    private readonly IMongoCollection<EventModel> _eventStoreCollectionl;
    
    public EventStoreRepository(IOptions<MonogoDbConfig> mongoDbConfig)
    {
        _mongoDbConfig = mongoDbConfig.Value;
        var mongoClient = new MongoClient(_mongoDbConfig.ConnectionString);
        var mongoDataBase = mongoClient.GetDatabase(_mongoDbConfig.Database);
        _eventStoreCollectionl = mongoDataBase.GetCollection<EventModel>(_mongoDbConfig.Collection);
    }
    
    public async Task SaveAsync(EventModel @event)
    {
        await _eventStoreCollectionl.InsertOneAsync(@event);
    }

    public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
    {
        return await _eventStoreCollectionl.Find(x => x.AggregateIdentifier == aggregateId).ToListAsync();
    }
}