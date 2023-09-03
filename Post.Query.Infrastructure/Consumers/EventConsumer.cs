using System.Text.Json;
using Confluent.Kafka;
using CQRS.Core.Consumer;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Converter;
using Post.Query.Infrastructure.Handlers;

namespace Post.Query.Infrastructure.Consumers;

public class EventConsumer : IEventConsumer
{
    private readonly IEventHandler _eventHandler;
    private readonly ConsumerConfig _config;

    public EventConsumer(IOptions<ConsumerConfig> config, IEventHandler eventHandler)
    {
        _eventHandler = eventHandler;
        _config = config.Value;
    }

    public void Consume(string topic)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();

        consumer.Subscribe(topic);

        while (true)
        {
            var consumeResult = consumer.Consume();
            if (consumeResult?.Message == null) continue;
            var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
            var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);
            var handlerMethod = _eventHandler.GetType().GetMethod("On", new[] { @event.GetType() });
            if (handlerMethod == null)
                throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method!");

            handlerMethod.Invoke(_eventHandler, new object?[] { @event });
            consumer.Commit(consumeResult);

        }
    }
}