namespace CQRS.Core.Consumer;

public interface IEventConsumer
{
    void Consume(string topic);
}