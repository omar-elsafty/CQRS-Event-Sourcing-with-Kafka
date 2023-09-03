using CQRS.Core.Events;

namespace CQRS.Core.Domain;

public abstract class AggregateRoot
{
    protected Guid _id;
    private readonly List<BaseEvent> _changes = new();

    public Guid Id => _id;
    public int Version { get; set; } = -1;

    public IEnumerable<BaseEvent> GetUnCommittedChanges() => _changes;

    public void MarkChangesAsCommitted()
    {
        _changes.Clear();
    }

    private void ApplyChanges(BaseEvent @event, bool isNew)
    {
        var method = GetType().GetMethod("Apply", new[] { @event.GetType() });
        if (method == null)
            throw new ArgumentNullException(nameof(method),
                $"This method wasn't found in the aggregate for {@event.GetType().Name}");

        method.Invoke(this, new[] { @event });
        if (isNew)
        {
            _changes.Add(@event);
        }
    }

    protected void RaiseEvent(BaseEvent @event)
    {
        ApplyChanges(@event, true);
    }

    public void ReplayEvents(IEnumerable<BaseEvent> events)
    {
        foreach (var @event in events)
        {
            ApplyChanges(@event, false);
        }
    }
}